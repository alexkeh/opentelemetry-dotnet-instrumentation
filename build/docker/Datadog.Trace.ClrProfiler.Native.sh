#!/bin/bash
set -euxo pipefail

uname_os() {
    os=$(uname -s | tr '[:upper:]' '[:lower:]')
    case "$os" in
        cygwin_nt*) echo "windows" ;;
        mingw*) echo "windows" ;;
        msys_nt*) echo "windows" ;;
        *) echo "$os" ;;
    esac
}

native_sufix() {
    os=$(uname_os)
    case "$os" in
        windows*) echo "dll" ;;
        linux*) echo "so" ;;
        darwin*) echo "dylib" ;;
        *) echo "OS: ${os} is not supported" ; exit 1 ;;
    esac
}

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"
cd "$DIR/../.."

BUILD_TYPE=${buildConfiguration:-Debug}
OUTDIR="$( pwd )/src/Datadog.Trace.ClrProfiler.Native/bin/${BUILD_TYPE}/x64"

# If running the unified pipeline, do not copy managed assets yet. Do so during the package build step
if [ -z "${UNIFIED_PIPELINE-}" ]; then
  PUBLISH_OUTPUT_MANAGED="$( pwd )/src/bin/managed-publish"

  mkdir -p ${OUTDIR}/
  cp -fr ${PUBLISH_OUTPUT_MANAGED}/* ${OUTDIR}/
fi

os=$(uname_os)
case "$os" in
 windows*)
    msbuild.exe Datadog.Trace.proj -t:BuildCpp -p:TracerHomeDirectory=${OUTDIR} -p:Configuration=${BUILD_TYPE} -p:Platform=x64
    ;;

 *)
    cd src/Datadog.Trace.ClrProfiler.Native

    mkdir -p build
    (cd build && cmake ../ -DCMAKE_BUILD_TYPE=${BUILD_TYPE}  && make)

    SUFIX=$(native_sufix)
    mkdir -p bin/${BUILD_TYPE}/x64
    cp -f build/bin/Datadog.Trace.ClrProfiler.Native.${SUFIX} ${OUTDIR}/OpenTelemetry.AutoInstrumentation.ClrProfiler.Native.${SUFIX}
esac
