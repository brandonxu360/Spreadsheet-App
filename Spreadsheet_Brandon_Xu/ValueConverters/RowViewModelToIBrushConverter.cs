// <copyright file="RowViewModelToIBrushConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ValueConverters;

using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Spreadsheet_Brandon_Xu.ViewModels;

/// <summary>
/// Responsible for converting between RowViewModel and SolidColorBrush (to be used by the UI).
/// </summary>
public class RowViewModelToIBrushConverter : IValueConverter
{
    /// <summary>
    /// The instance of the RowViewModelToIBrushConverter.
    /// </summary>
    public static readonly RowViewModelToIBrushConverter Instance = new();

    /// <summary>
    /// The current row.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private RowViewModel? currentRow;

    /// <summary>
    /// The cell count (initialized at 0).
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private int cellCounter;

    /// <summary>
    /// Overridden convert method to convert RowViewModel to SolidColorBrush.
    /// </summary>
    /// <param name="value">The value to be converted, expected to be a RowViewModel.</param>
    /// <param name="targetType">The type of the target property (not used).</param>
    /// <param name="parameter">An optional parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>The SolidColorBrush representing the background color of the current cell in the row.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // If the converter used for the wrong type throw an exception
        if (value is not RowViewModel row)
        {
            return new BindingNotification(
                new InvalidCastException(),
                BindingErrorType.Error);
        }

        // NOTE: Rows are rendered from column 0 to n and in order
        if (this.currentRow != row)
        {
            this.currentRow = row;
            this.cellCounter = 0;
        }

        var brush = this.currentRow.Cells[this.cellCounter].IsSelected
            ? new SolidColorBrush(0xff3393df)
            : new SolidColorBrush(this.currentRow.Cells[this.cellCounter].BackgroundColor);
        this.cellCounter++;
        if (this.cellCounter >= this.currentRow.Cells.Count)
        {
            this.currentRow = null;
        }

        return brush;
    }

    /// <summary>
    /// Not implemented. Conversion back from SolidColorBrush to RowViewModel is not supported.
    /// </summary>
    /// <param name="value">The value to be converted back, expected to be a SolidColorBrush.</param>
    /// <param name="targetType">The type of the target property (not used).</param>
    /// <param name="parameter">An optional parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>Throws a NotImplementedException.</returns>
    /// <exception cref="NotImplementedException">Thrown always as conversion back is not supported.</exception>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}