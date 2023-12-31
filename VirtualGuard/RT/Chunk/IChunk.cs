namespace VirtualGuard.RT.Chunk;

internal interface IChunk
{
    public int Length { get; }
    public void OnOffsetComputed(int offset);
    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt);
}