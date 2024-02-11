// <copyright file="FibonacciTextReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3.Models;

using System.IO;

/// <summary>
/// Overrides the TextReader class to read a sequence of Fibonacci numbers.
/// </summary>
public class FibonacciTextReader : TextReader
{
    /// <summary>
    /// Represents the maximum lines/number of numbers that will be generated in the fibonacci sequence.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private int maxLines;

    /// <summary>
    /// Initializes a new instance of the <see cref="FibonacciTextReader"/> class and sets the maxLines attribute.
    /// </summary>
    /// <param name="maxLines">Maximum number of lines available or the maximum number of numbers in the sequence that you can generate.</param>
    public FibonacciTextReader(int maxLines)
    {
        this.maxLines = maxLines;
    }

    /// <summary>
    /// Delivers the next line of the Fibonacci sequence as a string.
    /// </summary>
    /// <returns>String representing the next line of the Fibonacci sequence, null after nth call where n is maxLines.</returns>
    public override string? ReadLine()
    {
        // TODO: implement overridden ReadLine body
        return base.ReadLine();
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