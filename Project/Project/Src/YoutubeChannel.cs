using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class YoutubeChannel
    {
        public string channelName { get; set; }
        public long videoUploads { get; set; }
        public long videoViews { get; set; }
        public long subscribers { get; set; }

        public YoutubeChannel(string channelName, long videoUploads, long videoViews, long subscribers)
        {
            this.channelName = channelName;
            this.videoUploads = videoUploads;
            this.videoViews = videoViews;
            this.subscribers = subscribers;
        }
    }
}
