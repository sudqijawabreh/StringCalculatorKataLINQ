using System;
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
        public static int Add(string input)
        {
            var sum = input.Split(",").Select(n => int.Parse(n)).Sum();
            return sum;
        }
        static void Main(string[] args)
        {
            AssertTrue(Add("1,2") == 3);
            AssertTrue(Add("1,23") == 24);
            Console.WriteLine("Hello World!");
        }
    }
}