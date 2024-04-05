// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeDemo
{
    using SpreadsheetEngine.ExpressionTree;

    /// <summary>
    /// Main program of ExpressionTreeDemo.
    /// </summary>
    public static class Program
    {
        // ReSharper disable once InconsistentNaming
        private static ExpressionTree? expressionTree;

        /// <summary>
        /// Main function of the demo program (entry point).
        /// </summary>
        /// <param name="args">String array of args.</param>
        public static void Main(string[] args)
        {
            expressionTree = new ExpressionTree();
            var running = true;

            // Loop until the user decides to quit
            while (running)
            {
                Console.WriteLine("Current Expression: " + expressionTree.InfixStringExpression);
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Enter an expression string");
                Console.WriteLine("2. Set a variable value");
                Console.WriteLine("3. Evaluate expression");
                Console.WriteLine("4. Quit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                Console.WriteLine(); // Spacer
                switch (choice)
                {
                    case "1":
                        EnterExpression();
                        break;
                    case "2":
                        SetVariableValue();
                        break;
                    case "3":
                        EvaluateExpression();
                        break;
                    case "4":
                        running = false;
                        break;

                    // Should the user enter an “option” that isn’t one of these 4, simply ignore it. As trivial as this may
                    // seem it is vital should the assignment be tested with an automated grading app.
                }

                // Avoid use of Console.ReadKey() as it can be problematic when grading
                // with an automated app. Simply fall through to the end of main after the quit option is
                // selected.

                // Fall through to end of main if quit or undefined option was selected (avoid ReadKey)
                if (choice is "1" or "2" or "3")
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }

                Console.Clear();
            }
        }

        /// <summary>
        /// Gets a string infix expression from user input and builds the expression tree from it.
        /// </summary>
        private static void EnterExpression()
        {
            // Get string infix expression
            Console.Write("Enter an expression: ");
            var expression = Console.ReadLine();
            if (expression != null && expressionTree != null && expression.Length > 0)
            {
                // Build and set the expression tree
                expressionTree.SetExpressionTree(expression);
                Console.WriteLine("Expression set successfully.");
            }
            else
            {
                // If the expression tree object or user input are somehow null, or input is empty
                Console.WriteLine("Expression was not set successfully.");
            }
        }

        /// <summary>
        /// Sets the variable in the variable dictionary.
        /// </summary>
        /// <exception cref="InvalidOperationException">Variable name was somehow null.</exception>
        private static void SetVariableValue()
        {
            // Get variable name
            Console.Write("Enter variable name: ");
            var variableName = Console.ReadLine();

            // Get variable value
            Console.Write("Enter variable value: ");

            // Try to convert the value from string to double
            if (double.TryParse(Console.ReadLine(), out var variableValue))
            {
                try
                {
                    // Set the variable name and value in the variable dictionary
                    expressionTree?.SetVariable(variableName ?? throw new InvalidOperationException(), variableValue);
                    Console.WriteLine($"Variable '{variableName}' set to {variableValue} successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid variable value. Please enter a valid number.");
            }
        }

        /// <summary>
        /// Evaluates the current expression tree and prints the double result.
        /// </summary>
        /// <exception cref="NullReferenceException">The expression tree was somehow null.</exception>
        private static void EvaluateExpression()
        {
            try
            {
                // Catch null expressionTree
                if (expressionTree == null)
                {
                    throw new NullReferenceException("expressionTree was null");
                }

                // Evaluate the expression tree and write the result to console
                var result = expressionTree.Evaluate();
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}