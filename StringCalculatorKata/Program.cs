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
        public static int Add(string input)
        {
            var sum = input.Split(',','\n').Select(n => int.Parse(n)).Sum();
            return sum;
        }
        static void Main(string[] args)
        {
            AssertTrue(Add("1,2") == 3);
            AssertTrue(Add("1,23") == 24);

            AssertTrue(Add("10\n20,30") == 60);
            AssertThrowException(() => Add("1,\n23"));
            Console.WriteLine("Hello World!");
        }
    }
}