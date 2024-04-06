// <copyright file="ChangeColorCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Concrete implementation of ICommand for changing the background color of cells.
/// </summary>
public class ChangeColorCommand : ICommand
{
    /// <summary>
    /// The cells to execute the change color command on.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly List<Cell> cells;

    /// <summary>
    /// The new cell background color.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly uint newColor;

    /// <summary>
    /// The old cell background colors, used to enable undo functionality.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Dictionary<Cell, uint>? oldColors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeColorCommand"/> class.
    /// </summary>
    /// <param name="cells">The cells to execute the change color command on.</param>
    /// <param name="newColor">The new color to set the cell background color to.</param>
    public ChangeColorCommand(List<Cell> cells, uint newColor)
    {
        this.cells = cells;
        this.newColor = newColor;

        this.oldColors = new Dictionary<Cell, uint>();
        foreach (var cell in cells)
        {
            this.oldColors?.Add(cell, cell.BackgroundColor);
        }
    }

    /// <inheritdoc/>
    public string GetCommandTitle()
    {
        return "color change";
    }

    /// <summary>
    /// Set the background color of all provided cells to a new UInt value.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
    public void Execute()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the background color of the cells to their previous UInt value.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
    public void Undo()
    {
        throw new NotImplementedException();
    }
}