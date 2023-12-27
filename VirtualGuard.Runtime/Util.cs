using System.Text;
using VirtualGuard.Runtime.Dynamic;

namespace VirtualGuard.Runtime;

public class Util
{
    public static long Hash(byte[] buffer)
    {
        uint[] table = new uint[256];

        for (uint i = 0; i < 256; i++)
        {
            uint entry = i;
            for (int j = 0; j < 8; j++)
            {
                entry = (entry & 1) == 1
                    ? (entry >> 1) ^ Constants.SPolynomial
                    : entry >> 1;
                
                entry ^= (entry >> 12) ^ (entry >> 24);
            }

            table[i] = entry;
        }

        uint hash = Constants.SSeed;

        for (int i = 0; i < buffer.Length; i++)
        {
            hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
        }

        return ~hash ^ Constants.SXorMask;
    }
}