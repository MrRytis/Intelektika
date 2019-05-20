using Project.Src;
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

        public void CalculetaAnsBasedOnData(long viewCount, long uploadCount, int dataInterval)
        {
            var fullData = DivideData(youtubeChannel, dataInterval);

            KNN knn = new KNN(fullData, fullData, dataInterval);
            knn.GetResultBasedOnData(viewCount, uploadCount);

        }

        public void CrossValidation(int howManyValidationFolds)
        {
            var dataCount = youtubeChannel.Count;
            var range = dataCount / howManyValidationFolds;
            var values = youtubeChannel.Values.ToList();
            Console.WriteLine("Viso duomenų: " + dataCount + " 1/" + howManyValidationFolds + " duomenų: " + range + "\n");
            int start = 0;
            for (int i = 0; i < howManyValidationFolds; i++)
            {

                var testData = values.GetRange(start, range).ToDictionary(x => x.channelName); //paimamas range kiekis duomeų -  testavimui
                var trainData = values.GetRange(0, start).Concat(values.GetRange(start + range, dataCount - start - range)).ToDictionary(x => x.channelName); //paimami likusieji duomenys mokymuisi
                start += range;

                var fullData = DivideData(youtubeChannel, howManyValidationFolds);
                var dividedData = DivideData(trainData, howManyValidationFolds);
                //čia kviečiam algoritmo magijas
                KNN knn = new KNN(fullData, dividedData, howManyValidationFolds);
                knn.Test(testData);               
            }
        }
        private List<Dictionary<string, YoutubeChannel>> DivideData(Dictionary<string, YoutubeChannel> trainData, int howManyIntervals)
        {
            var Chanels = trainData.Values.ToList();
            long subMin = Chanels.Min(x => x.subscribers);
            long subMax = Chanels.Max(x => x.subscribers);

            long interval = (subMax - subMin) / howManyIntervals;
            var DataList = PrepareSplitedList(howManyIntervals);

            foreach (KeyValuePair<string, YoutubeChannel> entry in trainData)
            {
                int i = 0;
                long subs = entry.Value.subscribers;
                while (true)
                {
                    long min = subMin + interval * i;
                    long max = subMin + interval * (i + 1);
                    if (subs == subMax)
                    {
                        DataList[howManyIntervals - 1].Add(entry.Key, entry.Value);
                        break;
                    }
                    if (subs >= min && subs < max)
                    {
                        DataList[i].Add(entry.Key, entry.Value);
                        break;
                    }
                    i++;
                }
            }
            return DataList;
        }
        private List<Dictionary<string, YoutubeChannel>> PrepareSplitedList(int howManyIntervals)
        {
            var List = new List<Dictionary<string, YoutubeChannel>>();
            for (int i = 0; i < howManyIntervals; i++)
            {
                List.Add(new Dictionary<string, YoutubeChannel>());
            }
            return List;
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
                    if (!keyForRemoval.ContainsKey(entry.Key))
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

            foreach (KeyValuePair<string, string> removal in keyForRemoval)
            {
                youtubeChannel.Remove(removal.Value);
            }
        }

        private double[] GetAverages()
        {
            double[] avg = new double[3] { 0, 0, 0 };
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
                SD[0] = SD[0] + Math.Pow((entry.Value.subscribers - avg[0]), 2);
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
