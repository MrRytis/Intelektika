using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Src
{
    class Bayes
    {
        private List<Dictionary<string, YoutubeChannel>> FullData;
        private List<Dictionary<string, YoutubeChannel>> TrainigData;
        private Dictionary<string, YoutubeChannel> TestData;
        public List<int> IntervalChanelsCount { get; set; }
        public int ChanelsCount { get; set; }
        public int IntervalCount { get; set; }

        public Bayes()
        {
            TrainigData = new List<Dictionary<string, YoutubeChannel>>();
            TestData = new Dictionary<string, YoutubeChannel>();
            IntervalChanelsCount = new List<int>();
            ChanelsCount = 0;
            IntervalCount = 0;
        }
        /// <summary>
        /// Algorithms training
        /// </summary>
        /// <param name="Data">Data set for training</param>
        public void Train(List<Dictionary<string, YoutubeChannel>> Data)
        {
            TrainigData = Data;
            IntervalCount = TrainigData.Count;
            int count;
            ChanelsCount = 0;
            IntervalChanelsCount = new List<int>();

            for (int i = 0; i < IntervalCount; i++)
            {
                count = TrainigData[i].Count;
                IntervalChanelsCount.Add(count);
                ChanelsCount += count;
            }
        }
        /// <summary>
        /// Method used to check algorithm rightness.
        /// </summary>
        /// <param name="fullData">all scaned data to check rightness</param>
        /// <param name="testData">data wee use for testing</param>
        /// <returns></returns>
        public string Test(List<Dictionary<string, YoutubeChannel>> fullData, Dictionary<string, YoutubeChannel> testData)
        {
            FullData = fullData;
            TestData = testData;

            List<double> IntervalProbabilities = new List<double>();

            for (int i = 0; i < IntervalCount; i++)
            {
                IntervalProbabilities.Add(IsInThisInterval(i));
            }

            int prediction;
            int subInterval;
            int correct = 0;
            int incorrect = 0;

            foreach (KeyValuePair<string, YoutubeChannel> entry in testData)
            {
                prediction = FindHighestProbability(IntervalProbabilities);

                subInterval = FindSubInterval(entry.Value.channelName);

                if (prediction == subInterval)
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }
            double percentage = (correct * 100) / (correct + incorrect);
            return String.Format("Bayes Prediction Corrct {0} Incorrect {1} Percentage {2:f}", correct, incorrect, percentage);
        }
        /// <summary>
        /// Method used to find which interval is the most likely
        /// </summary>
        /// <param name="IntervalProbabilities">list of all interval probabilities</param>
        /// <returns></returns>
        private int FindHighestProbability(List<double> IntervalProbabilities)
        {
            double max = 0.0;
            int index = 0;
            for (int i = 0; i < IntervalProbabilities.Count; i++)
            {
                if (IntervalProbabilities[i] > max)
                {
                    max = IntervalProbabilities[i];
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// Method used to calcucalte interval probability
        /// </summary>
        /// <param name="index">interval index</param>
        /// <returns></returns>
        private double IntervalProbability(int index)
        {
            return Math.Round((double)IntervalChanelsCount[index] / (double)ChanelsCount, 4);
        }
        /// <summary>
        /// Method used to calculate probability of index interval is correct
        /// </summary>
        /// <param name="index">interval index</param>
        /// <returns></returns>
        private double IsInThisInterval(int index)
        {
            if (IntervalChanelsCount[index] == 0)
            {
                return 0.01;
            }
            if (IntervalChanelsCount[index] == ChanelsCount)
            {
                return 0.99;
            }
            double intervalProbability = IntervalProbability(index);
            double notInThisChanel = 0.0;
            for (int i = 0; i < IntervalCount; i++)
            {
                notInThisChanel += IntervalProbability(i);
            }
            return Math.Round((double)intervalProbability / (double)notInThisChanel, 4);
        }
        /// <summary>
        /// Method used to find in which interval chanel is
        /// </summary>
        /// <param name="name">chanel name</param>
        /// <returns></returns>
        private int FindSubInterval(string name)
        {
            for (int i = 0; i < FullData.Count; i++)
            {
                if (FullData[i].TryGetValue(name, value: out YoutubeChannel value))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
