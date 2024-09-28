using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace McProtoNet.Benchmark;

public static class Impl
{
    public static Vector128<byte> ShuffleUnsafe(Vector128<byte> values, Vector128<byte> indices)
    {
        if (Ssse3.IsSupported) return Ssse3.Shuffle(values, indices);
        return Vector128.Shuffle(values, indices);
    }

    public static Vector256<byte> ShuffleUnsafe(this Vector256<byte> values, Vector256<byte> indices)
    {
        if (Avx2.IsSupported)
        {
            var indicesXord = Avx2.And(Avx2.Xor(indices, Vector256.Create(Vector128.Create((byte)0), Vector128.Create((byte)0x10))), Vector256.Create((byte)0x9F));
            var swap = Avx2.Permute2x128(values, values, 0b00000001);
            var shuf1 = Avx2.Shuffle(values, indices);
            var shuf2 = Avx2.Shuffle(swap, indices);
            var selection = Avx2.CompareGreaterThan(indicesXord.AsSByte(), Vector256.Create((sbyte)0x0F)).AsByte();
            return Avx2.BlendVariable(shuf1, shuf2, selection);
        }
        return Vector256.Shuffle(values, indices);
    }

}