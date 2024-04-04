// <copyright file="MainWindowViewModelTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu_Tests;

using System.Diagnostics;
using Spreadsheet_Brandon_Xu.ViewModels;

/// <summary>
/// Unit tests for the MainWindowViewModel.
/// </summary>g
public class MainWindowViewModelTests
{
    /// <summary>
    /// The MainWindowViewModel used for testing.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private MainWindowViewModel? viewModel;

    /// <summary>
    /// Set up for each unit test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Instantiate a MainWindowViewModel
        this.viewModel = new MainWindowViewModel();
    }

    /// <summary>
    /// Tests the normal case of SelectCell to ensure cell is marked as selected after calling SelectCell.
    /// </summary>
    [Test]
    public void SelectCellTest()
    {
        // Arrange
        const int rowIndex = 0;
        const int columnIndex = 0;
        Debug.Assert(this.viewModel != null, nameof(this.viewModel) + " != null");

        // Act
        this.viewModel.SelectCell(rowIndex, columnIndex);

        // Assert
        var selectedCell = this.viewModel?.GetCell(rowIndex, columnIndex);
        Debug.Assert(selectedCell != null, nameof(selectedCell) + " != null");
        Assert.That(selectedCell.IsSelected, Is.True);
    }

    /// <summary>
    /// Tests the normal case of ToggleCellSelection where the cell is toggled twice (not selected -> selected -> not selected).
    /// </summary>
    [Test]
    public void ToggleCellSelectionTest()
    {
        // Arrange
        const int rowIndex = 0;
        const int columnIndex = 0;
        Debug.Assert(this.viewModel != null, nameof(this.viewModel) + " != null");

        // Act
        this.viewModel.ToggleCellSelection(rowIndex, columnIndex);
        this.viewModel.ToggleCellSelection(rowIndex, columnIndex);

        // Assert
        var selectedCell = this.viewModel.GetCell(rowIndex, columnIndex);
        Assert.That(selectedCell.IsSelected, Is.False);
    }

    /// <summary>
    /// Tests the normal case of ResetSelection to ensure all selected cells are unselected after calling ResetSelection.
    /// </summary>
    [Test]
    public void ResetSelection_AllCellsAreDeselected()
    {
        // Arrange
        Debug.Assert(this.viewModel != null, nameof(this.viewModel) + " != null");
        this.viewModel.SelectCell(0, 0);
        this.viewModel.SelectCell(1, 1);

        // Act
        this.viewModel.ResetSelection();

        // Assert
        Debug.Assert(this.viewModel.SpreadsheetData != null, "viewModel.SpreadsheetData != null");
        foreach (var cell in this.viewModel.SpreadsheetData.SelectMany(row => row.Cells))
        {
            Assert.That(cell.IsSelected, Is.False);
        }
    }
}