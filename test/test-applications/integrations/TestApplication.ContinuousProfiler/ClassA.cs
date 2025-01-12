// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;

namespace My.Custom.Test.Namespace;

internal static class ClassA
{
    private static readonly Callback TestCallback = GenericClassC<int>.GenericMethodCFromGenericClass;

    private delegate int Callback(int n);

    public static void MethodA()
    {
        MethodABytes(
            false,
            '\0',
            sbyte.MaxValue,
            byte.MaxValue);
    }

    public static void MethodABytes(
        bool b,
        char c,
        sbyte sb,
        byte b2)
    {
        MethodAInts(
            ushort.MaxValue,
            short.MaxValue,
            uint.MaxValue,
            int.MaxValue,
            ulong.MaxValue,
            long.MaxValue,
            nint.MaxValue,
            nuint.MaxValue);
    }

    public static void MethodAInts(
        ushort ui16,
        short i16,
        uint ui32,
        int i32,
        ulong ui64,
        long i64,
        nint nint,
        nuint nuint)
    {
        MethodAFloats(float.MaxValue, double.MaxValue);
    }

    public static unsafe void MethodAFloats(
        float fl,
        double db)
    {
        int a = 1;
        int* pointer = &a;
        MethodAPointer(pointer);
    }

    public static unsafe void MethodAPointer(int* pointer)
    {
        MethodAOthers(
            string.Empty,
            new object(),
            new CustomClass(),
            default,
            Array.Empty<CustomClass>(),
            Array.Empty<CustomStruct>(),
            new List<string>());
    }

    public static void MethodAOthers<T>(
        string s,
        object obj,
        CustomClass customClass,
        CustomStruct customStruct,
        CustomClass[] classArray,
        CustomStruct[] structArray,
        List<T> genericList)
    {
        Action(3);
        return;

        static void Action(int s) => InternalClassB<string, int>.DoubleInternalClassB.TripleInternalClassB<int>.MethodB(s, new[] { 3 }, TimeSpan.Zero, 0, new List<string> { "a" }, new List<string>(0));
    }

    [DllImport("TestApplication.ContinuousProfiler.NativeDep")]
    private static extern int OTelAutoCallbackTest(Callback fp, int n);

    internal static class InternalClassB<TA, TD>
    {
        internal static class DoubleInternalClassB
        {
            internal static class TripleInternalClassB<TC>
            {
                public static void MethodB<TB>(int testArg, TC[] a, TB b, TD t, IList<TA> c, IList<string> d)
                {
                    OTelAutoCallbackTest(TestCallback, testArg);
                }
            }
        }
    }
}
