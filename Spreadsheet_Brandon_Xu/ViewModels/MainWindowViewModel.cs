// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using System;
using System.Collections.Generic;
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

    /// <summary>
    /// Executes the demo.
    /// </summary>
    public void ExecuteRunDemo()
    {
        // Set random cells to "Hello World!"
        Random random = new Random();
        foreach (var rowIndex in Enumerable.Range(0, 50))
        {
            foreach (var columnIndex in Enumerable.Range(0, 'Z' - 'A' + 1))
            {
                var cell = this.spreadsheet?.GetCell(rowIndex, columnIndex);
                if (cell != null)
                {
                    // Set random cells to "Hello World!"
                    if (random.Next(10) < 5)
                    {
                        cell.Text = "Hello World!";
                    }
                }
            }
        }

        // Set column B cells to "This is cell B#"
        foreach (var rowIndex in Enumerable.Range(0, 50))
        {
            var cellB = this.spreadsheet?.GetCell(rowIndex, 1); // Column B
            if (cellB != null)
            {
                cellB.Text = $"This is cell B{rowIndex + 1}";
            }
        }

        // Set column A cells to "=B#"
        foreach (var rowIndex in Enumerable.Range(0, 50))
        {
            var cellA = this.spreadsheet?.GetCell(rowIndex, 0); // Column A
            if (cellA != null)
            {
                cellA.Text = $"=B{rowIndex + 1}";
            }
        }
    }

    /// <summary>
    /// Initialize Spreadsheet object with 26 columns and 50 rows and reference the cells in
    /// the SpreadsheetData as well.
    /// </summary>
    /// <exception cref="InvalidOperationException">Throw when the null is returned by GetCell.</exception>
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
                columns.Add(this.spreadsheet.GetCell(rowIndex, columnIndex) ?? throw new InvalidOperationException());
            }

            this.SpreadsheetData.Add(columns);
        }
    }
}