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

        public Youtube()
        {
            youtubeChannel = new Dictionary<string, YoutubeChannel>();
        }
        public void AddChannel(YoutubeChannel NewYoutuber)
        {
            youtubeChannel[NewYoutuber.channelName] = NewYoutuber;
        }
    }
}
