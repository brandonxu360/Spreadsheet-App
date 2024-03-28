// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.ComponentModel;
using System.Globalization;

/// <summary>
/// The spreadsheet class that will serve as a container for a 2D array of cells. It will also serve
/// as a factory for spreadsheet cells.
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// The 2D array of cells to represent the cells of the spreadsheet.
    /// </summary>
    // ReSharper disable once InconsistentNaming (conflicts with stylecop)
    private readonly Cell?[,] cells;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// </summary>
    /// <param name="rowCount">The number of rows in the spreadsheet.</param>
    /// <param name="columnCount">The number of columns in the spreadsheet.</param>
    public Spreadsheet(int rowCount, int columnCount)
    {
        this.RowCount = rowCount;
        this.ColumnCount = columnCount;

        // Initialize the 2D array of cells according to the provided dimensions
        this.cells = new Cell[rowCount, columnCount];

        // Create a spreadsheet cell and assign it to each position in the cell array
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < columnCount; j++)
            {
                this.cells[i, j] = new SpreadsheetCell(i, j);

                // Make sure the cell was successfully created and nonnull
                if (this.cells[i, j] != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    this.cells[i, j].PropertyChanged += this.OnCellPropertyChanged;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }
    }

    /// <summary>
    /// Gets the number of columns in the spreadsheet.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the number of rows in the spreadsheet.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <param name="columnIndex">The column index of the cell.</param>
    /// <returns>The Cell object at the column and cell index.</returns>
    public Cell? GetCell(int rowIndex, int columnIndex)
    {
        return this.cells[rowIndex, columnIndex] ?? null;
    }

    /// <summary>
    /// Returns the value of the cell with the specified name.
    /// </summary>
    /// <param name="cellName">The name of the cell (e.g., "A1").</param>
    /// <returns>The value of the cell.</returns>
    private string GetCellValue(string cellName)
    {
        // Parse the cell name to extract row and column indices
        var columnIndex = cellName[0] - 'A'; // Convert the column letter to a zero-based index
        var rowIndex = int.Parse(cellName[1..]) - 1; // Parse the row number and convert to zero-based index

        // Check if the indices are within the bounds of the spreadsheet
        if (rowIndex < 0 || rowIndex >= this.RowCount || columnIndex < 0 || columnIndex >= this.ColumnCount)
        {
            throw new ArgumentException("Cell name is out of range.");
        }

        // Retrieve the cell object from the 2D array and return its value
        var cell = this.GetCell(rowIndex, columnIndex);
        return cell?.Value ?? string.Empty; // Return the cell value or an empty string if the cell is null
    }

    /// <summary>
    /// Executed when a cell property is changed, will call the cell Value setter which has
    /// its own implementation of setting the cell value.
    /// </summary>
    /// <param name="sender">Cell that had its property changed.</param>
    /// <param name="e">PropertyChanged event arguments.</param>
    private void OnCellPropertyChanged(object? sender, PropertyChangedEventArgs? e)
    {
        if (sender is Cell cell && e?.PropertyName == nameof(Cell.Text))
        {
            // Expression
            if (cell.Text.StartsWith('=') && cell.Text.Length > 1)
            {
                cell.Value = this.EvaluateExpression(cell.Text);
            }

            // Plaintext
            else
            {
                cell.Value = cell.Text;
            }
        }
    }

    private string EvaluateExpression(string expression)
    {
        // Extract the cell expression from the text (e.g., "=A5" -> "A5")
        var strippedExpression = expression[1..]; // Remove the '='

        // Try building the expression tree
        try
        {
            // Build the expression tree using ExpressionTree
            var expressionTree = new ExpressionTree(strippedExpression);

            // Get a list of all the variables found when building the ExpressionTree (ExpressionTree's variable dictionary)
            var references = expressionTree.GetVariableNames();

            // If any of the variable names is a Cell reference (ie. A1, B12), get the value of that cell and set it in the ExpressionTree variable dictionary
            foreach (var reference in references)
            {
                if (this.GetCellValue(reference) != string.Empty)
                {
                    expressionTree.SetVariable(reference, Convert.ToDouble(this.GetCellValue(reference)));
                }
            }

            // Call evaluate on the ExpressionTree and return the value as a string
            return expressionTree.Evaluate().ToString(CultureInfo.InvariantCulture);
        }

        // If the expression tree cannot be built because the reference values (variable values) are invalid (do not hold doubles)
        catch (FormatException)
        {
            // Try returning the value of the referenced cell
            try
            {
                return this.GetCellValue(strippedExpression);
            }

            // Invalid reference
            catch (FormatException)
            {
                // Return the original expression
                return expression;
            }
        }

        // Invalid expression (ie. "=A1+")
        catch (InvalidOperationException)
        {
            // Return the original expression
            return expression;
        }
    }

    private class SpreadsheetCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index of the cell.</param>
        /// <param name="columnIndex">The column index of the cell.</param>
        public SpreadsheetCell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }

        protected override void SetValue(string newValue)
        {
            this.value = newValue;
        }
    }
}