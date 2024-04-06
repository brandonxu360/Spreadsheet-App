// <copyright file="ICommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Command interface for spreadsheet related commands.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Gets the title for the command - used for displaying which command is available in the UI.
    /// </summary>
    /// <returns>The string title of the command.</returns>
    public string GetCommandTitle();

    /// <summary>
    /// Executes the command.
    /// </summary>
    public void Execute();

    /// <summary>
    /// Undoes the command (the opposite effect of execute).
    /// </summary>
    public void Undo();
}