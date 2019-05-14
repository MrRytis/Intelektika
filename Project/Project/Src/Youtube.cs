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

        public void CleanAnomolies()
        {
            int k = 2;
            double[] avg = GetAverages();
            double[] sd = GetStandartDeviation();

            Dictionary<string, string> keyForRemoval = new Dictionary<string, string>();

            double tukeySubsPlus = avg[0] + (k * sd[0]);
            double tukeySubsMinus = avg[0] - (k * sd[0]);
            double tukeyUploadsPlus = avg[1] + (k * sd[1]);
            double tukeyUploadsMinus = avg[1] - (k * sd[1]);
            double tukeyViewsPlus = avg[2] + (k * sd[2]);
            double tukeyViewsMinus = avg[2] - (k * sd[2]);

            foreach (KeyValuePair<string, YoutubeChannel> entry in youtubeChannel)
            {
                if (entry.Value.subscribers > tukeySubsPlus || entry.Value.subscribers < tukeySubsMinus)
                {
                    if(!keyForRemoval.ContainsKey(entry.Key))
                        keyForRemoval.Add(entry.Key, entry.Key);
                }
                if (entry.Value.videoUploads > tukeyUploadsPlus || entry.Value.videoUploads < tukeyUploadsMinus)
                {
                    if (!keyForRemoval.ContainsKey(entry.Key))
                        keyForRemoval.Add(entry.Key, entry.Key);
                }
                if (entry.Value.videoViews > tukeyViewsPlus || entry.Value.videoViews < tukeyViewsMinus)
                {
                    if (!keyForRemoval.ContainsKey(entry.Key))
                        keyForRemoval.Add(entry.Key, entry.Key);
                }
            }

            foreach(KeyValuePair<string, string> removal in keyForRemoval)
            {
                youtubeChannel.Remove(removal.Value);
            }
        }

        private double[] GetAverages()
        {
            double[] avg = new double[3] { 0, 0, 0};
            foreach (KeyValuePair<string, YoutubeChannel> entry in youtubeChannel)
            {
                avg[0] = avg[0] + entry.Value.subscribers;
                avg[1] = avg[1] + entry.Value.videoUploads;
                avg[2] = avg[2] + entry.Value.videoViews;

            }

            for (int i = 0; i < avg.Length; i++)
            {
                avg[i] = avg[i] / youtubeChannel.Count;
            }

            return avg;
        }

        private double[] GetStandartDeviation()
        {
            double[] SD = new double[3] { 0, 0, 0 };
            double[] avg = GetAverages();
            foreach (KeyValuePair<string, YoutubeChannel> entry in youtubeChannel)
            {
                SD[0] = SD[0] + Math.Pow((entry.Value.subscribers - avg[0]),2);
                SD[1] = SD[1] + Math.Pow((entry.Value.videoUploads - avg[1]), 2);
                SD[2] = SD[2] + Math.Pow((entry.Value.videoViews - avg[2]), 2);

            }

            for (int i = 0; i < SD.Length; i++)
            {
                SD[i] = Math.Sqrt((1f / youtubeChannel.Count) * SD[i]);
            }

            return SD;
        }
    }
}
