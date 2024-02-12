// <copyright file="FibonacciTextReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3Tests;

using HW3.Models;

/// <summary>
/// Unit tests for the FibonacciTextReader functions.
/// </summary>
public class FibonacciTextReaderTests
{
    /// <summary>
    /// Tests if ReadLine returns the correct input for the first two numbers as they are an edge case in the sequence.
    /// </summary>
    [Test]
    public void ReadLine_Returns_First_Two_Numbers()
    {
        // Arrange
        var reader = new FibonacciTextReader(2);

        // Act
        var firstNumber = reader.ReadLine();
        var secondNumber = reader.ReadLine();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(firstNumber, Is.EqualTo("0"));
            Assert.That(secondNumber, Is.EqualTo("1"));
        });
    }

    /// <summary>
    /// Tests that ReadLine returns the correct fibonacci sequence for a sequence of length 10.
    /// </summary>
    [Test]
    public void ReadLine_Returns_Correct_Fibonacci_Sequence()
    {
        // Arrange
        const int maxLines = 10; // Assuming we want to generate the first 10 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);
        string[] expectedNumbers = ["0", "1", "1", "2", "3", "5", "8", "13", "21", "34"];

        // Act
        var actualNumbers = new string?[maxLines];
        for (var i = 0; i < maxLines; i++)
        {
            actualNumbers[i] = reader.ReadLine();
        }

        // Assert
        Assert.That(actualNumbers, Is.EqualTo(expectedNumbers));
    }

    /// <summary>
    /// Tests that ReadLine returns null when maxLines is 0.
    /// </summary>
    [Test]
    public void ReadLine_Returns_Null_If_MaxLines_Is_Zero()
    {
        // Arrange
        var reader = new FibonacciTextReader(0); // No line should be returned (null expected instead)

        // Act
        var result = reader.ReadLine();

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that ReadLine returns null when maxLines is negative.
    /// </summary>
    [Test]
    public void ReadLine_Returns_Null_If_MaxLines_Is_Negative()
    {
        // Arrange
        var reader = new FibonacciTextReader(-1); // No line should be returned (null expected instead)

        // Act
        var result = reader.ReadLine();

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    ///  Tests if ReadLine returns null when maxLines is reached.
    /// </summary>
    [Test]
    public void ReadLine_Returns_Null_After_MaxLines_Calls()
    {
        // Arrange
        const int maxLines = 5; // Assuming we want to generate the first 5 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);

        // Act
        for (var i = 0; i < maxLines; i++)
        {
            reader.ReadLine();
        }

        var result =
            reader.ReadLine(); // Read an additional line to cause lines read to exceed maxLines and store the result

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that ReadToEnd returns the correct fibonacci sequence for a sequence of length 10.
    /// </summary>
    [Test]
    public void ReadToEnd_Returns_Correct_Sequence()
    {
        // Arrange
        const int maxLines = 10; // Assuming we want to generate the first 10 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);
        const string expectedSequence = "0\r\n1\r\n1\r\n2\r\n3\r\n5\r\n8\r\n13\r\n21\r\n34\r\n"; // Fibonacci sequence of length 10

        // Act
        var actualSequence = reader.ReadToEnd();

        // Assert
        Assert.That(actualSequence, Is.EqualTo(expectedSequence));
    }

    /// <summary>
    /// Tests that ReadToEnd returns the correct number of lines/numbers for a fibonacci sequence of length 10.
    /// </summary>
    [Test]
    public void ReadToEnd_Returns_Correct_Number_Of_Lines_For_Ten_MaxLines()
    {
        // Arrange
        const int maxLines = 10; // Assuming we want to generate the first 10 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);

        // Act
        var actualSequence = reader.ReadToEnd();
        var actualNumberOfLines = actualSequence.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length; // RemoveEmptyEntries to remove the empty string at the end

        // Assert
        Assert.That(actualNumberOfLines, Is.EqualTo(maxLines));
    }

    /// <summary>
    /// Tests that ReadToEnd returns the correct number of lines/numbers for a fibonacci sequence of length 100.
    /// </summary>
    [Test]
    public void ReadToEnd_Returns_Correct_Number_Of_Lines_For_Large_MaxLines()
    {
        // Arrange
        const int maxLines = 100; // Assuming we want to generate the first 100 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);

        // Act
        var actualSequence = reader.ReadToEnd();
        var actualNumberOfLines = actualSequence.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length; // RemoveEmptyEntries to remove the empty string at the end

        // Assert
        Assert.That(actualNumberOfLines, Is.EqualTo(maxLines));
    }

    /// <summary>
    /// Tests that ReadToEnd does not return any lines/numbers when maxLines is 0.
    /// </summary>
    [Test]
    public void ReadToEnd_Returns_Empty_String_If_MaxLines_Is_Zero()
    {
        // Arrange
        const int maxLines = 0; // No lines should be returned
        var reader = new FibonacciTextReader(maxLines);

        // Act
        var actualSequence = reader.ReadToEnd();

        // Assert
        Assert.That(actualSequence, Is.Empty);
    }

    /// <summary>
    /// Tests that ReadToEnd does not return any lines/numbers when maxLines is negative.
    /// </summary>
    [Test]
    public void ReadToEnd_Returns_Empty_String_For_Negative_MaxLines()
    {
        // Arrange
        const int maxLines = -1; // Negative max lines
        var reader = new FibonacciTextReader(maxLines);

        // Act
        var actualSequence = reader.ReadToEnd();

        // Assert
        Assert.That(actualSequence, Is.Empty);
    }
}