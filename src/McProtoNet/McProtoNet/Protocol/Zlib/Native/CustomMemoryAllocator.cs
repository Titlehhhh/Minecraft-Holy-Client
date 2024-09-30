using System.Runtime.InteropServices;

namespace McProtoNet.Protocol.Zlib.Native;

internal static class CustomMemoryAllocator
{
    public delegate IntPtr malloc_func(UIntPtr size);


    public delegate void free_func(IntPtr alloc);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_set_memory_allocator(malloc_func malloc, free_func free);
}