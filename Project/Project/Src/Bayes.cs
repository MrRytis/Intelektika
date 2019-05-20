using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Src
{
    class Bayes
    {
        private List<Dictionary<string, YoutubeChannel>> TrainigData;
        private Dictionary<string, YoutubeChannel> TestData;
        public List<int> IntervalChanelsCount { get; set; }
        public int ChanelsCount { get; set; }
        public int IntervalCount { get; set; }

        public Bayes()
        {
            TrainigData = new List<Dictionary<string, YoutubeChannel>>();
            TestData = new Dictionary<string, YoutubeChannel>();
            IntervalChanelsCount = new List<int>(); ;
            ChanelsCount = 0;
            IntervalCount = 0;
        }
        public void Train(List<Dictionary<string, YoutubeChannel>> Data)
        {
            TrainigData = Data;
            IntervalCount = TrainigData.Count;
            int count;

            for (int i = 0; i < IntervalCount; i++)
            {
                count = TrainigData[i].Count;
                IntervalChanelsCount[i] = count;
                ChanelsCount += count;
            }
        }
        private double IntervalProbability(int index)
        {
            return IntervalChanelsCount[index] / ChanelsCount;
        }
        private double IsInThisInterval(int i)
        {
            if (IntervalChanelsCount[i] == 0)
            {
                return 0.01;
            }
            if (IntervalChanelsCount[i] == ChanelsCount)
            {
                return 0.99;
            }
            double notInThisChanel = 0.0;
            for (int i = 0; i < IntervalCount; i++)
            {
                if
            }
            return 0.0;
        }
    }
}
