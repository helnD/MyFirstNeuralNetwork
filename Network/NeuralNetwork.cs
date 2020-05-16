using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Network
{
    public class NeuralNetwork
    {
        private ISource _source;
        
        public List<int> Sizes { get; set; }
        public int NumLayers { get; set; }
        public List<Vector<double>> Biases { get; set; } = new List<Vector<double>>();
        public List<Matrix<double>> Weights { get; set; } = new List<Matrix<double>>();
        
        public NeuralNetwork(ISource source, params int[] sizes)
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
                var newWeights = Matrix<double>.Build.Random(sizes[x], sizes[y]);
                Weights.Add(newWeights);
            }
        }

        public Vector<double> Feedforward(IEnumerable<double> input)
        {
            var output = Vector<double>.Build.DenseOfEnumerable(input);
            var weightsWithBiases = Biases.Zip(Weights, (vector, matrix) => (vector, matrix));

            foreach (var (bias, weights) in weightsWithBiases)
            {
                output = (weights.TransposeThisAndMultiply(output) + bias).Map(Sigmoid);
            }

            return output;
        }

        public void BackPropagation(TrainTuple train)
        {
            var nablaB = Biases.Select(it => Vector<double>.Build.Dense(it.Count, 0)).ToList();
            var nablaW = Weights.Select(it => 
                Matrix<double>.Build.Dense(it.RowCount, it.ColumnCount, 0)).ToList();

            var activation = Vector<double>.Build.DenseOfEnumerable(train.Input.Select(it => (double)it));
            var activations = new List<Vector<double>> { activation };
            var zs = new List<Vector<double>>();
            
            var weightsWithBiases = Biases.Zip(Weights, (vector, matrix) => (vector, matrix));
            foreach (var (bias, weights) in weightsWithBiases)
            {
                var z = weights.TransposeThisAndMultiply(activation) + bias;
                zs.Add(z);
                activations.Add(z.Map(Sigmoid));
            }

            var expected = Vector<double>.Build.DenseOfEnumerable(train.Output.Select(it => (double) it));
            var delta = CostDerivative(activations.Last(), expected) *
                zs.Last().Map(SigmoidPrime);

            //nablaB[^0] = delta;
        }

        public void Train()
        {
            var all = _source.ReadAll();
            ;
        }

        private Vector<double> CostDerivative(Vector<double> output, Vector<double> actual) =>
            output - actual;

        private double Sigmoid(double weightedSum) =>
            1.0 / (1.0 + Math.Exp(-weightedSum));

        private double SigmoidPrime(double weightedSum) =>
            Sigmoid(weightedSum) * (1 - Sigmoid(weightedSum));
    }
}