using System.Collections;

namespace McProtoNet.Protocol340.Data.World.Chunk
{
    public class ChunkColumn : IEnumerable<Chunk>
    {

        private Chunk[] chunks;

        public ChunkColumn(Chunk[] chunks)
        {
            this.chunks = chunks;
        }
        public ChunkColumn(int size)
        {
            chunks = new Chunk[size];
        }

        public Chunk? this[int index]
        {
            get
            {
                if (index >= 0 && index < chunks.Length)
                    return chunks[index];
                return null;
            }
            set
            {
                chunks[index] = value;
            }
        }


        public IEnumerator<Chunk> GetEnumerator()
        {
            foreach (var item in chunks)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return chunks.GetEnumerator();
        }
    }
}
