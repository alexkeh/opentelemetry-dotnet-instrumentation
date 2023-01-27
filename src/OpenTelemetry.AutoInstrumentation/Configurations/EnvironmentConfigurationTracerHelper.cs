// <copyright file="EnvironmentConfigurationTracerHelper.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Runtime.CompilerServices;
using OpenTelemetry.AutoInstrumentation.Loading;
using OpenTelemetry.AutoInstrumentation.Loading.Initializers;
using OpenTelemetry.AutoInstrumentation.Plugins;
using OpenTelemetry.Trace;

namespace OpenTelemetry.AutoInstrumentation.Configurations;

internal static class EnvironmentConfigurationTracerHelper
{
    public static TracerProviderBuilder UseEnvironmentVariables(
        this TracerProviderBuilder builder,
        LazyInstrumentationLoader lazyInstrumentationLoader,
        TracerSettings settings,
        PluginManager pluginManager)
    {
        foreach (var enabledInstrumentation in settings.EnabledInstrumentations)
        {
            _ = enabledInstrumentation switch
            {
                TracerInstrumentation.AspNet => Wrappers.AddAspNetInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.GrpcNetClient => Wrappers.AddGrpcClientInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.HttpClient => Wrappers.AddHttpClientInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.Npgsql => builder.AddSource("Npgsql"),
                TracerInstrumentation.SqlClient => Wrappers.AddSqlClientInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.Wcf => Wrappers.AddWcfInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.NServiceBus => builder.AddSource("NServiceBus.Core"),
                TracerInstrumentation.Elasticsearch => builder.AddSource("Elastic.Clients.Elasticsearch.ElasticsearchClient"),
                TracerInstrumentation.Quartz => Wrappers.AddQuartzInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
#if NET6_0_OR_GREATER
                TracerInstrumentation.MassTransit => builder.AddSource("MassTransit"),
                TracerInstrumentation.MongoDB => builder.AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources"),
                TracerInstrumentation.MySqlData => Wrappers.AddMySqlClientInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
                TracerInstrumentation.StackExchangeRedis => builder.AddSource("OpenTelemetry.Instrumentation.StackExchangeRedis"),
                TracerInstrumentation.EntityFrameworkCore => Wrappers.AddEntityFrameworkCoreInstrumentation(builder, pluginManager, lazyInstrumentationLoader),
#endif
                _ => null
            };
        }

        builder
            .SetSampler(settings)
            // Exporters can cause dependency loads.
            // Should be called later if dependency listeners are already setup.
            .SetExporter(settings, pluginManager)
            .AddSource(settings.ActivitySources.ToArray());

        foreach (var legacySource in settings.LegacySources)
        {
            builder.AddLegacySource(legacySource);
        }

        return builder;
    }

    private static TracerProviderBuilder SetSampler(this TracerProviderBuilder builder, TracerSettings settings)
    {
        if (settings.TracesSampler == null)
        {
            return builder;
        }

        var sampler = TracerSamplerHelper.GetSampler(settings.TracesSampler, settings.TracesSamplerArguments);

        if (sampler == null)
        {
            return builder;
        }

        return builder.SetSampler(sampler);
    }

    private static TracerProviderBuilder SetExporter(this TracerProviderBuilder builder, TracerSettings settings, PluginManager pluginManager)
    {
        if (settings.ConsoleExporterEnabled)
        {
            Wrappers.AddConsoleExporter(builder, pluginManager);
        }

        return settings.TracesExporter switch
        {
            TracesExporter.Zipkin => Wrappers.AddZipkinExporter(builder, pluginManager),
            TracesExporter.Otlp => Wrappers.AddOtlpExporter(builder, settings, pluginManager),
            TracesExporter.None => builder,
            _ => throw new ArgumentOutOfRangeException($"Traces exporter '{settings.TracesExporter}' is incorrect")
        };
    }

    /// <summary>
    /// This class wraps external extension methods to ensure the dlls are not loaded, if not necessary.
    /// .NET Framework is aggressively inlining these wrappers. Inlining must be disabled to ensure the wrapping effect.
    /// </summary>
    private static class Wrappers
    {
        // Instrumentations

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddWcfInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddWcf(lazyInstrumentationLoader, pluginManager);

            return builder.AddSource("OpenTelemetry.Instrumentation.Wcf");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddHttpClientInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddHttpClient(lazyInstrumentationLoader, pluginManager);

#if NETFRAMEWORK
            builder.AddSource("OpenTelemetry.Instrumentation.Http.HttpWebRequest");
#else
            builder.AddSource("OpenTelemetry.Instrumentation.Http.HttpClient");
            builder.AddSource("System.Net.Http"); // This works only System.Net.Http >= 7.0.0
            builder.AddLegacySource("System.Net.Http.HttpRequestOut");
#endif

            return builder;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddAspNetInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddAspNet(lazyInstrumentationLoader, pluginManager);
#if NET462
            builder.AddSource(OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule.AspNetSourceName);
#elif NET6_0_OR_GREATER
            builder.AddSource("OpenTelemetry.Instrumentation.AspNetCore");
            builder.AddLegacySource("Microsoft.AspNetCore.Hosting.HttpRequestIn");
#endif
            return builder;
        }

#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddMySqlClientInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddMySqlClient(lazyInstrumentationLoader, pluginManager);

            return builder.AddSource("OpenTelemetry.Instrumentation.MySqlData");
        }
#endif

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddSqlClientInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddSqlClient(lazyInstrumentationLoader, pluginManager);

            return builder.AddSource("OpenTelemetry.Instrumentation.SqlClient");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddGrpcClientInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddGrpcClient(lazyInstrumentationLoader, pluginManager);

            builder.AddSource("OpenTelemetry.Instrumentation.GrpcNetClient");
            builder.AddLegacySource("Grpc.Net.Client.GrpcOut");

            return builder;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddQuartzInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddQuartz(lazyInstrumentationLoader, pluginManager);

            return builder.AddSource("OpenTelemetry.Instrumentation.Quartz")
                .AddLegacySource("Quartz.Job.Execute")
                .AddLegacySource("Quartz.Job.Veto");
        }

#if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddEntityFrameworkCoreInstrumentation(TracerProviderBuilder builder, PluginManager pluginManager, LazyInstrumentationLoader lazyInstrumentationLoader)
        {
            DelayedInitialization.Traces.AddEntityFrameworkCore(lazyInstrumentationLoader, pluginManager);

            return builder.AddSource("OpenTelemetry.Instrumentation.EntityFrameworkCore");
        }
#endif

        // Exporters

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddConsoleExporter(TracerProviderBuilder builder, PluginManager pluginManager)
        {
            return builder.AddConsoleExporter(pluginManager.ConfigureTracesOptions);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddZipkinExporter(TracerProviderBuilder builder, PluginManager pluginManager)
        {
            return builder.AddZipkinExporter(pluginManager.ConfigureTracesOptions);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static TracerProviderBuilder AddOtlpExporter(TracerProviderBuilder builder, TracerSettings settings, PluginManager pluginManager)
        {
            return builder.AddOtlpExporter(options =>
            {
                if (settings.OtlpExportProtocol.HasValue)
                {
                    options.Protocol = settings.OtlpExportProtocol.Value;
                }

                pluginManager.ConfigureTracesOptions(options);
            });
        }
    }
}