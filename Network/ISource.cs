using System.Collections.Generic;

namespace Network
{
    public interface ISource
    {
        IReadOnlyCollection<TrainTuple> ReadAll();
        TrainTuple ReadBy(int number);
    }
}