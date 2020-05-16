using System.Collections.Generic;
using System.IO;

namespace Network.Source
{
    public class FileSource : ISource
    {
        private FileStream _imageReader = new FileStream(
            @"C:\Users\1\RiderProjects\MyFirstNeuralNetwork\Network\Source\train-images.idx3-ubyte", FileMode.Open);

        private FileStream _labelReader = new FileStream(
            @"C:\Users\1\RiderProjects\MyFirstNeuralNetwork\Network\Source\train-labels.idx1-ubyte", FileMode.Open);
        
        public IReadOnlyCollection<TrainTuple> ReadAll()
        {
            var result = new List<TrainTuple>();
            
            _imageReader.Seek(GetPixelPosition(0, 0, 0), SeekOrigin.Begin);
            _labelReader.Seek(GetLabelPosition(0), SeekOrigin.Begin);

            for (var number = 0; number < 60000; number++)
            {
                var pixels = new List<byte>();

                for (var x = 0; x < 28; x++)
                {
                    for (var y = 0; y < 28; y++)
                    {
                        pixels.Add((byte)_imageReader.ReadByte());
                    }
                }

                var label = _labelReader.ReadByte();
                var output = new List<byte>();

                for (byte index = 1; index <= 9; index++)
                {
                    output.Add(index == label ? index : (byte)0);
                }
                
                result.Add(new TrainTuple  { Input = pixels, Output = output });
            }

            return result;
        }

        public TrainTuple ReadBy(int number)
        {
            var pixels = new List<byte>();

            _imageReader.Seek(GetPixelPosition(number, 0, 0), SeekOrigin.Begin);
            _labelReader.Seek(GetLabelPosition(number), SeekOrigin.Begin);
            
            for (var x = 0; x < 28; x++)
            {
                for (var y = 0; y < 28; y++)
                {
                    pixels.Add((byte)_imageReader.ReadByte());
                }
            }
            
            var label = _labelReader.ReadByte();
            var output = new List<byte>();

            for (byte index = 1; index <= 9; index++)
            {
                output.Add(index == label ? (byte)1 : (byte)0);
            }

            return new TrainTuple
            {
                Input = pixels, Output = output
            };
        }

        private int GetPixelPosition(int imageNumber, int x, int y) =>
            (imageNumber * 28 * 28) + (x + (y * 28)) + 15;

        private int GetLabelPosition(int labelNumber) =>
            labelNumber + 8;
    }
}