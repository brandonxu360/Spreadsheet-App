// <copyright file="Cell.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.ComponentModel;

// ReSharper disable InconsistentNaming (stylecop conflicts with IDE suggestions)
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// The abstract Cell class to represent a cell in a grid.
/// </summary>
[SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1401:Fields should be private",
    Justification = "Assignment calls for protected fields; text and value should be accessible to children classes")]
public abstract class Cell : INotifyPropertyChanged
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
    /// The event handler for the cell class.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

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
            // Call OnPropertyChanged if text changes
            this.SetField(ref this.text, value);
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

    /// <summary>
    /// Invokes the event handler when a property is changed.
    /// </summary>
    /// <param name="propertyName">The name of the property being changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Checks if the field is being set to a different value, sets the field and calls OnPropertyChanged if so.
    /// </summary>
    /// <param name="field">The reference to field being assigned a new value.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">The property name.</param>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <returns>Bool indicating if the field was changed.</returns>
    // ReSharper disable once ParameterHidesMember
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
}