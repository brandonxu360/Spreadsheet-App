// <copyright file="FibonacciTextReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3.Models;

// ReSharper disable InconsistentNaming (naming convention conflicts with StyleCop and rubric convention)
using System.IO;
using System.Numerics;

/// <summary>
/// Overrides the TextReader class to read a sequence of Fibonacci numbers.
/// </summary>
public class FibonacciTextReader : TextReader
{
    /// <summary>
    /// Represents the maximum lines/number of numbers that will be generated in the fibonacci sequence.
    /// </summary>
    private readonly int maxLines;

    /// <summary>
    /// Represents the current line (which is to be compared to the maxLines attribute).
    /// </summary>
    private int currentLine;

    /// <summary>
    /// BigInteger value for the larger iterator in the fibonacci sequence (excluding first number of the sequence edge case).
    /// </summary>
    private BigInteger currentNumber;

    /// <summary>
    /// BigInteger value for the smaller iterator in the fibonacci sequence (excluding first number of the sequence edge case).
    /// </summary>
    private BigInteger previousNumber;

    /// <summary>
    /// Flag for iterating on first number in the fibonacci sequence.
    /// </summary>
    private bool firstNumberHandled;

    /// <summary>
    /// Initializes a new instance of the <see cref="FibonacciTextReader"/> class and sets the maxLines attribute.
    /// </summary>
    /// <param name="maxLines">Maximum number of lines available or the maximum number of numbers in the sequence that you can generate.</param>
    public FibonacciTextReader(int maxLines)
    {
        this.maxLines = maxLines;
        this.currentLine = 0;
        this.currentNumber =
            0; // Starts at 0 to allow proper behavior of first two numbers (0, 1, ...), currentNumber will overtake previousNumber after this edge case
        this.previousNumber = 1;
        this.firstNumberHandled = false;
    }

    /// <summary>
    /// Delivers the next line of the Fibonacci sequence as a string.
    /// </summary>
    /// <returns>String representing the next line of the Fibonacci sequence, null after nth call where n is maxLines.</returns>
    public override string? ReadLine()
    {
        // Return null if the current line reaches or exceeds maxLine attribute (enforce maxLine attribute)
        if (this.currentLine >= this.maxLines)
        {
            return null;
        }

        this.currentLine++; // Next if/else statement will return a line no matter what, so we will increment currentLine here

        // If we have already handled the first two numbers
        if (this.firstNumberHandled)
        {
            // Algorithm can be envisioned like an inchworm
            var temp = this.currentNumber;
            this.currentNumber += this.previousNumber; // Current number becomes next number in sequence
            this.previousNumber = temp; // Previous number follows by becoming the old currentNumber value
            return this.currentNumber.ToString();
        }

        // Edge case that we are returning the first number in the sequence
        else
        {
            this.firstNumberHandled = true;
            return this.currentNumber.ToString();
        }
    }

    /// <summary>
    /// Reads all lines until maxLines is met by repeatedly calling ReadLine and concatenating all the resultant strings together.
    /// </summary>
    /// <returns>String representing the lines read within maxLines.</returns>
    public override string ReadToEnd()
    {
        // TODO: implement overridden ReadToEnd body
        return base.ReadToEnd();
    }
}