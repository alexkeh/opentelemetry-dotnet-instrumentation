using System;
using System.Collections.Generic;
using System.Threading;
using Datadog.Trace.Configuration;
using Datadog.Trace.DiagnosticListeners;
using Datadog.Trace.Logging;
using Datadog.Trace.ServiceFabric;

namespace Datadog.Trace.ClrProfiler
{
    /// <summary>
    /// Provides access to the profiler CLSID and whether it is attached to the process.
    /// </summary>
    public static class Instrumentation
    {
        /// <summary>
        /// Indicates whether we're initializing Instrumentation for the first time
        /// </summary>
        private static int _firstInitialization = 1;

        /// <summary>
        /// Gets the CLSID for the Datadog .NET profiler
        /// </summary>
        public static readonly string ProfilerClsid = "{918728DD-259F-4A6A-AC2B-B85E1B658318}";

        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor(typeof(Instrumentation));

        /// <summary>
        /// Gets a value indicating whether Datadog's profiler is attached to the current process.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the profiler is currently attached; <c>false</c> otherwise.
        /// </value>
        public static bool ProfilerAttached
        {
            get
            {
                try
                {
                    return NativeMethods.IsProfilerAttached();
                }
                catch (DllNotFoundException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Initializes global instrumentation values.
        /// </summary>
        public static void Initialize()
        {
            if (Interlocked.Exchange(ref _firstInitialization, 0) != 1)
            {
                // Initialize() was already called before
                return;
            }

            try
            {
                // ensure global instance is created if it's not already
                _ = Tracer.Instance;
            }
            catch
            {
                // ignore
            }

#if !NETFRAMEWORK
            try
            {
                if (GlobalSettings.Source.DiagnosticSourceEnabled)
                {
                    // check if DiagnosticSource is available before trying to use it
                    var type = Type.GetType("System.Diagnostics.DiagnosticSource, System.Diagnostics.DiagnosticSource", throwOnError: false);

                    if (type == null)
                    {
                        Log.Warning("DiagnosticSource type could not be loaded. Skipping diagnostic observers.");
                    }
                    else
                    {
                        // don't call this method unless DiagnosticSource is available
                        StartDiagnosticManager();
                    }
                }
            }
            catch
            {
                // ignore
            }

            // we only support Service Fabric Service Remoting instrumentation on .NET Core (including .NET 5+)
            if (string.Equals(FrameworkDescription.Instance.Name, ".NET Core", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(FrameworkDescription.Instance.Name, ".NET", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    ServiceRemotingClient.StartTracing();
                }
                catch
                {
                    // ignore
                }

                try
                {
                    ServiceRemotingService.StartTracing();
                }
                catch
                {
                    // ignore
                }
            }
#endif
        }

#if !NETFRAMEWORK
        private static void StartDiagnosticManager()
        {
            var observers = new List<DiagnosticObserver> { new AspNetCoreDiagnosticObserver() };
            var diagnosticManager = new DiagnosticManager(observers);
            diagnosticManager.Start();

            DiagnosticManager.Instance = diagnosticManager;
        }
#endif
    }
}
