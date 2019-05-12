using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {
        private const string filePath = @"../../Files/data.csv";

        static void Main(string[] args)
        {
            //0 diemnsija yra kiek eilučių, 1 dimensija kiek stulpelių.
            long[,] inputs = {
                { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 },//watch time
                { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 }//uploads
            };

            long[] outputs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };//subs
            Youtube youtube = new Youtube(inputs, outputs);
            //List<YoutubeChannel> list = new List<YoutubeChannel>();
            //ReadCsvFile(youtube);
            Console.WriteLine(youtube.Train());
            long[,] data = {
                { 2, 2, 2 },//watch time , uploads, subs
                { 3, 3, 3 },//watch time , uploads, subs
                { 4, 4, 4 },//watch time , uploads, subs
                { 5, 5, 5 }//watch time , uploads, subs
            };
            //long[] data = { 5, 5 };
            int r = youtube.Predict(data);
            Console.WriteLine(r);
        }

        private static void ReadCsvFile(Youtube youtube)
        {
            int i = 0;
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = cleanData(reader.ReadLine());
                    var data = line.Split(',');
                    if (i != 0)
                    {
                        try
                        {
                            YoutubeChannel channel = new YoutubeChannel(
                            data[2],
                            Convert.ToInt64(data[3]),
                            Convert.ToInt64(data[5]),
                            Convert.ToInt64(data[4])
                            );
                            youtube.AddChannel(channel);
                        } catch
                        {
                            //data exception
                        }
                        
                    }
                    i++;
                }
            }
        }

        private static string cleanData(string data)
        {
            string oldline = data;
            data = data.Replace("\"", "");
            data = data.Replace("--", "0");
            if (!data.Equals(oldline))
            {
                data = data.Remove(data.IndexOf(','), 1);
            }
            return data;
        }
    }
}
