// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using OpenTelemetry.ResourceDetectors.Azure;
using OpenTelemetry.ResourceDetectors.Container;
using OpenTelemetry.ResourceDetectors.ProcessRuntime;
using OpenTelemetry.Resources;

namespace OpenTelemetry.AutoInstrumentation.Configurations;

internal static class ResourceConfigurator
{
    internal const string ServiceNameAttribute = "service.name";

    public static ResourceBuilder CreateResourceBuilder(IReadOnlyList<ResourceDetector> enabledResourceDetectors)
    {
        var resourceBuilder = ResourceBuilder
            .CreateEmpty() // Don't use CreateDefault because it puts service name unknown by default.
            .AddEnvironmentVariableDetector()
            .AddTelemetrySdk()
            .AddAttributes(new KeyValuePair<string, object>[]
            {
                new(Constants.DistributionAttributes.TelemetryDistroNameAttributeName, Constants.DistributionAttributes.TelemetryDistroNameAttributeValue),
                new(Constants.DistributionAttributes.TelemetryDistroVersionAttributeName, AutoInstrumentationVersion.Version)
            });

        foreach (var enabledResourceDetector in enabledResourceDetectors)
        {
            resourceBuilder = enabledResourceDetector switch
            {
                ResourceDetector.Container => Wrappers.AddContainerResourceDetector(resourceBuilder),
                ResourceDetector.AzureAppService => Wrappers.AddAzureAppServiceResourceDetector(resourceBuilder),
                ResourceDetector.ProcessRuntime => Wrappers.AddProcessRuntimeResourceDetector(resourceBuilder),
                _ => resourceBuilder
            };
        }

        var pluginManager = Instrumentation.PluginManager;
        if (pluginManager != null)
        {
            resourceBuilder.InvokePlugins(pluginManager);
        }

        var resource = resourceBuilder.Build();
        if (!resource.Attributes.Any(kvp => kvp.Key == ServiceNameAttribute))
        {
            // service.name was not configured yet use the fallback.
            resourceBuilder.AddAttributes(new KeyValuePair<string, object>[] { new(ServiceNameAttribute, ServiceNameConfigurator.GetFallbackServiceName()) });
        }

        return resourceBuilder;
    }

    private static class Wrappers
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ResourceBuilder AddContainerResourceDetector(ResourceBuilder resourceBuilder)
        {
            return resourceBuilder.AddDetector(new ContainerResourceDetector());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ResourceBuilder AddAzureAppServiceResourceDetector(ResourceBuilder resourceBuilder)
        {
            return resourceBuilder.AddDetector(new AppServiceResourceDetector());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ResourceBuilder AddProcessRuntimeResourceDetector(ResourceBuilder resourceBuilder)
        {
            return resourceBuilder.AddDetector(new ProcessRuntimeDetector());
        }
    }
}
