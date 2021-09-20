using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculatorKata
{
    class Program
    {
        public static void AssertTrue(bool condition)
        {
            if (!condition)
                throw new Exception();
        }
        public static void AssertThrowException(Action action)
        {
            var thrown = false;
            try
            {
                action();
            }
            catch(Exception ex)
            {
                thrown = true;
            }
            if (!thrown)
            {
                throw new Exception();
            }
        }
        public static void AssertThrowExceptionWithMessage(Action action, string message)
        {
            
            var thrown = false;
            try
            {
                action();
            }
            catch(Exception ex)
            {
                if(ex.Message == message)
                    thrown = true;
            }
            if (!thrown)
            {
                throw new Exception();
            }
        }
        public static int Add(string input)
        {
            var delimiter = ",";
            var sum = 0;
            var numberStrings = new List<string>();
            var numbersText = string.Empty;

            if (input.StartsWith("//"))
            {
                var firstLine = input.Substring(2).TakeWhile(ch => ch != '\n');
                delimiter = string.Join("",firstLine.Where(ch => ch != '[' && ch != ']'));
                numbersText = input.Substring(2 + firstLine.Count() + 1);
            }
            else
            {
                numbersText = input;
            }

            var delimiters = new string[] { delimiter, "\n" };
            numberStrings = numbersText.Split(delimiters, StringSplitOptions.None).ToList();
            var numbers = numberStrings.Select(n => int.Parse(n));
            var negatives = numbers.Where(n => n < 0);
            var positives = numbers.Where(n => n >= 0 && n < 1000);

            if (negatives.Any())
                throw new Exception($"negatives not allowed: {string.Join(",", negatives)}");

            sum = positives.Sum();
            return sum;
        }
        static void Main(string[] args)
        {
            AssertTrue(Add("1,2") == 3);
            AssertTrue(Add("1,23") == 24);

            AssertTrue(Add("10\n20,30") == 60);
            AssertThrowException(() => Add("1,\n23"));

            AssertTrue(Add("//;\n1;2") == 3);
            AssertTrue(Add("//;\n25;30;60") == 115);
            AssertTrue(Add("//;\n25;30\n60") == 115);

            AssertThrowExceptionWithMessage(() => Add("1,-2,-23"), "negatives not allowed: -2,-23");

            AssertTrue(Add("//;\n25;30;1000") == 55);

            AssertTrue(Add("//;\n25;30;1000") == 55);

            AssertTrue(Add("//[;;]\n25;;30") == 55);
            AssertTrue(Add("//[***]\n25***30") == 55);
            AssertTrue(Add("//[***]\n20***30\n50") == 100);
            //AssertTrue(Add("1,23") == 24);
            Console.WriteLine("Hello World!");
        }
    }
}