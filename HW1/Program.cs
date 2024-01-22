using System;

namespace HW1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var myBST = new BST();
            
            // Get collection of numbers from user and store in inputNums as an array of substrings
            var inputNums = GetUserNums();
            
            // Parse each substring into an int and add it to a BST
            foreach (var subS in inputNums)
            {
                myBST.Add(int.Parse(subS));
            }
            
            // Display BST and statistics
            myBST.DisplayInOrder();
            Console.WriteLine($"Node Count: {myBST.NodeCount()}");
            Console.WriteLine($"Level Count: {myBST.LevelCount()}");
            
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

