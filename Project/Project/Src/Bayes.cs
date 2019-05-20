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
    }
}
