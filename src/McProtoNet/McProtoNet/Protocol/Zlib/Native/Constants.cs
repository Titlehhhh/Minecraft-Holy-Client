using System.Runtime.InteropServices;

namespace McProtoNet.Net.Zlib.Native;

internal static class Constants
{
    public const string DllName = "libdeflate";

    public const CallingConvention CallConv = CallingConvention.Cdecl;
}