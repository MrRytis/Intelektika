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
            Youtube youtube = new Youtube();
            //List<YoutubeChannel> list = new List<YoutubeChannel>();
            ReadCsvFile(youtube);
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
