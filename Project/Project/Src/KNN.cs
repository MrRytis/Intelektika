using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Src
{
    class KNN
    {
        private List<Dictionary<string, YoutubeChannel>> FullDataList;
        private List<Dictionary<string, YoutubeChannel>> TainingDataList;
        private Dictionary<string, YoutubeChannel> TestingDataList;
        private int classesCount;

        public KNN(List<Dictionary<string, YoutubeChannel>> fullDataList,  List<Dictionary<string, YoutubeChannel>> dataList, int classesCount)
        {
            FullDataList = fullDataList;
            TainingDataList = dataList;
            this.classesCount = classesCount;
        }
        public void Train()
        {
            //?
        }
        public void Test(Dictionary<string, YoutubeChannel> testingDataList)
        {
            TestingDataList = testingDataList;
            long number;
            List<long> Closest;
            List<int> Intervals;
            int viewsInterval;
            int uploadsInterval;
            int subInterval;
            bool prediction;
            int correct = 0;
            int incorrect = 0;
            foreach (KeyValuePair<string, YoutubeChannel> entry in TestingDataList)
            {
                number = entry.Value.videoViews;
                Closest = FindClosest(number, true, 5);
                Intervals = FindInterval(Closest, true);
                viewsInterval = PredictInterval(Intervals);
                //Console.WriteLine("Prediction {0}", viewsInterval);
                number = entry.Value.videoUploads;
                Closest = FindClosest(number, false, 5);
                Intervals = FindInterval(Closest, false);
                uploadsInterval = PredictInterval(Intervals);
                subInterval = FindSubInterval(entry.Value.channelName);
                //Console.WriteLine("Prediction {0}", uploadsInterval);
                //Console.WriteLine("Subs {0}", entry.Value.subscribers);
                prediction = isPredictionCorrect(subInterval, viewsInterval, uploadsInterval);
                if (prediction)
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }
            double percentage = (correct * 100) / (correct + incorrect);
            //Console.WriteLine("{0:f4}",correct / (correct + incorrect));
            Console.WriteLine("Corrct {0} Incorrect {1} Percentage {2:f}", correct, incorrect, percentage);
        }
        private bool isPredictionCorrect(int trueValue, int prediction1, int prediction2)
        {
            return trueValue == prediction1 || trueValue == prediction2;
        }
        private int FindSubInterval(string name)
        {
            for (int i = 0; i < FullDataList.Count; i++)
            {
                if (FullDataList[i].TryGetValue(name, value: out YoutubeChannel value))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Binary search rasti artimiausiems skaičiams pagal duotajį
        /// </summary>
        /// <param name="number">Duotasis skaičius</param>
        /// <param name="isViews">Ar views?</param>
        /// <param name="howMany">Kiek kaimynųS</param>
        /// <returns></returns>
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
                        if (mid < howMany && mid + howMany < List.Count)
                        {
                            for (int index = mid; index < mid + howMany; index++)
                            {
                                Closest.Add(List[index]);
                            }
                        }
                        else
                        {
                            for (int index = mid - howMany; index < mid; index++)
                            {
                                Closest.Add(List[index]);
                            }
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
                        if (mid < howMany && mid + howMany < List.Count)
                        {
                            for (int index = mid; index < mid + howMany; index++)
                            {
                                Closest.Add(List[index]);
                            }
                        }
                        else
                        {
                            for (int index = mid - howMany; index < mid; index++)
                            {
                                Closest.Add(List[index]);
                            }
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
        /// <summary>
        /// Randame į kokius intervalus papuoda duotieji skaičiai
        /// </summary>
        /// <param name="Numbers">Doutieji skaičiai</param>
        /// <param name="isViews">Ar views</param>
        /// <returns></returns>
        private List<int> FindInterval(List<long> Numbers, bool isViews)
        {
            List<int> Result = new List<int>();
            long[,] Intervals = FindIntervals(FormListOfArrays(isViews));
            foreach (var number in Numbers)
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
        /// <summary>
        /// Susirandam tarp kokių intarvalų pasiskirste skaičiai masyvuose
        /// </summary>
        /// <param name="Arr">Matrica kurioje turim views arba vid uploads</param>
        /// <returns></returns>
        private long[,] FindIntervals(List<long[]> Arr)
        {
            long[,] Intervals = new long[classesCount, 2];
            for (int i = 0; i < Arr.Count; i++)
            {
                long min = Arr[i].Min();
                long max = Arr[i].Max();

                Intervals[i, 0] = min;
                Intervals[i, 1] = max;
            }
            return Intervals;
        }
        /// <summary>
        /// Sudarom sąrašą visų views arba vid uploads
        /// </summary>
        /// <param name="isViews">Ar views?</param>
        /// <returns></returns>
        private List<long> FormList(bool isViews)
        {
            List<long> Result = new List<long>();

            if (isViews)
            {
                for (int i = 0; i < TainingDataList.Count; i++)
                {
                    foreach (KeyValuePair<string, YoutubeChannel> entry in TainingDataList[i])
                    {
                        Result.Add(entry.Value.videoViews);
                    }
                }
            }
            else
            {
                for (int i = 0; i < TainingDataList.Count; i++)
                {
                    foreach (KeyValuePair<string, YoutubeChannel> entry in TainingDataList[i])
                    {
                        Result.Add(entry.Value.videoUploads);
                    }
                }
            }
            Result.Sort();
            return Result;
        }
        /// <summary>
        /// Susidarom sąrašus views arba vid uploads pagal tai kaip suskirstyta training data į intervalus
        /// </summary>
        /// <param name="isViews">Ar atlikti veiksmus views ar vid uploads</param>
        /// <returns></returns>
        private List<long[]> FormListOfArrays(bool isViews)
        {
            List<long[]> Result = new List<long[]>();

            if (isViews)
            {
                for (int i = 0; i < TainingDataList.Count; i++)
                {
                    Result.Add(new long[TainingDataList[i].Count]);
                    int j = 0;
                    foreach (KeyValuePair<string, YoutubeChannel> entry in TainingDataList[i])
                    {
                        Result[i][j++] = entry.Value.videoViews;
                    }
                }
            }
            else
            {
                for (int i = 0; i < TainingDataList.Count; i++)
                {
                    Result.Add(new long[TainingDataList[i].Count]);
                    int j = 0;
                    foreach (KeyValuePair<string, YoutubeChannel> entry in TainingDataList[i])
                    {
                        Result[i][j++] = entry.Value.videoUploads;
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// Surandam kuris intervlaas dažniausiai kartojasi toks ir bus atsakymas
        /// </summary>
        /// <param name="Intervals">Gauti intervalai</param>
        /// <returns></returns>
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
