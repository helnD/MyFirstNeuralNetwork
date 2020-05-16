using System.Collections.Generic;

namespace Network
{
    public class TrainTuple
    {
        public IReadOnlyCollection<byte> Input { get; set; }
        public IReadOnlyCollection<byte> Output { get; set; }
    }
}