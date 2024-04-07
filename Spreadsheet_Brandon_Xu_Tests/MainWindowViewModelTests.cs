// <copyright file="MainWindowViewModelTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Avalonia.Media;

namespace Spreadsheet_Brandon_Xu_Tests;

using System.Diagnostics;
using Spreadsheet_Brandon_Xu.ViewModels;

/// <summary>
/// Unit tests for the MainWindowViewModel.
/// </summary>
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

    /// <summary>
    /// Tests that the exposed EditCellText method, which uses commands to edit the cell text, functions as expected.
    /// </summary>
    [Test]
    public void EditCellTextWithCommand()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        const string newText = "boohoo new text";

        // Act
        viewModel.EditCellText(0, 0, newText);

        // Assert
        Assert.That(cellA1.Text, Is.EqualTo(newText));
    }

    /// <summary>
    /// Tests that the exposed EditCellText method, which uses commands to edit the cell text, functions as expected.
    /// </summary>
    [Test]
    public void ChangeCellBackgroundColorWithCommand()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        viewModel.SelectCell(0, 0);
        var newColor = Colors.Aqua.ToUInt32();

        // Act
        viewModel.ChangeSelectedCellColor(newColor);

        // Assert
        Assert.That(cellA1.BackgroundColor, Is.EqualTo(newColor));
    }

    /// <summary>
    /// Tests the exposed Undo function, which uses commands to apply an undo, on a text edit.
    /// </summary>
    [Test]
    public void UndoTextEditTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        var initialText = cellA1.Text;
        var secondaryText = "blah blah";
        viewModel.EditCellText(0, 0, secondaryText);

        // Act
        viewModel.UndoCommand();

        // Assert
        Assert.That(cellA1.Text, Is.EqualTo(initialText));
    }

    /// <summary>
    /// Tests the exposed Undo function, which uses commands to apply an undo, on a background color change.
    /// </summary>
    [Test]
    public void UndoBackgroundColorChangeTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        var initialBackgroundColor = cellA1.BackgroundColor;
        var secondaryBackgroundColor = Colors.Aqua.ToUInt32();
        viewModel.SelectCell(0, 0);
        viewModel.ChangeSelectedCellColor(secondaryBackgroundColor);

        // Act
        viewModel.UndoCommand();

        // Assert
        Assert.That(cellA1.BackgroundColor, Is.EqualTo(initialBackgroundColor));
    }

    /// <summary>
    /// Tests the exposed Redo function, which uses commands to apply an redo, on a text edit.
    /// </summary>
    [Test]
    public void RedoTextEditTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        var secondaryText = "blah blah";
        viewModel.EditCellText(0, 0, secondaryText);
        viewModel.UndoCommand();

        // Act
        viewModel.RedoCommand();

        // Assert
        Assert.That(cellA1.Text, Is.EqualTo(secondaryText));
    }

    /// <summary>
    /// Tests the exposed Redo function, which uses commands to apply an redo, on a background color change.
    /// </summary>
    [Test]
    public void RedoBackgroundColorChangeTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");
        var cellA1 = viewModel.GetCell(0, 0);
        var secondaryBackgroundColor = Colors.Aqua.ToUInt32();
        viewModel.SelectCell(0, 0);
        viewModel.ChangeSelectedCellColor(secondaryBackgroundColor);
        viewModel.UndoCommand();

        // Act
        viewModel.RedoCommand();

        // Assert
        Assert.That(cellA1.BackgroundColor, Is.EqualTo(secondaryBackgroundColor));
    }

    /// <summary>
    /// Tests the exposed Undo function, which uses commands to apply an undo, on a text edit in the EXCEPTIONAL CASE where no undo commands exist.
    /// </summary>
    [Test]
    public void UndoTextEditWhenNoUndoExceptionalTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");

        // Act and Assert
        Assert.Multiple(() =>
            {
                // The viewmodel should know that the undo stack is empty, and thus the option to undo will not be available
                Assert.That(viewModel.UndoAvailable, Is.False);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    // The undo stack is empty so the stack pop will result in InvalidOperationException
                    viewModel.UndoCommand();
                });
            }
        );
    }

    /// <summary>
    /// Tests the exposed Undo function, which uses commands to apply an undo, on a background color change in the EXCEPTIONAL CASE where no undo commands exist.
    /// </summary>
    [Test]
    public void UndoBackgroundColorChangeWhenNoUndoExceptionalTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");

        // Act and Assert
        Assert.Multiple(() =>
            {
                // The viewmodel should know that the undo stack is empty, and thus the option to undo will not be available
                Assert.That(viewModel.UndoAvailable, Is.False);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    // The undo stack is empty so the stack pop will result in InvalidOperationException
                    viewModel.UndoCommand();
                });
            }
        );
    }

    /// <summary>
    /// Tests the exposed Redo function, which uses commands to apply an redo, on a text edit in the EXCEPTIONAL CASE where no redo commands exist.
    /// </summary>
    [Test]
    public void RedoTextEditWhenNoRedoExceptionalTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");

        // Act and Assert
        Assert.Multiple(() =>
            {
                // The viewmodel should know that the redo stack is empty, and thus the option to redo will not be available
                Assert.That(viewModel.RedoAvailable, Is.False);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    // The redo stack is empty so the stack pop will result in InvalidOperationException
                    viewModel.RedoCommand();
                });
            }
        );
    }

    /// <summary>
    /// Tests the exposed Undo function, which uses commands to apply an undo, on a background color change.
    /// </summary>
    [Test]
    public void RedoBackgroundColorChangeWhenNoRedoExceptionalTest()
    {
        // Arrange
        Debug.Assert(viewModel != null, nameof(viewModel) + " != null");

        // Act and Assert
        Assert.Multiple(() =>
            {
                // The viewmodel should know that the stack is empty, and thus the option to redo will not be available
                Assert.That(viewModel.RedoAvailable, Is.False);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    // The redo stack is empty so the stack pop will result in InvalidOperationException
                    viewModel.RedoCommand();
                });
            }
        );
    }
}