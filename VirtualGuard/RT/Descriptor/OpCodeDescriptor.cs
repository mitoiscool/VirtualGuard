using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor;

internal class OpCodeDescriptor {
    byte[] opCodeOrder = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();

    public OpCodeDescriptor(Random random) {
        random.Shuffle(opCodeOrder);
    }

    public byte this[VmCode opCode] => opCodeOrder[(int)opCode]; // get from random opcode order
}