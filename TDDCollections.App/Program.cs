using System;
using System.Collections.Generic;
using TddCollections.Lib;

namespace TDDCollections.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, string> dictionary = new MyDictionary<string, string>();
            
            string question;
            for(; ; )
            {
                Console.Write("Enter word =>");
                question = Console.ReadLine();
                string answer;
                if (question.ToLower().CompareTo("quit") == 0) break;

                if (dictionary.TryGetValue(question.ToLower(), out answer))
                {                    
                    Console.WriteLine($"Translate: {answer}");
                }
                else
                {
                    Console.Write($"Word {question} not found, enter answer =>");
                    answer = Console.ReadLine();
                    dictionary.Add(question, answer);
                }
            }
        }
    }
}
