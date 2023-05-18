/*
 * Copyright The OpenTelemetry Authors
 * SPDX-License-Identifier: Apache-2.0
 */

#ifndef BYTECODE_INSTRUMENTATIONS_H
#define BYTECODE_INSTRUMENTATIONS_H

#include "string.h"

namespace trace
{
inline std::unordered_map<WSTRING, WSTRING> trace_integration_names =
    {
    {WStr("StrongNamedValidation"), WStr("OTEL_DOTNET_AUTO_TRACES_STRONGNAMEDVALIDATION_INSTRUMENTATION_ENABLED")},
    {WStr("StackExchangeRedis"), WStr("OTEL_DOTNET_AUTO_TRACES_STACKEXCHANGEREDIS_INSTRUMENTATION_ENABLED")},
    {WStr("NServiceBus"), WStr("OTEL_DOTNET_AUTO_TRACES_NSERVICEBUS_INSTRUMENTATION_ENABLED")},
    {WStr("MySqlData"), WStr("OTEL_DOTNET_AUTO_TRACES_MYSQLDATA_INSTRUMENTATION_ENABLED")},
    {WStr("MongoDB"), WStr("OTEL_DOTNET_AUTO_TRACES_MONGODB_INSTRUMENTATION_ENABLED")},
    {WStr("GraphQL"), WStr("OTEL_DOTNET_AUTO_TRACES_GRAPHQL_INSTRUMENTATION_ENABLED")},
    {WStr("WcfClient"), WStr("OTEL_DOTNET_AUTO_TRACES_WCFCLIENT_INSTRUMENTATION_ENABLED")}
    };
inline std::unordered_map<WSTRING, WSTRING> metric_integration_names = {{WStr("NServiceBus"), WStr("OTEL_DOTNET_AUTO_METRICS_NSERVICEBUS_INSTRUMENTATION_ENABLED")}};
inline std::unordered_map<WSTRING, WSTRING> log_integration_names = {{WStr("ILogger"), WStr("OTEL_DOTNET_AUTO_LOGS_ILOGGER_INSTRUMENTATION_ENABLED")}};
}
#endif
