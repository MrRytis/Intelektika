using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Youtube
    {
        private Dictionary<string, Youtuber> Youtubers;

        public Youtube()
        {
            Youtubers = new Dictionary<string, Youtuber>();
        }
        public void AddYoutuber(Youtuber NewYoutuber)
        {
            Youtubers[NewYoutuber.ChannelName] = NewYoutuber;
        }
    }
}
