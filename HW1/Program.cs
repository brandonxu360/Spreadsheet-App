using System;

namespace HW1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var inputNums = getUserNums();
        }
        
        // Prompts user for a line of numbers seperated by spaces.
        // Returns the array of numbers as substrings
        // *Assumes input is correctly formatted
        private static string[] getUserNums()
        {
            Console.WriteLine("Enter a collection of numbers in the range [0, 100], seperated by spaces: ");
            var input = Console.ReadLine();
            return input.Split(' ');
        }
    }
}

