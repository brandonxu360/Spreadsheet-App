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
    private int ColumnCount { get; }

    /// <summary>
    /// Gets the number of rows in the spreadsheet.
    /// </summary>
    private int RowCount { get; }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <param name="columnIndex">The column index of the cell.</param>
    /// <returns>The Cell object at the column and cell index.</returns>
    public Cell GetCell(int rowIndex, int columnIndex)
    {
        return this.cells[rowIndex, columnIndex] ?? throw new IndexOutOfRangeException();
    }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="cellName">The cell name/reference (ie. "A1").</param>
    /// <returns>The Cell object at the specified location.</returns>
    private Cell GetCell(string cellName)
    {
        // Parse the cell name to extract row and column indices
        var columnIndex = cellName[0] - 'A'; // Convert the column letter to a zero-based index
        var rowIndex = int.Parse(cellName[1..]) - 1; // Parse the row number and convert to zero-based index

        // Check if the indices are within the bounds of the spreadsheet
        if (rowIndex < 0 || rowIndex >= this.RowCount || columnIndex < 0 || columnIndex >= this.ColumnCount)
        {
            throw new ArgumentException("Cell name is out of range.");
        }

        // Retrieve the cell object from the 2D array and return it
        return this.GetCell(rowIndex, columnIndex);
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
                cell.Value = this.EvaluateExpression(cell.Text, cell);
            }

            // Plaintext
            else
            {
                cell.Value = cell.Text;
            }
        }
    }

    /// <summary>
    /// Attempts to build and evaluate the value of the expression (indicated with an = sign), and handles subscribing and unsubscribing referencing cells from referenced
    /// cell events.
    /// </summary>
    /// <param name="expression">The string expression input taken directly from the cell text (includes the =).</param>
    /// <param name="sender">The cell that had its property changed.</param>
    /// <returns>The string representation for the evaluation of the expression.</returns>
    /// <exception cref="Exception">Any errors encountered during the building/evaluating of the expression will return "#Invalid expression" code.</exception>
    private string EvaluateExpression(string expression, Cell sender)
    {
        // Extract the cell expression from the text (e.g., "=A5" -> "A5")
        var strippedExpression = expression[1..]; // Remove the '='

        // Unsubscribe the cell from all cells previously subscribed to (will resubscribe depending on new expression references)
        foreach (var reference in sender.ReferencedCellNames)
        {
            this.GetCell(reference).PropertyChanged -= sender.OnReferencedCellPropertyChanged;
            sender.ReferencedCellNames.Remove(reference);
        }

        // Try to build and evaluate an expression tree
        try
        {
            // Build the expression tree using ExpressionTree
            var expressionTree = new ExpressionTree.ExpressionTree(strippedExpression);

            // Get a list of all the references/variables found when building the ExpressionTree (ExpressionTree's variable dictionary)
            var references = expressionTree.GetVariableNames();

            // If simple single reference to cell, subscribe to cell and return the value of the referenced cell (or throw ref exception if string is empty)
            if (references.Count > 0 && strippedExpression == references.First())
            {
                this.SubscribeToReferencedCells(sender, references.First());
                var referencedValue = this.GetCell(references.First()).Value;
                return referencedValue != string.Empty ? referencedValue : "#InvalidRef";
            }

            // Otherwise, iterate through each reference to subscribe to the cell, set the variable value, and return the final evaluated value
            foreach (var reference in references)
            {
                // Subscribe to the references (variables) found when building the tree
                this.SubscribeToReferencedCells(sender, reference);

                // Get the value from the referenced cell
                var stringValue = this.GetCell(reference).Value;

                // If the cell value is valid (double), set variable accordingly
                if (double.TryParse(stringValue, out var doubleValue))
                {
                    expressionTree.SetVariable(reference, doubleValue);
                }
                else
                {
                    throw new Exception("#InvalidRef");
                }
            }

            // Finally, call evaluate on the ExpressionTree and return the value as a string
            return expressionTree.Evaluate().ToString(CultureInfo.InvariantCulture);
        }

        // Return error for invalid references whenever expression fails to build/evaluate
        catch (Exception)
        {
            return "#InvalidRef";
        }
    }

    /// <summary>
    /// Subscribe referencing cell to referenced cell event (if not already subscribed).
    /// </summary>
    /// <param name="sender">The referencing cell (subscriber).</param>
    /// <param name="reference">The referenced cell (notifier).</param>
    private void SubscribeToReferencedCells(Cell sender, string reference)
    {
        // If the referenced cell is already subscribed to (as indicated in the cell's ReferencedCellNames property), do not subscribe again
        if (sender.ReferencedCellNames.Contains(reference))
        {
            return;
        }

        this.GetCell(reference).PropertyChanged += sender.OnReferencedCellPropertyChanged;
        sender.ReferencedCellNames.Add(reference);
    }

    /// <summary>
    /// Embedded SpreadsheetCell to expose the setter privileges only to the Spreadsheet class.
    /// </summary>
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

        /// <summary>
        /// Overridden setter for the value of a cell.
        /// </summary>
        /// <param name="newValue">The new string to set the value to.</param>
        protected override void SetValue(string newValue)
        {
            this.value = newValue;
        }
    }
}