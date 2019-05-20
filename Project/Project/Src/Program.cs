using System;
using System.IO;

namespace Project
{
    class Program
    {
        private const string filePath = @"../../Files/data.csv";

        static void Main(string[] args)
        {
            Youtube youtube = new Youtube();
            ReadCsvFile(youtube);
            youtube.CleanAnomolies();
            youtube.CrossValidation(5);

        //read data in console
        Inputs:
            Console.WriteLine("Write views count");
            long viewCount = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Write video upload count");
            long uploadCount = Convert.ToInt64(Console.ReadLine());
            youtube.CalculetaAnsBasedOnData(viewCount, uploadCount, 5);
        goto Inputs;

        }

        private static void ReadCsvFile(Youtube youtube)
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = CleanFileData(reader.ReadLine());
                    var data = line.Split(',');
                    try
                    {
                        YoutubeChannel channel = new YoutubeChannel(
                        data[2],
                        Convert.ToInt64(data[3]),
                        Convert.ToInt64(data[5]),
                        Convert.ToInt64(data[4])
                        );
                        youtube.AddChannel(channel);
                    }
                    catch
                    {
                        //data exception
                    }
                }
            }
        }

        private static string CleanFileData(string data)
        {
            string oldline = data;
            data = data.Replace("\"", "");
            if (!data.Equals(oldline))
            {
                data = data.Remove(data.IndexOf(','), 1);
            }
            return data;
        }
    }
}
