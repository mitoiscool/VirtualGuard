namespace VirtualGuard.Tests;

public class CRC32
{
    public static uint ComputeHash(byte[] buffer, uint polynomial, uint seed, uint xorMask)
    {
        uint[] table = new uint[256];

        for (uint i = 0; i < 256; i++)
        {
            uint entry = i;
            for (int j = 0; j < 8; j++)
            {
                entry = (entry & 1) == 1
                    ? (entry >> 1) ^ polynomial
                    : entry >> 1;

                // Additional XOR to make the algorithm more unique
                entry ^= (entry >> 12) ^ (entry >> 24);
            }

            table[i] = entry;
        }

        uint hash = seed;

        for (int i = 0; i < buffer.Length; i++)
        {
            hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
        }

        return ~hash ^ xorMask;
    }
}