using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Youtube
    {
        private Dictionary<string, YoutubeChannel> youtubeChannel;
        private long[,] Inputs;
        private long[] Outputs;
        private const double Threshold = 0.05;
        private const double Precision = 0.00;
        private double[] Weights;
        private int Epoch = 0;

        public Youtube(long[,] Inputs, long[] Outputs)
        {
            youtubeChannel = new Dictionary<string, YoutubeChannel>();
            this.Inputs = Inputs;
            this.Outputs = Outputs;
        }
        public void AddChannel(YoutubeChannel NewYoutuber)
        {
            youtubeChannel[NewYoutuber.channelName] = NewYoutuber;
        }
        public int Predict(/*long[] Inputs*/long[,] Inputs)
        {
            for (int row = 0; row < Inputs.GetLength(0); row++)
            {
                double U = 0;

                for (int col = 0; col < Inputs.GetLength(1) - 1; col++)
                {
                    U += Inputs[row, col] * Weights[col];
                }
                Console.WriteLine("Inputs {0} {1}", Inputs[row, 0], Inputs[row, 1]);
                Console.WriteLine("Prediction {0} Should be {1}", U, Inputs[row, 2]);
            }
            //double U = 0;

            //for (int i = 0; i < Inputs.Length; i++)
            //{
            //    U += Inputs[i] * Weights[i];
            //}
            //Console.WriteLine("U {0}", U);
            //return StepFunction(U);
            return 0;
        }
        public int Train()
        {
            InitialiseRandomWeights(Inputs);

            double EQMAnterior, EQMAtual;

            do
            {
                EQMAnterior = LeastMeanSquare();

                for (int k = 0; k < Inputs.GetLength(0); k++)
                {
                    double U = 0;

                    for (int i = 0; i < Weights.Length; i++)
                    {
                        U +=  Inputs[i,k] * Weights[i];
                    }

                    for (int i = 0; i < Weights.Length; i++)
                    {
                        Weights[i] = Weights[i] + Threshold * (Outputs[i] - U) * Inputs[i, k];
                    }
                }
                Epoch++;

                EQMAtual = LeastMeanSquare();
            } while (Math.Abs(EQMAtual - EQMAnterior) > Precision);

            return Epoch;
        }
        private double WeightSun(int I)
        {
            double U = 0;

            for (int i = 0; i < Inputs.GetLength(0); i++)
            {
                U += 0 * Weights[i];
            }
            return U;
        }
        private void InitialiseRandomWeights(long[,] Inputs)
        {
            Random rnd = new Random();

            int i = Inputs.GetLength(0);//kiek eiluciu. musu aveju 2

            Weights = new double[i];

            for (int j = 0; j < i; j++)
            {
                Weights[j] = rnd.NextDouble();
            }
        }
        private int StepFunction(double U)
        {
            return (U >= 0.5) ? 1 : -1;
        }
        private double LeastMeanSquare()
        {
            int i = Outputs.Length;//tike outupu kiek yra inputu deriniu arba stulpeliu?

            double lms = 0d;
            //for (int x = 0; x < Inputs.GetLength(0); x++)//eil
            //{
            //    double U = 0;
            //    for (int y = 0; y < Inputs.GetLength(1); y++)//stulp
            //    {
            //        U += Weights[y] * Inputs[y, x];
            //    }

            //    lms += Math.Pow(Outputs[x] - U, 2);
            //}
            for (int x = 0; x < Inputs.GetLength(1); x++)//stulpeliai
            {
                double U = 0;
                for (int y = 0; y < Inputs.GetLength(0); y++)//eilutes
                {
                    U += Weights[y] * Inputs[y, x];
                }

                lms += Math.Pow(Outputs[x] - U, 2);
            }

            return lms/i;
        }
    }
}
