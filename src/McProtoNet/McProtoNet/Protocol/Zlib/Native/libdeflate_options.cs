using System.Runtime.CompilerServices;

namespace McProtoNet.Protocol.Zlib.Native;

internal readonly struct libdeflate_options
{
    private static readonly UIntPtr Size = (nuint)(nint)Unsafe.SizeOf<libdeflate_options>();

    public libdeflate_options(CustomMemoryAllocator.malloc_func malloc, CustomMemoryAllocator.free_func free)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(malloc);
        ArgumentNullException.ThrowIfNull(free);
#else
        ThrowIfNull(malloc);
        ThrowIfNull(free);
#endif

        this.sizeof_options = Size;
        this.malloc = malloc;
        this.free = free;

#if !NET6_0_OR_GREATER
        static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName
 = null)
        {
            if(argument is null)
            {
                ThrowHelperArgumentNull(paramName!);
            }

            [DoesNotReturn]
            static void ThrowHelperArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        }
#endif
    }


    public readonly UIntPtr sizeof_options;


    public readonly CustomMemoryAllocator.malloc_func malloc;


    public readonly CustomMemoryAllocator.free_func free;
}