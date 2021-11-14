using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextAnalysis;

namespace textAnalysis
{
    class TextAnalysis
    {
        private static int linesCounter = 0, sentencesCounter = 0, sentencesAllLen = 0, sentencesMaxLen = 0, sentenceCurrentLen = 0, wordSequenceWithoutKCurrentLen = 0, wordSequenceWithoutKMaxLen = 0;
        private static Dictionary<string, int> wordsDic = new Dictionary<string, int>();

        private static bool isPrevWordNum = false;
        private static List<double> numbers = new List<double>();
        private static double maxNum = 0.0;

        static void ParseWordAndCheckTheMaxNum(string word)
        {
            //Cover the format: twenty-nine
            var listWords = word.Split('-');
            foreach (var w in listWords)
            {
                CheckTheMaxNum(w);
            }
        }
        static void CheckTheMaxNum(string word)
        {
            bool isNum = false, isLowNumber = false;
            double num = Number.StringToNumber(word, out isNum, out isLowNumber);

            if (isNum)
            {
                if (numbers.Count == 0)
                {
                    numbers.Add(num);
                    isPrevWordNum = isLowNumber;
                    return;
                }
                if (isLowNumber)//Number between 0 - 99
                {
                    if (isPrevWordNum)
                        numbers[^1] = numbers[^1] + num;
                    else
                        numbers.Add(num);
                    isPrevWordNum = true;
                }
                else//A number starting from 100 and up
                { 
                    numbers[^1] = numbers[^1] * num;
                    isPrevWordNum = false;
                }
            }

            if(word.Equals("and"))
                return;
            string key = isNum ? num.ToString() : "";
            if (!isNum || !(key.Equals(word) || (key+"s").Equals(word))) //If the end of the number - Sum the list of numbers and compare with the max
            {
                maxNum = Number.SumNumbersAndCompare(numbers, maxNum);
                isPrevWordNum = false;
                numbers.Clear();
            }
        }

        private static void PrintOutput()
        {
            int averageOfSentencesLen = sentencesCounter == 0 ? 0 : sentencesAllLen / sentencesCounter;
            sentencesMaxLen = sentencesMaxLen == 0 ? sentenceCurrentLen : sentencesMaxLen;
            wordSequenceWithoutKMaxLen = wordSequenceWithoutKMaxLen == 0
                ? wordSequenceWithoutKCurrentLen
                : wordSequenceWithoutKMaxLen;

            Console.WriteLine("The output:");
            Console.WriteLine("The number of lines:                 " + linesCounter);
            Console.WriteLine("The number of words:                 " + wordsDic.Sum(x => x.Value));
            Console.WriteLine("The number of unique words:          " + wordsDic.Count(x => x.Value == 1));
            Console.WriteLine("The average length of the sentences: " + averageOfSentencesLen);
            Console.WriteLine("The longest sentence length:         " + sentencesMaxLen);
            Console.WriteLine("The longest word sequence without k: " + wordSequenceWithoutKMaxLen);
            if (Number.IsFoundNumber())
                Console.WriteLine("The max number in the text:          " + maxNum);
            else
            {
                if (numbers.Count == 0)
                    Console.WriteLine("No number found within the text.");
                else
                    Console.WriteLine("The max number in the text:          " + numbers.Sum());
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the text file path:");
            string filePath = Path.Combine(Console.ReadLine() ?? string.Empty);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ERROR! Incorrect path, please use this format for path: C:\\Temp\\text1.txt.\nEnter any key to end the program.");
                Console.ReadKey();
                return;
            }
            if (Path.GetExtension(filePath) != ".txt")
            {
                Console.WriteLine("ERROR! The program is for text files only.\nEnter any key to end the program.");
                Console.ReadKey();
                return;
            }

            List<string> words = new List<string>();
            foreach (string line in File.ReadLines(filePath))
            {
                linesCounter += 1;
                words = new List<string>(line.Split(" "));

                for (int i = 0; i < words.Count; i++)
                {
                    sentenceCurrentLen += words[i].Length + 1;

                    if(!wordsDic.ContainsKey(words[i]))
                        wordsDic.Add(words[i], 0);
                    wordsDic[words[i]] += 1;

                    if (!words[i].ToLower().Contains("k"))
                        wordSequenceWithoutKCurrentLen += 1;
                    else
                    {
                        if (wordSequenceWithoutKCurrentLen > wordSequenceWithoutKMaxLen)
                            wordSequenceWithoutKMaxLen = wordSequenceWithoutKCurrentLen;
                        wordSequenceWithoutKCurrentLen = 0;
                    }

                    ParseWordAndCheckTheMaxNum(words[i]);

                    if (Word.IsEndOfSentence(words[i], i))
                    {
                        sentencesCounter += 1;
                        sentenceCurrentLen -= 1;
                        sentencesAllLen += sentenceCurrentLen;
                        if(sentenceCurrentLen>sentencesMaxLen)
                            sentencesMaxLen = sentenceCurrentLen;
                        sentenceCurrentLen = 0;
                    }
                }
            }

            PrintOutput();

            Console.ReadKey();
        }
    }
}
