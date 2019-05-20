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

        public Bayes()
        {
            TrainigData = new List<Dictionary<string, YoutubeChannel>>();
            TestData = new Dictionary<string, YoutubeChannel>();
            IntervalChanelsCount = new List<int>(); ;
            ChanelsCount = 0;
        }
    }
}
