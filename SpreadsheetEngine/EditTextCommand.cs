// <copyright file="EditTextCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Concrete implementation of ICommand for editing the color of a cell.
/// </summary>
public class EditTextCommand : ICommand
{
    /// <summary>
    /// The cell to execute the edit text command on.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Cell cell;

    /// <summary>
    /// The new version of the cell text.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly string newText;

    /// <summary>
    /// The old version of the cell text, used to enable undo functionality.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly string oldText;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditTextCommand"/> class.
    /// </summary>
    /// <param name="cell">The cell to execute the edit command on.</param>
    /// <param name="newText">The new text to set the cell text to.</param>
    public EditTextCommand(Cell cell, string newText)
    {
        this.cell = cell;
        this.newText = newText;
        this.oldText = cell.Text;
    }

    /// <inheritdoc/>
    public string GetCommandTitle()
    {
        return "text edit";
    }

    /// <summary>
    /// Set the text of the cell to a new text value.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
    public void Execute()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the text of the cell to its previous text value.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
    public void Undo()
    {
        throw new NotImplementedException();
    }
}