// <copyright file="SpreadsheetTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests;

using System.Diagnostics;
using SpreadsheetEngine;

/// <summary>
/// Class to unit test the Spreadsheet functionality.
/// </summary>
[TestFixture]
internal class SpreadsheetTests
{
    /// <summary>
    /// Tests the proper initialization of a spreadsheet.
    /// </summary>
    [Test]
    public void SpreadsheetInitializationTest()
    {
        // Arrange
        const int rowCount = 5;
        const int columnCount = 6;
        var initialValue = string.Empty;

        // Act
        var testSpreadsheet = new Spreadsheet(rowCount, columnCount);

        // Assert
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                // Check that every cell was instantiated and initialized with the correct initial value
                Assert.That(
                    testSpreadsheet.GetCell(row, column)?.Value,
                    Is.EqualTo(initialValue),
                    $"Cell at ({row}, {column}) should be initialized with value {initialValue}");
            }
        }
    }

    /// <summary>
    /// Tests the proper initialization of a spreadsheet with dimensions of 0 (edge case).
    /// </summary>
    [Test]
    public void SpreadsheetInitializationTestEmpty()
    {
        // Arrange
        const int rowCount = 0;
        const int columnCount = 0;

        // Act
        var testSpreadsheet = new Spreadsheet(rowCount, columnCount);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            // The cell array has dimensions of 0, so attempting to retrieve a cell at 0, 0 should throw an IndexOutOfRangeException
            testSpreadsheet.GetCell(0, 0);
        });
    }

    /// <summary>
    /// Tests the improper initialization of a spreadsheet with negative dimensions (exception case).
    /// </summary>
    [Test]
    public void SpreadsheetInitializationTestNegative()
    {
        // Arrange
        const int rowCount = -2;
        const int columnCount = -13;

        // Act & Assert
        Assert.Throws<OverflowException>(() =>
        {
            // The initialization of the Cell array with negative parameters should throw an OverflowException
            // ReSharper disable once UnusedVariable
            var spreadsheet = new Spreadsheet(rowCount, columnCount);
        });
    }

    /// <summary>
    /// Tests cell value update with plaintext text input (no expression evaluation).
    /// </summary>
    [Test]
    public void SpreadsheetCellUpdatePlainText()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 1);
        var cell = spreadsheet.GetCell(0, 0);

        // Act and Assert

        // Value should update to become the text inputted
        Debug.Assert(cell != null, nameof(cell) + " != null");
        cell.Text = "Hello";
        Assert.That(cell.Value, Is.EqualTo("Hello"));
    }

    /// <summary>
    /// Tests cell value update with same plaintext text input as current value (no expression evaluation).
    /// </summary>
    [Test]
    public void SpreadsheetCellUpdateSamePlainText()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 1);
        var cell = spreadsheet.GetCell(0, 0);

        // Act and Assert

        // The value should not change when text is set to the same as current value
        var originalValue = cell?.Value;
        Debug.Assert(originalValue != null, nameof(originalValue) + " != null");
        Debug.Assert(cell != null, nameof(cell) + " != null");
        cell.Text = originalValue;
        Assert.That(cell.Value, Is.EqualTo(originalValue));
    }

    /// <summary>
    /// Tests cell value update with reference text input (evaluate reference to another cell).
    /// </summary>
    [Test]
    public void SpreadsheetCellUpdateReference()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 2);
        var cellA1 = spreadsheet.GetCell(0, 0);
        var cellB1 = spreadsheet.GetCell(0, 1);

        // Set value of A1 to "hello"
        Debug.Assert(cellA1 != null, nameof(cellA1) + " != null");
        cellA1.Value = "hello";

        // Set value of B1 to "not hello"
        Debug.Assert(cellB1 != null, nameof(cellB1) + " != null");
        cellB1.Value = "not hello";

        // Act

        // Set B1 to reference A1 through text
        cellB1.Text = "=A1";

        // Assert

        // B1 value should become "hello"
        Assert.That(cellB1.Value, Is.EqualTo("hello"));
    }

    /// <summary>
    /// Tests cell value update with reference text input to itself (evaluate reference to same cell).
    /// </summary>
    [Test]
    public void SpreadsheetCellUpdateReferenceSelf()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 2);
        var cellA1 = spreadsheet.GetCell(0, 0);
        var cellB1 = spreadsheet.GetCell(0, 1);

        // Set value of A1 to "hello"
        Debug.Assert(cellA1 != null, nameof(cellA1) + " != null");
        cellA1.Value = "hello";

        // Set value of B1 to "not hello"
        Debug.Assert(cellB1 != null, nameof(cellB1) + " != null");
        cellB1.Value = "not hello";

        // Act

        // Set B1 to reference itself (B1) through text
        cellB1.Text = "=B1";

        // Assert

        // B1 value should stay "not hello"
        Assert.That(cellB1.Value, Is.EqualTo("not hello"));
    }

    /// <summary>
    /// Tests cell value update with reference text input to itself when the reference is two characters (short) and value initially empty (evaluate reference to same cell).
    /// For example, cell at B3 with value "" referencing B3
    /// This test case was created due to strange behavior noticed when using the UI.
    /// </summary>
    [Test]
    public void SpreadsheetCellUpdateReferenceSelfShortReferenceAndEmpty()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 2);
        var cellA1 = spreadsheet.GetCell(0, 0);
        var cellB1 = spreadsheet.GetCell(0, 1);

        Debug.Assert(cellA1 != null, nameof(cellA1) + " != null");
        Debug.Assert(cellB1 != null, nameof(cellB1) + " != null");

        // Act

        // Set B1 to reference itself (B1) through text
        cellB1.Text = "=B1";

        // Assert

        // B1 value should stay "" (empty)
        Assert.That(cellB1.Value, Is.EqualTo("0"));
    }

    /// <summary>
    /// Tests cell's ability to take a simple expression input (no variables/references), evaluate, and set the value to the correct result.
    /// </summary>
    /// <param name="expression">The string expression as text input.</param>
    /// <returns>The double evaluated value (the value will be a string, but it will be converted to double for the purposes of this test).</returns>
    /// <exception cref="Exception">The result was not able to be converted to a double, indicating the wrong result.</exception>
    [Test]
    [TestCase("=3+7", ExpectedResult = 10)] // Expression with a single add operator
    [TestCase("=3+7+2+1", ExpectedResult = 13)] // Expression with multiple add operators
    [TestCase("=3/7", ExpectedResult = 3.0 / 7.0)] // Expression with a single division operator
    [TestCase("=3/7/2/1", ExpectedResult = 3.0 / 7.0 / 2.0 / 1.0)] // Expression with multiple division operators
    [TestCase("=0/0", ExpectedResult = 0.0 / 0)] // Expression with multiple division operators

    // Testing operator precedence between addition and subtraction vs multiplication and division
    [TestCase("=3+7/4", ExpectedResult = 3.0 + (7.0 / 4.0))]
    [TestCase("=3*2-5/8", ExpectedResult = (3.0 * 2.0) - (5.0 / 8.0))]

    // Testing parenthesis
    [TestCase("=(3+7)/4", ExpectedResult = (3.0 + 7.0) / 4.0)]
    [TestCase("=3/(7+4)", ExpectedResult = 3.0 / (7.0 + 4.0))]
    public double SpreadsheetEvaluateSimpleExpressionTest(string expression)
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 1);
        var cell = spreadsheet.GetCell(0, 0);

        // Act and Assert
        Debug.Assert(cell != null, nameof(cell) + " != null");
        cell.Text = expression;

        if (!double.TryParse(cell.Value, out var result))
        {
            throw new Exception("Non double result");
        }

        return result;
    }

    /// <summary>
    /// Tests cell's ability to take an expression input with variables/references, evaluate, and set the value to the correct result.
    /// </summary>
    /// <exception cref="Exception">The result was not able to be converted to a double, indicating the wrong result.</exception>
    [Test]
    public void SpreadsheetEvaluateVariableExpression()
    {
        // Arrange
        var spreadsheet = new Spreadsheet(1, 2);
        var cellA1 = spreadsheet.GetCell(0, 0);
        var cellB1 = spreadsheet.GetCell(0, 1);

        Debug.Assert(cellA1 != null, nameof(cellA1) + " != null");
        Debug.Assert(cellB1 != null, nameof(cellB1) + " != null");

        // Act
        cellA1.Text = "20";
        cellB1.Text = "=(2/A1)+3*5";

        if (!double.TryParse(cellB1.Value, out var result))
        {
            throw new Exception("Non double result");
        }

        // Assert
        Assert.That(result, Is.EqualTo(15.1));
    }

    /// <summary>
    /// Test the update of a referencing cell value when the referenced cell value updates.
    /// </summary>
    /// <param name="initialReferencedValue">The initial value of the referenced cell.</param>
    /// <param name="finalReferencedValue">The final/updated value of the referenced cell.</param>
    /// <param name="referencingText">The text of the referencing cell (set to reference the referenced cell).</param>
    /// <returns>The updated value of the referencing cell.</returns>
    [Test]
    [TestCase("20", "10", "=A1", ExpectedResult = "10")] // Single value update
    [TestCase("20", "40", "=A1+60", ExpectedResult = "100")] // Value update for a value within an expression
    [TestCase("20", "hello I am not a double", "=A1", ExpectedResult = "hello I am not a double")] // Referenced cell value updated to value not able to parse to double
    [TestCase("20", "hello I am not a double", "=A1+5", ExpectedResult = "=A1+5")] // Referenced cell value updated such that previously valid referencing text expression becomes invalid

    [TestCase("hello", "20", "=A1+5", ExpectedResult = "25")] // Referenced cell value updated such that previously invalid referencing text expression becomes valid
    public string SpreadsheetUpdateReferencingCellTest(string initialReferencedValue, string finalReferencedValue, string referencingText)
    {
        // Arrange (set up referencing and referenced cells)
        var spreadsheet = new Spreadsheet(1, 2);
        var cellA1 = spreadsheet.GetCell(0, 0);
        var cellB1 = spreadsheet.GetCell(0, 1);

        Debug.Assert(cellA1 != null, nameof(cellA1) + " != null");
        Debug.Assert(cellB1 != null, nameof(cellB1) + " != null");

        cellA1.Value = initialReferencedValue;
        cellB1.Text = referencingText;

        // Act (change referenced cell value)
        cellA1.Value = finalReferencedValue;

        // Assert (referencing cell value updates to reflect new referenced cell value)
        return cellB1.Value;
    }
}