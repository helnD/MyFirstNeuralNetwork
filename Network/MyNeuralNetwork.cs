using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Network
{
    public class MyNeuralNetwork
    {
        private ISource _source;
        
        public List<int> Sizes { get; set; }
        public int NumLayers { get; set; }
        public List<Vector<double>> Biases { get; set; } = new List<Vector<double>>();
        public List<Matrix<double>> Weights { get; set; } = new List<Matrix<double>>();
        
        public MyNeuralNetwork(ISource source, params int[] sizes)
        {
            _source = source;
            Sizes = sizes.ToList();
            NumLayers = sizes.Length;
            
            for (var y = 1; y < sizes.Length; y++)
            {
                var newBiases = Vector<double>.Build.Random(sizes[y]);
                Biases.Add(newBiases);
            }

            for (int x = 0, y = 1; y < sizes.Length; y++, x++)
            {
                var newWeights = Matrix<double>.Build.Random(sizes[y], sizes[x]);
                Weights.Add(newWeights);
            }
        }
        
        public Vector<double> Feedforward(IEnumerable<byte> input)
        {
            var output = Vector<double>.Build.DenseOfEnumerable(input.Select(it => (double) it));
            var biasesWithWeights = Biases.Zip(Weights, (vector, matrix) => (vector, matrix));

            foreach (var (bias, weights) in biasesWithWeights)
            {
                output = Sigmoid(weights.Multiply(output) + bias);
            }

            return output;
        }

        public Vector<double> BackPropagation(TrainTuple train, double speed)
        {
            var activations = new List<Vector<double>>
            {
                Vector<double>.Build.DenseOfEnumerable(train.Input.Select(it => (double) it))
            };
                
            var weightedSums = new List<Vector<double>>();
            
            var biasesWithWeights = Biases.Zip(Weights, (vector, matrix) => (vector, matrix));
            foreach (var (biases, weights) in biasesWithWeights)
            {
                var weightedSum = weights * activations[^0] + biases;
                weightedSums.Add(weightedSum);

                var newActivation = Sigmoid(weightedSum);
                activations.Add(newActivation);
            }
            
            var actual = Vector<double>.Build.Dens
            
            var delta = Cost(activations.Last(), tra)
        }

        private Vector<double> Sigmoid(Vector<double> vector) =>
            vector.Map(it => 1.0 / (1.0 + Math.Exp(it)));

        private Vector<double> Cost(Vector<double> actual, Vector<double> expected) =>
            actual - expected;


    }
}