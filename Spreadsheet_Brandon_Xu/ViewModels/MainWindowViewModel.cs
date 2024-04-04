// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using SpreadsheetEngine;

/// <summary>
/// The MainWindowViewModel to prepare the spreadsheet data to be displayed.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    // ReSharper disable once InconsistentNaming
    private readonly List<CellViewModel> selectedCells = [];

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
    /// Gets or sets the spreadsheet data that is exposed and used by the UI. This is a list of RowViewModels, and each row consists of CellViewModels.
    /// </summary>
    public List<RowViewModel>? SpreadsheetData { get; set; }

    /// <summary>
    /// Resets the current cell selection and selects the cell indicated by the rowIndex and columnIndex parameters.
    /// </summary>
    /// <param name="rowIndex">The row index of the selected cell.</param>
    /// <param name="columnIndex">The column index of the selected cell.</param>
    public void SelectCell(int rowIndex, int columnIndex)
    {
        var clickedCell = this.GetCell(rowIndex, columnIndex);
        var shouldEditCell = clickedCell.IsSelected;

        // Reset the current cell selection
        this.ResetSelection();

        // Add the pressed cell back to the list
        this.selectedCells.Add(clickedCell);
        clickedCell.IsSelected = true;
        if (shouldEditCell)
        {
            clickedCell.CanEdit = true;
        }
    }

    /// <summary>
    /// Toggles the cell IsSelected property and adds or removes the cell from the selectedCells list accordingly.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell to be toggled.</param>
    /// <param name="columnIndex">The column index of the cell to be toggled.</param>
    public void ToggleCellSelection(int rowIndex, int columnIndex)
    {
        var clickedCell = this.GetCell(rowIndex, columnIndex);

        // Toggle from unselected to selected
        if (clickedCell.IsSelected == false)
        {
            this.selectedCells.Add(clickedCell);
            clickedCell.IsSelected = true;
        }

        // Toggle from selected to unselected.
        else
        {
            this.selectedCells.Remove(clickedCell);
            clickedCell.IsSelected = false;
        }
    }

    /// <summary>
    /// Unselect all currently selected cells by modifying each selected cell to reflect an unselected state and clearing the selectedCells list.
    /// </summary>
    public void ResetSelection()
    {
        // Clear current selection
        foreach (var cell in this.selectedCells)
        {
            cell.IsSelected = false;
            cell.CanEdit = false;
        }

        this.selectedCells.Clear();
    }

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
    /// Retrieves the CellViewModel at the specified row and column indices.
    /// </summary>
    /// <param name="rowIndex">The index of the row where the cell is located.</param>
    /// <param name="columnIndex">The index of the column where the cell is located.</param>
    /// <returns>The CellViewModel object corresponding to the specified row and column indices.</returns>
    public CellViewModel GetCell(int rowIndex, int columnIndex)
    {
        // Catch case where SpreadsheetData is null
        if (this.SpreadsheetData == null)
        {
            throw new NullReferenceException("Tried to reference null SpreadSheetData");
        }

        // Catch case where indices are invalid
        if (rowIndex < 0 || rowIndex >= this.SpreadsheetData.Count || columnIndex < 0 ||
            columnIndex >= this.SpreadsheetData[0].Cells.Count)
        {
            throw new IndexOutOfRangeException("Tried to get a cell with invalid index");
        }

        return this.SpreadsheetData[rowIndex].Cells[columnIndex];
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

        this.SpreadsheetData = new List<RowViewModel>(rowCount);
        foreach (var rowIndex in Enumerable.Range(0, rowCount))
        {
            var columns = new List<CellViewModel>(columnCount);
            foreach (var columnIndex in Enumerable.Range(0, columnCount))
            {
                columns.Add(new CellViewModel(this.spreadsheet.GetCell(rowIndex, columnIndex)) ??
                            throw new InvalidOperationException());
            }

            this.SpreadsheetData.Add(new RowViewModel(columns));
        }
    }
}