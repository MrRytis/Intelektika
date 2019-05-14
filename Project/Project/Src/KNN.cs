﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Src
{
    class KNN
    {
        private List<Dictionary<string, YoutubeChannel>> tainingDataList;
        private Dictionary<string, YoutubeChannel> testingDataList;
        private int classesCount;

        public KNN(List<Dictionary<string, YoutubeChannel>> dataList, int classesCount)
        {
            this.tainingDataList = dataList;
            this.classesCount = classesCount;
        }
        public void Train()
        {
            //?
        }
        public void Test(Dictionary<string, YoutubeChannel> testingDataList)
        {
            this.testingDataList = testingDataList;

            foreach (KeyValuePair<string, YoutubeChannel> entry in this.testingDataList)
            {
                long number = entry.Value.videoViews;
                List<long> Closest = FindClosest(number, true, 5);
                Closest.Add(3060768208);
                List<int> intervals = FindInterval(Closest, true);

                for (int i = 0; i < Closest.Count; i++)
                {
                    Console.WriteLine("{0} {1}",Closest[i],number);
                }
                foreach (var item in intervals)
                {
                    Console.WriteLine("{0} {1}", item, number);
                }
                var interval = PredictInterval(intervals);
                Console.WriteLine("Prediction {0}", interval);
            }
        }
        private List<long> FindClosest(long number, bool isViews, int howMany)//binary search geeksforgeeks
        {
            List<long> Closest = new List<long>();

            var List = FormList(isViews);

            int n = List.Count;
            // Corner cases 
            if (number <= List[0])
            {
                for (int index = 0; index < howMany; index++)
                {
                    Closest.Add(List[index]);
                }

                return Closest;
            }

            if (number >= List[n - 1])
            {
                for (int index = n; index < n - howMany; index--)
                {
                    Closest.Add(List[index]);
                }
                Closest.Sort();
                return Closest;
            }


            // Doing binary search  
            int i = 0, j = n, mid = 0;
            while (i < j)
            {
                mid = (i + j) / 2;

                if (List[mid] == number)
                {
                    for (int index = mid - howMany / 2; index < mid + howMany / 2 + 1; index++)
                    {
                        Closest.Add(List[index]);
                    }
                    return Closest;
                }


                /* If target is less  
                than array element, 
                then search in left */
                if (number < List[mid])
                {

                    // If target is greater  
                    // than previous to mid,  
                    // return closest of two 
                    if (mid > 0 && number > List[mid - 1])
                    {
                        for (int index = mid - howMany / 2; index < mid + howMany / 2 + 1; index++)
                        {
                            Closest.Add(List[index]);
                        }
                        return Closest;
                    }

                    /* Repeat for left half */
                    j = mid;
                }

                // If target is  
                // greater than mid 
                else
                {
                    if (mid < n - 1 && number < List[mid + 1])
                    {
                        for (int index = mid - howMany / 2; index < mid + howMany / 2 + 1; index++)
                        {
                            Closest.Add(List[index]);
                        }
                        return Closest;
                    }
                    i = mid + 1; // update i 
                }
            }

            // Only single element 
            // left after search 
            for (int index = mid - howMany / 2; index < mid + howMany / 2 + 1; index++)
            {
                Closest.Add(List[index]);
            }
            return Closest;
        }
        private List<int> FindInterval(List<long> numbers, bool isViews)
        {
            List<int> Result = new List<int>();
            long[,] Intervals = FindIntervals(FormListOfArrays(isViews));
            foreach (var number in numbers)
            {
                for (int i = 0; i < Intervals.GetLength(0); i++)
                {
                    if (number >= Intervals[i, 0] && number <= Intervals[i, 1])
                    {
                        Result.Add(i);
                    }
                }
            }
            return Result;
        }
        private long[,] FindIntervals(List<long[]> arr)
        {
            long[,] Intervals = new long[10, 2];
            for (int i = 0; i < arr.Count; i++)
            {
                long min = arr[i].Min();
                long max = arr[i].Max();
                Console.WriteLine("{0} {1}", min, max);

                Intervals[i, 0] = min;
                Intervals[i, 1] = max;
            }
            return Intervals;
        }
        private List<long> FormList(bool isViews)
        {
            List<long> result = new List<long>();

            if (isViews)
            {
                for (int i = 0; i < tainingDataList.Count; i++)
                {
                    foreach (KeyValuePair<string, YoutubeChannel> entry in tainingDataList[i])
                    {
                        result.Add(entry.Value.videoViews);
                    }
                }
            }
            else
            {
                for (int i = 0; i < tainingDataList.Count; i++)
                {
                    foreach (KeyValuePair<string, YoutubeChannel> entry in tainingDataList[i])
                    {
                        result.Add(entry.Value.videoUploads);
                    }
                }
            }
            result.Sort();
            return result;
        }
        private List<long[]> FormListOfArrays(bool isViews)
        {
            List<long[]> result = new List<long[]>();

            if (isViews)
            {
                for (int i = 0; i < tainingDataList.Count; i++)
                {
                    result.Add(new long[tainingDataList[i].Count]);
                    int j = 0;
                    foreach (KeyValuePair<string, YoutubeChannel> entry in tainingDataList[i])
                    {
                        result[i][j++] = entry.Value.videoViews;
                    }
                }
            }
            else
            {
                for (int i = 0; i < tainingDataList.Count; i++)
                {
                    result.Add(new long[tainingDataList[i].Count]);
                    int j = 0;
                    foreach (KeyValuePair<string, YoutubeChannel> entry in tainingDataList[i])
                    {
                        result[i][j++] = entry.Value.videoUploads;
                    }
                }
            }

            return result;
        }
        private int PredictInterval(List<int> Intervals)
        {
            int interval = 0;
            int oldCount = 0;

            for (int i = 0; i < classesCount; i++)
            {
                int newCount = 0;
                for (int j = 0; j < Intervals.Count; j++)
                {
                    if (i == Intervals[j])
                    {
                        newCount++;
                    }
                }
                if (newCount > oldCount)
                {
                    interval = i;
                    oldCount = newCount;
                }
            }

            return interval;
        }
    }
}