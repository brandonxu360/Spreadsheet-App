// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// The spreadsheet class that will serve as a container for a 2D array of cells. It will also serve
/// as a factory for spreadsheet cells.
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// The 2D array of cells to represent the cells of the spreadsheet.
    /// </summary>
    private Cell?[,]? cells;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// </summary>
    /// <param name="rowCount">The number of rows in the spreadsheet.</param>
    /// <param name="columnCount">The number of columns in the spreadsheet.</param>
    public Spreadsheet(int rowCount, int columnCount)
    {
    }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="columnIndex">The column index of the cell.</param>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <returns>The Cell object at the column and cell index.</returns>
    public Cell? GetCell(int columnIndex, int rowIndex)
    {
        return this.cells?[rowIndex, columnIndex] ?? null;
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
            // Placeholder implementation
            this.value = newValue;
        }
    }
}