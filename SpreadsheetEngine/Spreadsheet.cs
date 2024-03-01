// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.ComponentModel;
using System.Text.RegularExpressions;

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
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
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
        // Extract the cell reference from the text (e.g., "=A5" -> "A5")
        string cellReference = expression.Substring(1); // Remove the '='

        // Define the cell reference pattern as 1+ uppercase followed by 1+ digits
        string cellReferencePattern = "([A-Z]+)([0-9]+)";

        // Assign the reference to a Regex instance
        Regex cellReferenceRegex = new Regex(cellReferencePattern);

        // Match the input against the reference Regex
        Match match = cellReferenceRegex.Match(cellReference);

        // Regex for reference is matched
        if (match.Success)
        {
            // Extract the column and row indices from the matched groups
            string column = match.Groups[1].Value;
            int rowIndex = int.Parse(match.Groups[2].Value) - 1; // Adjust for 0-based indexing

            // Convert the column letters to a zero-based index
            int columnIndex = 0;
            foreach (char c in column)
            {
                columnIndex *= 26; // Multiply by 26 (number of letters in the alphabet)
                columnIndex += c - 'A' + 1; // Add the numerical value of the letter
            }

            columnIndex--; // Adjust for 0-based indexing

            // Simply return the original expression if the reference is invalid
            if (rowIndex < 0 || rowIndex >= this.RowCount || columnIndex < 0 || columnIndex >= this.ColumnCount)
            {
                return expression;
            }

            // If the reference is valid, retrieve the cell based on the calculated indices
            Cell? cell = this.GetCell(rowIndex, columnIndex);

            // Return the value of the referenced cell if it exists
            if (cell != null)
            {
                return cell.Value;
            }
        }

        // Return original expression if reference regex not matched
        return expression;
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