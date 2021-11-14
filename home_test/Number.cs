using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    public static class Number
    {
        public static readonly Dictionary<string, double> LowNumbersDic = new Dictionary<string, double>()
        {
            {"zero", 0}, {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}, {"ten", 10},
            {"eleven", 11}, {"twelve", 12}, {"thirteen", 13}, {"fourteen", 14}, {"fifteen", 15}, {"sixteen", 16}, {"seventeen", 17}, {"eighteen", 18}, {"nineteen", 19},
            {"twenty", 20}, {"thirty", 30}, {"forty", 40}, {"fifty", 50}, {"sixty", 60}, {"seventy", 70}, {"eighty", 80}, {"ninety", 90},
        };

        public static readonly Dictionary<string, double> HighNumbersDic = new Dictionary<string, double>()
        {
            {"hundred", 100}, {"thousand", 1000}, {"million", Math.Pow(10, 6)}, {"billion", Math.Pow(10, 9)}, {"trillion", Math.Pow(10, 12)},
            {"quadrillion", Math.Pow(10, 15)}, {"quintillion", Math.Pow(10, 18)}, {"sextillion", Math.Pow(10, 21)}, {"septillion", Math.Pow(10, 24)},
            {"octillion", Math.Pow(10, 27)}, {"nonillion", Math.Pow(10, 30)}, {"decillion", Math.Pow(10, 33)}, {"undecillion", Math.Pow(10, 36)},
            {"duodecillion", Math.Pow(10, 39)}, {"tredecillion", Math.Pow(10, 42)}, {"quattuordecillion", Math.Pow(10, 45)}, {"quindecillion", Math.Pow(10, 48)},
            {"sexdecillion", Math.Pow(10, 51)}, {"septendecillion", Math.Pow(10, 54)}, {"octodecillion", Math.Pow(10, 57)}, {"novemdecillion", Math.Pow(10, 60)},
            {"vigintillion", Math.Pow(10, 63)}, {"unvigintillion", Math.Pow(10, 66)}, {"duovigintillion", Math.Pow(10, 69)}, {"trevigintillion", Math.Pow(10, 72)},
            {"quattuorvigintillion", Math.Pow(10, 75)}, {"quinvigintillion", Math.Pow(10, 78)}, {"sexvigintillion", Math.Pow(10, 81)}, {"septenvigintillion", Math.Pow(10, 84)},
            {"octovigintillion", Math.Pow(10, 87)}, {"novemvigintillion", Math.Pow(10, 90)}, {"trigintillion", Math.Pow(10, 93)}, {"untrigintillion", Math.Pow(10, 96)},
            {"duotrigintillion", Math.Pow(10, 99)}, {"googol", Math.Pow(10, 100)}, {"tretrigintillion", Math.Pow(10, 102)}, {"quattuortrigintillion", Math.Pow(10, 105)},
            {"quintrigintillion", Math.Pow(10, 108)}, {"sextrigintillion", Math.Pow(10, 111)}, {"septentrigintillion", Math.Pow(10, 114)},
            {"octotrigintillion", Math.Pow(10, 117)}, {"novemtrigintillion", Math.Pow(10, 120)}, {"centillion", Math.Pow(10, 303)},
        };

        private static bool _isFoundNumber = false;

        /// <summary>
        /// Returns the number as a double type, and fill in the values ​​regarding the type of number.
        /// If the string is not a number - returns default values - 0
        /// </summary>
        /// <param name="word"></param>
        /// <param name="isNum"></param>
        /// <param name="isLowNumber"></param>
        /// <returns></returns>
        public static double StringToNumber(string word, out bool isNum, out bool isLowNumber)
        {
            double num = 0;
            string numInString = null;

            word = word.Replace(",", ""); //Change 3,000 to 3000
            isNum = double.TryParse(word, out num);
            isLowNumber = isNum;
            if (isNum)
                numInString = num.ToString();
            KeyValuePair<string, double> pair = new KeyValuePair<string, double>(numInString, num);


            if (!isNum)
            {
                pair = LowNumbersDic.FirstOrDefault(item => word.ToLower().Equals(item.Key));
                if (pair.Key == null)
                    pair = LowNumbersDic.FirstOrDefault(item => word.ToLower().StartsWith(item.Key));//for example: My age is ten. (with . at the end).
                if (pair.Key != null)
                {
                    isNum = true;
                    num = pair.Value;
                    isLowNumber = true;
                }
                else
                {
                    pair = HighNumbersDic.FirstOrDefault(item => word.ToLower().StartsWith(item.Key));
                    if (pair.Key != null)
                    {
                        isNum = true;
                        num = pair.Value;
                        isLowNumber = false;
                    }
                }

                if (pair.Key == null)//Covers the format: (345).
                {
                    int i = 0;

                    while (i < word.Length)
                    {
                        if (char.IsDigit(word[i]))
                        {
                            isNum = true;
                            num = num * 10 + ((double)word[i] - 48);
                        }
                        else if (num != 0)
                        {
                            break;
                        }

                        i++;
                    }
                }
            }

            return num;
        }

        /// <summary>
        /// Summarize all the numbers in the list and compare with the second parameter.
        /// Returns the larger number.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static double SumNumbersAndCompare(List<double> numbers, double maxNum)
        {
            if (numbers.Count > 0)
            {
                double num = numbers.Sum();
                if (!_isFoundNumber)
                {
                    maxNum = num;
                    _isFoundNumber = true;
                }
                else
                {
                    maxNum = maxNum > num ? maxNum : num;
                }
            }

            return maxNum;
        }

        public static bool IsFoundNumber()
        {
            return _isFoundNumber;
        }
    }
}
