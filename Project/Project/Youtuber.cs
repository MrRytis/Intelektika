using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Youtuber
    {
        public string ChannelName { get; set; }
        public int VideoUploads { get; set; }
        public int VideoViews { get; set; }
        public int Subscribers { get; set; }

        public Youtuber(string ChannelName, int VideoUploads, int VideoViews, int Subscribers)
        {
            this.ChannelName = ChannelName;
            this.VideoUploads = VideoUploads;
            this.VideoViews = VideoViews;
            this.Subscribers = Subscribers;
        }
    }
}
