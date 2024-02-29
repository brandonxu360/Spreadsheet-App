// <copyright file="SpreadsheetEngineTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests;

using SpreadsheetEngine;

/// <summary>
/// Class to contain the tests for the SpreadsheetEngine functionality tests.
/// </summary>
internal class SpreadsheetEngineTests
{
    /// <summary>
    /// Tests the proper initialization of a spreadsheet.
    /// </summary>
    [Test]
    public void SpreadsheetInitializationTest()
    {
        // Arrange
        const int rowCount = 5;
        const int columnCount = 5;
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
            // The cell array has dimensions of 0, so attempting to retrieve a cell at 0, 0 should throw an IndexOutofRangeException
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
        Assert.That(cell, Is.Not.Null);
        if (cell == null)
        {
            return;
        }

        // Value should update to become the text inputted
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
        Assert.That(cell, Is.Not.Null);
        if (cell == null)
        {
            return;
        }

        // The value should not change when text is set to the same as current value
        var originalValue = cell.Value;
        cell.Text = originalValue;
        Assert.That(cell.Value, Is.EqualTo(originalValue));
    }
}