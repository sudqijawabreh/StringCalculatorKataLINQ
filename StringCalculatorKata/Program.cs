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
            var numbersText = input;
            var delimiters = new List<string>{ delimiter};

            if (input.StartsWith("//"))
            {
                var firstLine = string.Join("", input.Substring(2).TakeWhile(ch => ch != '\n'));
                var brackets = firstLine.Select((ch, i) => (Char:ch, Index:i)).Where(indexChar => indexChar.Char == '[' || indexChar.Char == ']');

                // [0,1,2,3,4,5,6,7,8]
                // 0 / 2  = 0 , 1 / 2 = 0
                // 2 / 2 == 1, 3 / 2 == 1
                // group each 2 brackets together
                var bracketIndexs = brackets.Select((b, i) => (Bracket: b, Index: (i / 2)))
                                    .GroupBy(b => b.Index)
                                    .Select(g =>
                                    {
                                        var OpenBracket = g.First().Bracket;
                                        var CloseBracket = g.Skip(1).First().Bracket;
                                        return (OpenBracket, CloseBracket);
                                    });

                if (bracketIndexs.Any(two => two.OpenBracket.Char != '[' || two.CloseBracket.Char != ']'))
                {
                    throw new Exception();
                }

                var indexes = bracketIndexs.Select(two => (Start: two.OpenBracket.Index + 1, End: two.CloseBracket.Index))
                                            .Select(two => (two.Start, Length: two.End - two.Start));
                delimiters = indexes.Select(index => firstLine.Substring(index.Start, index.Length)).ToList();

                if (!delimiters.Any())
                {
                    delimiter = string.Join("", firstLine.Where(ch => ch != '[' && ch != ']'));
                    delimiters.Add(delimiter);
                }
                numbersText = input.Substring(2 + firstLine.Count() + 1);
            }

            delimiters.Add("\n");

            numberStrings = numbersText.Split(delimiters.ToArray(), StringSplitOptions.None).ToList();
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
            //https://kata-log.rocks/string-calculator-kata
            // simple unit testing mechanisim
            // if now exception happend then all passed

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

            AssertTrue(Add("//[*][;]\n20*30;50") == 100);
            AssertTrue(Add("//[*][;]\n20*30;50\n20") == 120);
            AssertTrue(Add("//[*][;;]\n20*30;;50\n200") == 300);

            AssertThrowException(() => Add("//[*];;]\n20*30;;50\n200"));
            Console.WriteLine("Hello World!");
        }
    }
}