// <copyright file="InstrumentationDefinitions.cs" company="OpenTelemetry Authors">
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

namespace OpenTelemetry.AutoInstrumentation;

internal static partial class InstrumentationDefinitions
{
    private static string assemblyFullName = typeof(InstrumentationDefinitions).Assembly.FullName!;

    internal static Payload GetAllDefinitions()
    {
        return new Payload
        {
            // Fixed Id for definitions payload (to avoid loading same integrations from multiple AppDomains)
            DefinitionsId = "FFAFA5168C4F4718B40CA8788875C2DA",

            // Autogenerated definitions array
            Definitions = GetDefinitionsArray(),
        };
    }

    internal static Payload GetDerivedDefinitions()
    {
        return new Payload
        {
            // Fixed Id for definitions payload (to avoid loading same integrations from multiple AppDomains)
            DefinitionsId = "61BF627FA9B5477F85595A9F0D68B29C",

            // Autogenerated definitions array
            Definitions = GetDerivedDefinitionsArray(),
        };
    }

    // TODO: Generate this list using source generators
    private static NativeCallTargetDefinition[] GetDefinitionsArray()
        => new NativeCallTargetDefinition[]
        {
            // GraphQL
            new("GraphQL", "Trace", "GraphQL", "GraphQL.Execution.ExecutionStrategy", "ExecuteAsync",  new[] { "System.Threading.Tasks.Task`1<GraphQL.ExecutionResult>", "GraphQL.Execution.ExecutionContext" }, 2, 3, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.GraphQL.ExecuteAsyncIntegration"),
            new("GraphQL", "Trace", "GraphQL", "GraphQL.Execution.SubscriptionExecutionStrategy", "ExecuteAsync",  new[] { "System.Threading.Tasks.Task`1<GraphQL.ExecutionResult>", "GraphQL.Execution.ExecutionContext" }, 2, 3, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.GraphQL.ExecuteAsyncIntegration"),

            // ILogger
            new("ILogger", "Log", "Microsoft.Extensions.Logging", "Microsoft.Extensions.Logging.LoggingBuilder", ".ctor",  new[] { "System.Void", "Microsoft.Extensions.DependencyInjection.IServiceCollection" }, 3, 1, 0, 7, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.Logger.LoggingBuilderIntegration"),

            // MongoDB
            new("MongoDB", "Trace", "MongoDB.Driver", "MongoDB.Driver.MongoClient", ".ctor",  new[] { "System.Void", "MongoDB.Driver.MongoClientSettings" }, 2, 13, 3, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.MongoDB.MongoClientIntegration"),

            // MySqlData
            new("MySqlData", "Trace", "MySql.Data", "MySql.Data.MySqlClient.MySqlConnectionStringBuilder", "get_Logging",  new[] { "System.Boolean" }, 8, 0, 31, 8, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.MySqlData.MySqlConnectionStringBuilderIntegration"),

            // NServiceBus
            new("NServiceBus", "Trace", "NServiceBus.Core", "NServiceBus.EndpointConfiguration", ".ctor",  new[] { "System.Void", "System.String" }, 8, 0, 0, 8, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.NServiceBus.EndpointConfigurationIntegration"),
            new("NServiceBus", "Metric", "NServiceBus.Core", "NServiceBus.EndpointConfiguration", ".ctor",  new[] { "System.Void", "System.String" }, 8, 0, 0, 8, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.NServiceBus.EndpointConfigurationIntegration"),

            // StackExchangeRedis
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImpl",  new[] { "StackExchange.Redis.ConnectionMultiplexer", "System.Object", "System.IO.TextWriter" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegration"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImpl",  new[] { "StackExchange.Redis.ConnectionMultiplexer", "StackExchange.Redis.ConfigurationOptions", "System.IO.TextWriter" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegration"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImpl",  new[] { "StackExchange.Redis.ConnectionMultiplexer", "StackExchange.Redis.ConfigurationOptions", "System.IO.TextWriter", "System.Nullable`1[StackExchange.Redis.ServerType]" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegration"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImpl",  new[] { "StackExchange.Redis.ConnectionMultiplexer", "StackExchange.Redis.ConfigurationOptions", "System.IO.TextWriter", "System.Nullable`1[StackExchange.Redis.ServerType]", "StackExchange.Redis.EndPointCollection" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegration"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImplAsync",  new[] { "System.Threading.Tasks.Task`1<StackExchange.Redis.ConnectionMultiplexer>", "System.Object", "System.IO.TextWriter" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegrationAsync"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImplAsync",  new[] { "System.Threading.Tasks.Task`1<StackExchange.Redis.ConnectionMultiplexer>", "StackExchange.Redis.ConfigurationOptions", "System.IO.TextWriter" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegrationAsync"),
            new("StackExchangeRedis", "Trace", "StackExchange.Redis", "StackExchange.Redis.ConnectionMultiplexer", "ConnectImplAsync",  new[] { "System.Threading.Tasks.Task`1<StackExchange.Redis.ConnectionMultiplexer>", "StackExchange.Redis.ConfigurationOptions", "System.IO.TextWriter", "System.Nullable`1[StackExchange.Redis.ServerType]" }, 2, 0, 0, 2, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.StackExchangeRedis.StackExchangeRedisIntegrationAsync"),

            // WcfClient
            new("WcfClient", "Trace", "System.ServiceModel", "System.ServiceModel.ChannelFactory", "InitializeEndpoint",  new[] { "System.Void", "System.String", "System.ServiceModel.EndpointAddress" }, 4, 0, 0, 4, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.Wcf.WcfClientIntegration"),
            new("WcfClient", "Trace", "System.ServiceModel", "System.ServiceModel.ChannelFactory", "InitializeEndpoint",  new[] { "System.Void", "System.String", "System.ServiceModel.EndpointAddress", "System.Configuration.Configuration" }, 4, 0, 0, 4, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.Wcf.WcfClientIntegration"),
            new("WcfClient", "Trace", "System.ServiceModel", "System.ServiceModel.ChannelFactory", "InitializeEndpoint",  new[] { "System.Void", "System.ServiceModel.Description.ServiceEndpoint" }, 4, 0, 0, 4, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.Wcf.WcfClientIntegration"),
            new("WcfClient", "Trace", "System.ServiceModel", "System.ServiceModel.ChannelFactory", "InitializeEndpoint",  new[] { "System.Void", "System.ServiceModel.Channels.Binding", "System.ServiceModel.EndpointAddress" }, 4, 0, 0, 4, 65535, 65535, assemblyFullName, "OpenTelemetry.AutoInstrumentation.Instrumentations.Wcf.WcfClientIntegration"),
        };

    // TODO: Generate this list using source generators
    private static NativeCallTargetDefinition[] GetDerivedDefinitionsArray()
        => new NativeCallTargetDefinition[]
        {
        };

    internal struct Payload
    {
        public string DefinitionsId { get; set; }

        public NativeCallTargetDefinition[] Definitions { get; set; }
    }
}
