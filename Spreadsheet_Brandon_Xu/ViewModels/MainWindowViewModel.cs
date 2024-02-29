// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SpreadsheetEngine;

/// <summary>
/// The MainWindowViewModal to prepare the spreadsheet data to be displayed.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The instance of the spreadsheet being operated on.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private Spreadsheet? spreadsheet;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
        this.InitializeSpreadsheet();
    }

    /// <summary>
    /// Gets or sets the 2D list of Cells that are bound to the UI. Initializes spreadsheet and SpreadsheetData.
    /// </summary>
    public List<List<Cell>>? SpreadsheetData { get; set; }

    private void InitializeSpreadsheet()
    {
        const int rowCount = 50;
        const int columnCount = 'Z' - 'A' + 1;

        this.spreadsheet = new Spreadsheet(rowCount, columnCount);

        this.SpreadsheetData = new List<List<Cell>>(rowCount);
        foreach (var rowIndex in Enumerable.Range(0, rowCount))
        {
            var columns = new List<Cell>(columnCount);
            foreach (var columnIndex in Enumerable.Range(0, columnCount))
            {
                columns.Add(this.spreadsheet.GetCell(rowIndex, columnIndex));
            }

            this.SpreadsheetData.Add(columns);
        }
    }

}