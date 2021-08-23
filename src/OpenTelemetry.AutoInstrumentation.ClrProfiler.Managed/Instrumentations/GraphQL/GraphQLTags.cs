using System.Diagnostics;
using System.Linq;
using OpenTelemetry.AutoInstrumentation.ClrProfiler.Managed.Tagging;
using OpenTelemetry.AutoInstrumentation.ClrProfiler.Managed.Util;

namespace OpenTelemetry.AutoInstrumentation.ClrProfiler.Managed.Instrumentations.GraphQL
{
    internal class GraphQLTags : InstrumentationTags
    {
        protected static readonly IProperty<string>[] GraphQLTagsProperties =
            InstrumentationTagsProperties.Concat(
                new ReadOnlyProperty<GraphQLTags, string>(Tags.InstrumentationName, t => t.InstrumentationName),
                new Property<GraphQLTags, string>(Tags.GraphQL.Source, t => t.Source, (t, v) => t.Source = v),
                new Property<GraphQLTags, string>(Tags.GraphQL.OperationName, t => t.OperationName, (t, v) => t.OperationName = v),
                new Property<GraphQLTags, string>(Tags.GraphQL.OperationType, t => t.OperationType, (t, v) => t.OperationType = v),
                new ReadOnlyProperty<GraphQLTags, string>(Tags.Language, t => t.Language));

        public override ActivityKind Kind => ActivityKind.Server;

        public string InstrumentationName => GraphQLCommon.IntegrationName;

        public string Language => TracerConstants.Language;

        public string Source { get; set; }

        public string OperationName { get; set; }

        public string OperationType { get; set; }

        protected override IProperty<string>[] GetAdditionalTags() => GraphQLTagsProperties;
    }
}