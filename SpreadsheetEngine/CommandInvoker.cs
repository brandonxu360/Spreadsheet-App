// <copyright file="CommandInvoker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Invoker class for the spreadsheet command pattern.
/// </summary>
public class CommandInvoker
{
    /// <summary>
    /// The stack of commands to perform undo operations with.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Stack<ICommand> undoStack;

    /// <summary>
    ///  The stack of commands to perform redo operations with.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Stack<ICommand> redoStack;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInvoker"/> class.
    /// </summary>
    public CommandInvoker()
    {
        this.redoStack = new Stack<ICommand>();
        this.undoStack = new Stack<ICommand>();
    }

    /// <summary>
    /// Tries to get the top command of the undo stack, returns null if unsuccessful.
    /// </summary>
    /// <returns>The ICommand command at the top of undo stack, or null.</returns>
    public ICommand? TryGetUndoCommand()
    {
        try
        {
            return this.undoStack.Peek();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to get the top command of the redo stack, returns null if unsuccessful.
    /// </summary>
    /// <returns>The ICommand command at the top of redo stack, or null.</returns>
    public ICommand? TryGetRedoCommand()
    {
        try
        {
            return this.redoStack.Peek();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Uses the EditTextCommand to edit the cell text.
    /// </summary>
    /// <param name="cell">The cell that should be edited.</param>
    /// <param name="newText">The new text to set the text property to.</param>
    public void EditCellText(Cell cell, string newText)
    {
        // Create the command and execute it
        var command = new EditTextCommand(cell, newText);
        command.Execute();

        // Add the command to the undo stack
        this.undoStack.Push(command);

        // Clear the redo stack since a new command was executed
        this.redoStack.Clear();
    }

    /// <summary>
    /// Uses the ChangeColorCommand to change the cell background color of all provided cells.
    /// </summary>
    /// <param name="cells">The cells that should be changed.</param>
    /// <param name="newColor">The new color to set the text background color to.</param>
    public void ChangeCellColor(List<Cell> cells, uint newColor)
    {
        // Create the command and execute it
        var command = new ChangeColorCommand(cells, newColor);
        command.Execute();

        // Add the command to the undo stack
        this.undoStack.Push(command);

        // Clear the redo stack since a new command was executed
        this.redoStack.Clear();
    }

    /// <summary>
    /// Initiates the undo functionality - undoes the last command that was executed by popping the undo stack.
    /// </summary>
    public void Undo()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initiates the redo functionality - redoes the last undo command.
    /// </summary>
    public void Redo()
    {
        throw new NotImplementedException();
    }
}