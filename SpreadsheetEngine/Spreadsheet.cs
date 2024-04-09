// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.ComponentModel;
using System.Globalization;
using System.Xml;

/// <summary>
/// The spreadsheet class that will serve as a container for a 2D array of cells. It will also serve
/// as a factory for spreadsheet cells.
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// Instance of command invoker to execute the set commands with.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly CommandInvoker commandInvoker;

    /// <summary>
    /// The 2D array of cells to represent the cells of the spreadsheet.
    /// </summary>
    // ReSharper disable once InconsistentNaming (conflicts with stylecop)
    private Cell?[,]? cells;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// </summary>
    /// <param name="rowCount">The number of rows in the spreadsheet.</param>
    /// <param name="columnCount">The number of columns in the spreadsheet.</param>
    public Spreadsheet(int rowCount, int columnCount)
    {
        this.RowCount = rowCount;
        this.ColumnCount = columnCount;

        // Instantiate the invoker
        this.commandInvoker = new CommandInvoker();

        this.InitializeEmptyCellMatrix(rowCount, columnCount);
    }

    /// <summary>
    /// Gets the number of columns in the spreadsheet.
    /// </summary>
    private int ColumnCount { get; }

    /// <summary>
    /// Gets the number of rows in the spreadsheet.
    /// </summary>
    private int RowCount { get; }

    /// <summary>
    /// Expose the TryGetUndoCommand from the CommandInvoker.
    /// </summary>
    /// <returns>The top ICommand command on the undo stack, null if unsuccessful.</returns>
    public ICommand? TryGetUndoCommand()
    {
        return this.commandInvoker.TryGetUndoCommand();
    }

    /// <summary>
    /// Expose the TryGetRedoCommand from the CommandInvoker.
    /// </summary>
    /// <returns>The top ICommand on the redo stack, null if unsuccessful.</returns>
    public ICommand? TryGetRedoCommand()
    {
        return this.commandInvoker.TryGetRedoCommand();
    }

    /// <summary>
    /// Performs undo operation on the spreadsheet.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    public void Undo()
    {
        this.commandInvoker.Undo();
    }

    /// <summary>
    /// Performs redo operation on the spreadsheet.
    /// </summary>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    public void Redo()
    {
        this.commandInvoker.Redo();
    }

    /// <summary>
    /// Gets the cell that will have its text edited and calls the commandInvoker to edit the cell text using a command.
    /// </summary>
    /// <param name="rowIndex">The row index corresponding to the cell location.</param>
    /// <param name="columnIndex">The column index corresponding to the cell location.</param>
    /// <param name="newText">The new string text to set the cell text to.</param>
    public void EditCellText(int rowIndex, int columnIndex, string newText)
    {
        var cell = this.GetCell(rowIndex, columnIndex);

        // Call the command invoker to edit the cell text
        this.commandInvoker.EditCellText(cell, newText);
    }

    /// <summary>
    /// Calls the command invoker to use a command to change the cell background color of the selected cells.
    /// </summary>
    /// <param name="selectedCells">The list of cells to change the background color of (selected cells).</param>
    /// <param name="newColor">The new uint color to set the backgrounds colors to.</param>
    public void ChangeCellColor(List<Cell> selectedCells, uint newColor)
    {
        // Call the command invoker to change the cell background color
        this.commandInvoker.ChangeCellColor(selectedCells, newColor);
    }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <param name="columnIndex">The column index of the cell.</param>
    /// <returns>The Cell object at the column and cell index.</returns>
    public Cell GetCell(int rowIndex, int columnIndex)
    {
        return this.cells?[rowIndex, columnIndex] ?? throw new IndexOutOfRangeException();
    }

    /// <summary>
    /// Saves an XML file to the stream using XMLWriter.
    /// </summary>
    /// <param name="stream">The stream to write the XML to.</param>
    public void SaveToStream(Stream stream)
    {
        // Create an XML writer using the stream
        using var writer = XmlWriter.Create(stream);

        // Start writing the root element
        writer.WriteStartElement("spreadsheet");

        // Iterate through each cell in the spreadsheet
        for (var i = 0; i < this.RowCount; i++)
        {
            for (var j = 0; j < this.ColumnCount; j++)
            {
                var cell = this.cells?[i, j];

                // Check if the cell has changed properties
                if (cell is not { HasChanged: true })
                {
                    continue;
                }

                // Write the cell element
                writer.WriteStartElement("cell");
                writer.WriteAttributeString("name", cell.GetName());

                // Write the bgcolor element
                writer.WriteStartElement("bgcolor");
                writer.WriteString(cell.BackgroundColor.ToString("X"));
                writer.WriteEndElement();

                // Write the text element
                writer.WriteStartElement("text");
                writer.WriteString(cell.Text);
                writer.WriteEndElement();

                writer.WriteEndElement(); // Close cell element
            }
        }
    }

    /// <summary>
    /// Loads an XML file from the stream using XMLReader.
    /// </summary>
    /// <param name="stream">The stream to read the XML from.</param>
    public void LoadFromStream(Stream stream)
    {
        // Clear existing spreadsheet data by reinitializing the cell matrix
        this.InitializeEmptyCellMatrix(this.RowCount, this.ColumnCount);

        // Create an XML reader using the stream
        using var reader = XmlReader.Create(stream);

        // Iterate through the Xml document, looking for cells
        while (reader.Read())
        {
            // If element is not a cell, skip it
            if (reader.NodeType != XmlNodeType.Element || reader.Name != "cell")
            {
                continue;
            }

            // Read cell attributes
            var cellName = reader.GetAttribute("name");
            string? bgColorHex = null;
            string? text = null;

            // Move to the first child node of the "cell" element
            reader.ReadStartElement();

            // Read all child elements of the "cell" element
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                // Check that the child element is the correct node type
                if (reader.NodeType == XmlNodeType.Element)
                {
                    // Set the cell attributes depending on the name of the node
                    switch (reader.Name)
                    {
                        case "bgcolor":
                            bgColorHex = reader.ReadElementContentAsString();
                            break;
                        case "text":
                            text = reader.ReadElementContentAsString();
                            break;
                        default:
                            // Skip any unexpected tags
                            reader.Skip();
                            break;
                    }
                }
                else
                {
                    reader.Read(); // Move to the next node
                }
            }

            // Create and populate the cell
            if (cellName == null)
            {
                continue;
            }

            var cell = this.GetCell(cellName);
            if (bgColorHex != null)
            {
                var bgColor = uint.Parse(bgColorHex, NumberStyles.HexNumber);
                cell.BackgroundColor = bgColor;
            }

            if (text != null)
            {
                cell.Text = text;
            }
        }
    }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="cellName">The cell name/reference (ie. "A1").</param>
    /// <returns>The Cell object at the specified location.</returns>
    public Cell GetCell(string cellName)
    {
        // Parse the cell name to extract row and column indices
        var columnIndex = cellName[0] - 'A'; // Convert the column letter to a zero-based index
        var rowIndex = int.Parse(cellName[1..]) - 1; // Parse the row number and convert to zero-based index

        // Check if the indices are within the bounds of the spreadsheet
        if (rowIndex < 0 || rowIndex >= this.RowCount || columnIndex < 0 || columnIndex >= this.ColumnCount)
        {
            throw new ArgumentException("Cell name is out of range.");
        }

        // Retrieve the cell object from the 2D array and return it
        return this.GetCell(rowIndex, columnIndex);
    }

    private void InitializeEmptyCellMatrix(int rowCount, int columnCount)
    {
        // Initialize the 2D array of cells according to the provided dimensions
        this.cells = new Cell[rowCount, columnCount];

        // Create a spreadsheet cell and assign it to each position in the cell array
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < columnCount; j++)
            {
                this.cells[i, j] = new SpreadsheetCell(i, j);

                // Make sure the cell was successfully created and nonnull
                if (this.cells[i, j] != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    this.cells[i, j].PropertyChanged += this.OnCellPropertyChanged;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }
    }

    /// <summary>
    /// Executed when a cell property is changed, will call the cell Value setter which has
    /// its own implementation of setting the cell value.
    /// </summary>
    /// <param name="sender">Cell that had its property changed.</param>
    /// <param name="e">PropertyChanged event arguments.</param>
    private void OnCellPropertyChanged(object? sender, PropertyChangedEventArgs? e)
    {
        if (sender is Cell cell && e?.PropertyName == nameof(Cell.Text))
        {
            // Expression
            if (cell.Text.StartsWith('=') && cell.Text.Length > 1)
            {
                cell.Value = this.EvaluateExpression(cell.Text, cell);
            }

            // Plaintext
            else
            {
                cell.Value = cell.Text;
            }
        }
    }

    /// <summary>
    /// Attempts to build and evaluate the value of the expression (indicated with an = sign), and handles subscribing and unsubscribing referencing cells from referenced
    /// cell events.
    /// </summary>
    /// <param name="expression">The string expression input taken directly from the cell text (includes the =).</param>
    /// <param name="sender">The cell that had its property changed.</param>
    /// <returns>The string representation for the evaluation of the expression.</returns>
    /// <exception cref="Exception">Any errors encountered during the building/evaluating of the expression will return "#Invalid expression" code.</exception>
    private string EvaluateExpression(string expression, Cell sender)
    {
        // Extract the cell expression from the text (e.g., "=A5" -> "A5")
        var strippedExpression = expression[1..]; // Remove the '='

        // Unsubscribe the cell from all cells previously subscribed to (will resubscribe depending on new expression references)
        foreach (var reference in sender.ReferencedCellNames)
        {
            this.GetCell(reference).PropertyChanged -= sender.OnReferencedCellPropertyChanged;
            sender.ReferencedCellNames.Remove(reference);
        }

        // Try to build and evaluate an expression tree
        try
        {
            // Build the expression tree using ExpressionTree
            var expressionTree = new ExpressionTree.ExpressionTree(strippedExpression);

            // Get a list of all the references/variables found when building the ExpressionTree (ExpressionTree's variable dictionary)
            var references = expressionTree.GetVariableNames();

            // If simple single reference to cell, subscribe to cell and return the value of the referenced cell (or throw ref exception if string is empty)
            if (references.Count > 0 && strippedExpression == references.First())
            {
                this.SubscribeToReferencedCells(sender, references.First());
                var referencedValue = this.GetCell(references.First()).Value;
                return referencedValue != string.Empty ? referencedValue : "#InvalidRef";
            }

            // Otherwise, iterate through each reference to subscribe to the cell, set the variable value, and return the final evaluated value
            foreach (var reference in references)
            {
                // Subscribe to the references (variables) found when building the tree
                this.SubscribeToReferencedCells(sender, reference);

                // Get the value from the referenced cell
                var stringValue = this.GetCell(reference).Value;

                // If the cell value is valid (double), set variable accordingly
                if (double.TryParse(stringValue, out var doubleValue))
                {
                    expressionTree.SetVariable(reference, doubleValue);
                }
                else
                {
                    throw new Exception("#InvalidRef");
                }
            }

            // Finally, call evaluate on the ExpressionTree and return the value as a string
            return expressionTree.Evaluate().ToString(CultureInfo.InvariantCulture);
        }

        // Return error for invalid references whenever expression fails to build/evaluate
        catch (Exception)
        {
            return "#InvalidRef";
        }
    }

    /// <summary>
    /// Subscribe referencing cell to referenced cell event (if not already subscribed).
    /// </summary>
    /// <param name="sender">The referencing cell (subscriber).</param>
    /// <param name="reference">The referenced cell (notifier).</param>
    private void SubscribeToReferencedCells(Cell sender, string reference)
    {
        // If the referenced cell is already subscribed to (as indicated in the cell's ReferencedCellNames property), do not subscribe again
        if (sender.ReferencedCellNames.Contains(reference))
        {
            return;
        }

        this.GetCell(reference).PropertyChanged += sender.OnReferencedCellPropertyChanged;
        sender.ReferencedCellNames.Add(reference);
    }

    /// <summary>
    /// Embedded SpreadsheetCell to expose the setter privileges only to the Spreadsheet class.
    /// </summary>
    private class SpreadsheetCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index of the cell.</param>
        /// <param name="columnIndex">The column index of the cell.</param>
        public SpreadsheetCell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }

        /// <summary>
        /// Overridden setter for the value of a cell.
        /// </summary>
        /// <param name="newValue">The new string to set the value to.</param>
        protected override void SetValue(string newValue)
        {
            this.value = newValue;
        }
    }
}