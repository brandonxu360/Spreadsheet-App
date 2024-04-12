// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using ReactiveUI;
using SpreadsheetEngine;

/// <summary>
/// The MainWindowViewModel to prepare the spreadsheet data to be displayed.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// A collection of the currently selected cells.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly List<CellViewModel> selectedCells = [];

    /// <summary>
    /// Field for the undo menu item header.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private string undoMenuItemHeader;

    /// <summary>
    /// Field for the redo menu item header.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private string redoMenuItemHeader;

    /// <summary>
    /// The instance of the spreadsheet being operated on.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private Spreadsheet? spreadsheet;

    /// <summary>
    /// Value indicating an undo command is available (used for enabling/disabling the undo menu item).
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private bool undoAvailable;

    /// <summary>
    /// Value indicating a redo command is available (used for enabling/disabling the redo menu item).
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private bool redoAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
        this.undoAvailable = false;
        this.redoAvailable = false;

        // Set menu headers to default
        this.undoMenuItemHeader = "Undo";
        this.redoMenuItemHeader = "Redo";

        // Create an interaction between the view model and the view for the file to be loaded:
        this.AskForFileToLoad = new Interaction<Unit, IStorageFile?>();

        // Similarly to load, there is a need to create an interaction for saving into a file:
        this.AskForFileToSave = new Interaction<Unit, IStorageFile?>();

        this.InitializeSpreadsheet();
    }

    /// <summary>
    /// Gets prompts the view to allow the user to select a file for loading by triggering the DoOpenFile method in the MainWindow.
    /// </summary>
    public Interaction<Unit, IStorageFile?> AskForFileToLoad { get; }

    /// <summary>
    /// Gets prompts the view to allow the user to select a file for loading by triggering the DoOpenFile method in the MainWindow.
    /// </summary>
    public Interaction<Unit, IStorageFile?> AskForFileToSave { get; }

    /// <summary>
    /// Gets or sets a value indicating whether an undo command is available (used for enabling/disabling the undo menu item).
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public bool UndoAvailable
    {
        get => this.undoAvailable;
        set => this.RaiseAndSetIfChanged(ref this.undoAvailable, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a redo command is available (used for enabling/disabling the redo menu item).
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public bool RedoAvailable
    {
        get => this.redoAvailable;
        set => this.RaiseAndSetIfChanged(ref this.redoAvailable, value);
    }

    /// <summary>
    /// Gets or sets the menu item header of the undo menu option.
    /// </summary>
    public string UndoMenuItemHeader
    {
        get => this.undoMenuItemHeader;
        set => this.RaiseAndSetIfChanged(ref this.undoMenuItemHeader, value);
    }

    /// <summary>
    /// Gets or sets the menu item header of the redo menu option.
    /// </summary>
    public string RedoMenuItemHeader
    {
        get => this.redoMenuItemHeader;
        set => this.RaiseAndSetIfChanged(ref this.redoMenuItemHeader, value);
    }

    /// <summary>
    /// Gets or sets the spreadsheet data that is exposed and used by the UI. This is a list of RowViewModels, and each row consists of CellViewModels.
    /// </summary>
    public List<RowViewModel>? SpreadsheetData { get; set; }

    /// <summary>
    /// This method will be executed when the user wants to load content from a file.
    /// </summary>
    public async void LoadFromFile()
    {
        // Wait for the user to select the file to load from.
        var filePath = await this.AskForFileToLoad.Handle(default);
        if (filePath == null)
        {
            return;
        }

        // If the user selected a file, open writing stream from the file.
        await using var stream = await filePath.OpenReadAsync();
        this.spreadsheet?.LoadFromStream(stream);
    }

    /// <summary>
    /// This method will be executed when the user wants to load content from a file.
    /// </summary>
    public async void SaveToFile()
    {
        // Wait for user to select file to save to.
        var file = await this.AskForFileToSave.Handle(default);
        if (file == null)
        {
            return;
        }

        // Open writing stream from the file.
        await using var stream = await file.OpenWriteAsync();
        this.spreadsheet?.SaveToStream(stream);
    }

    /// <summary>
    /// Expose EditCellText functionality from the Spreadsheet to the UI.
    /// </summary>
    /// <param name="rowIndex">The row index corresponding to the cell that was edited.</param>
    /// <param name="columnIndex">The column index corresponding to the cell that was edited.</param>
    /// <param name="newText">The new text to set the cell text to.</param>
    public void EditCellText(int rowIndex, int columnIndex, string newText)
    {
        this.spreadsheet?.EditCellText(rowIndex, columnIndex, newText);
        this.UpdateUndoRedoHeaders();
    }

    /// <summary>
    /// Expose ChangeCellColor functionality from the Spreadsheet to the UI.
    /// </summary>
    /// <param name="newColor">The new uint color to set the cell background color to.</param>
    public void ChangeSelectedCellColor(uint newColor)
    {
        // Map the selected cell CellViewModels to a list of Cell objects to pass to the spreadsheet
        List<Cell> cells = this.selectedCells.Select(cellViewModel => cellViewModel.Cell).ToList();

        this.spreadsheet?.ChangeCellColor(cells, newColor);
        this.UpdateUndoRedoHeaders();
    }

    /// <summary>
    /// Expose undo functionality from the Spreadsheet to the UI.
    /// </summary>
    public void UndoCommand()
    {
        this.spreadsheet?.Undo();
        this.UpdateUndoRedoHeaders();
    }

    /// <summary>
    /// Expose redo functionality from the Spreadsheet to the UI.
    /// </summary>
    public void RedoCommand()
    {
        this.spreadsheet?.Redo();
        this.UpdateUndoRedoHeaders();
    }

    /// <summary>
    /// Changes the color of the selected cells to the Color passed in as input.
    /// </summary>
    /// <param name="color">The color to change the selected cell colors to.</param>
    public void ChangeSelectedCellColor(Color color)
    {
        foreach (var cell in this.selectedCells)
        {
            cell.BackgroundColor = color.ToUInt32();
        }
    }

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
    /// Helper function to determine if any of the selected cells have a different background color from the passed-in color.
    /// Used to determine whether to change the color of the selected cells when the color picker color changes.
    /// </summary>
    /// <param name="color">The uint color to compare the selected cell colors against.</param>
    /// <returns>True if the selected cells have at least one different background color, false otherwise.</returns>
    public bool SelectedCellsContainDiffColor(uint color)
    {
        foreach (var cell in this.selectedCells)
        {
            if (cell.BackgroundColor != color)
            {
                return true;
            }
        }

        return false;
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

    /// <summary>
    /// Update the undo/redo headers.
    /// Set the header to describe a specific undo or redo action if the corresponding command exists.
    /// Enable the header if the command exists, disable otherwise.
    /// </summary>
    /// <exception cref="InvalidOperationException">The referenced spreadsheet is null.</exception>
    private void UpdateUndoRedoHeaders()
    {
        // Catch exceptional case where referenced spreadsheet is null
        if (this.spreadsheet == null)
        {
            throw new InvalidOperationException();
        }

        var undoCommand = this.spreadsheet.TryGetUndoCommand();
        var redoCommand = this.spreadsheet.TryGetRedoCommand();

        // * UPDATE UNDO MENU OPTION

        // If undo command does not exist
        if (undoCommand == null)
        {
            // Default undo header text
            this.UndoMenuItemHeader = "Undo";

            // Disable undo option
            this.UndoAvailable = false;
        }

        // If undo command exists
        else
        {
            // Use the command title
            this.UndoMenuItemHeader = "Undo " + undoCommand.GetCommandTitle();

            // Enable undo option
            this.UndoAvailable = true;
        }

        // * UPDATE REDO MENU OPTION

        // If redo command does not exist
        if (redoCommand == null)
        {
            // Default redo menu header
            this.RedoMenuItemHeader = "Redo";

            // Disable undo option
            this.RedoAvailable = false;
        }

        // If undo command exists, set header to title
        else
        {
            // Use the command title
            this.RedoMenuItemHeader = "Redo " + redoCommand.GetCommandTitle();

            // Enable undo option
            this.RedoAvailable = true;
        }
    }
}