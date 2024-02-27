// <copyright file="Cell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

// ReSharper disable InconsistentNaming (stylecop conflicts with IDE suggestions)
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The abstract Cell class to represent a cell in a grid.
/// </summary>
[SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1401:Fields should be private",
    Justification = "Assignment calls for protected fields; text and value should be accessible to children classes")]
public abstract class Cell
{
    /// <summary>
    /// The encapsulated text of the cell (input of the cell).
    /// </summary>
    protected string text;

    /// <summary>
    /// The encapsulated value of the cell (expression-evaluated output of the cell).
    /// </summary>
    protected string value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <param name="columnIndex">The column index of the cell.</param>
    protected Cell(int rowIndex, int columnIndex)
    {
        this.RowIndex = rowIndex;
        this.ColumnIndex = columnIndex;

        this.text = string.Empty;
        this.value = string.Empty;
    }

    /// <summary>
    /// Gets the row index of the cell in the grid.
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// Gets the column index of the cell in the grid.
    /// </summary>
    public int ColumnIndex { get; }

    /// <summary>
    /// Gets or sets the text of the cell.
    /// </summary>
    public string Text
    {
        get => this.text;

        set
        {
            // Placeholder implementation
            this.text = value;
        }
    }

    /// <summary>
    /// Gets or sets the value of the cell.
    /// </summary>
    public virtual string Value
    {
        get => this.value;

        // Must be implemented in children class
        set => this.SetValue(value);
    }

    /// <summary>
    /// Implementation of the value setter, which must be implemented in children classes.
    /// </summary>
    /// <param name="newValue">The new value to set the value attribute to.</param>
    protected abstract void SetValue(string newValue);
}