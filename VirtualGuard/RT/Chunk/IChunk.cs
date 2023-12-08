namespace VirtualGuard.RT.Chunk;

public interface IChunk
{
    public int Length { get; }
    public void OnOffsetComputed(int offset);
    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt);
}