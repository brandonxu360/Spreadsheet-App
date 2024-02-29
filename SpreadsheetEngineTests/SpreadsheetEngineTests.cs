// <copyright file="SpreadsheetEngineTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests;

using SpreadsheetEngine;

/// <summary>
/// Class to contain the tests for the SpreadsheetEngine functionality tests.
/// </summary>
public class SpreadsheetEngineTests
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
}