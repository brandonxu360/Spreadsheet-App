using System;

namespace HW1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Get collection of numbers from user and store in inputNums as an array of substrings
            var inputNums = GetUserNums();
            
            // Rudimentary testing
            var myBst = new BST();
            myBst.Add(1);
            myBst.Add(2);
            myBst.Add(3);
            myBst.Add(211);
            Console.WriteLine(myBst.Count());
            myBst.DisplayInOrder();
        }
        
        // Prompts user for a line of numbers seperated by spaces.
        // Returns the array of numbers as substrings
        // *Assumes input is correctly formatted
        private static string[] GetUserNums()
        {
            // Prompt user with input requirements
            Console.WriteLine("Enter a collection of numbers in the range [0, 100], seperated by spaces: ");
            
            // Read and store user input
            var input = Console.ReadLine();
            
            // Split and return array of substrings
            return input.Split(' ');
        }
    }
}

