// <copyright file="MainWindow.axaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.Views;

using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;

// ReSharper disable once RedundantNameQualifier
using Spreadsheet_Brandon_Xu.ViewModels;

/// <summary>
/// Partial class representing the main window of the application.
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.WhenAnyValue(x => x.DataContext)
            .Where(dataContext => dataContext != null)
            .Subscribe(dataContext =>
            {
                if (dataContext is MainWindowViewModel)
                {
                    this.InitializeDataGrid();
                }
            });
    }

    /// <summary>
    /// Initializes the DataGrid with columns A-Z.
    /// </summary>
    private void InitializeDataGrid()
    {
        var columnCount = 'Z' - 'A' + 1;

        // Add columns from A to Z
        foreach (var columnIndex in Enumerable.Range(0, columnCount))
        {
            var columnHeader = (char)('A' + columnIndex);
            var column = new DataGridTemplateColumn()
            {
                Header = columnHeader,
                CellStyleClasses = { "SpreadsheetCellClass" },
                CellTemplate = new FuncDataTemplate<RowViewModel>((_, _) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding($"[{columnIndex}].Value"),
                        TextAlignment = TextAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = Thickness.Parse("5,0,5,0"),
                    }),
                CellEditingTemplate = new FuncDataTemplate<RowViewModel>((_, _) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding($"[{columnIndex}].Text"),
                    }),
            };
            this.SpreadsheetDataGrid.Columns.Add(column);
        }

        // Add function to DataGrid cell editing event when a cell enters editing mode
        this.SpreadsheetDataGrid.PreparingCellForEdit += (_, args) =>
        {
            // Catch exceptional case where the EditingElement is not a TextBox
            if (args.EditingElement is not TextBox textInput)
            {
                // Ignore and quit execution of the function
                return;
            }

            // Get the row and column indices of the DataGrid cell that entered editing mode
            var rowIndex = args.Row.GetIndex();
            var columnIndex = args.Column.DisplayIndex;

            // Set the EditingElement TextBox text to the cell's text at the given location
            textInput.Text = (this.DataContext as MainWindowViewModel)?.GetCell(rowIndex, columnIndex).Text;
        };

        // Add function to DataGrid cell event when a cell finishes editing
        this.SpreadsheetDataGrid.CellEditEnding += (_, args) =>
        {
            // Catch exceptional case where the EditingElement is not a TextBox
            if (args.EditingElement is not TextBox textInput)
            {
                return;
            }

            // Get the row and column indices of the DataGrid cell that was edited
            var rowIndex = args.Row.GetIndex();
            var columnIndex = args.Column.DisplayIndex;

            // Set the text of the cell to the text of the EditingElement TextBox text
            if (this.DataContext is MainWindowViewModel model)
            {
                model.GetCell(rowIndex, columnIndex).Text = textInput.Text;
            }
        };

        // Add function with custom selection logic for when a DataGrid Cell is mouse-pressed
        this.SpreadsheetDataGrid.CellPointerPressed += (_, args) =>
        {
            // Get the indices of the cell that was pressed
            var rowIndex = args.Row.GetIndex();
            var columnIndex = args.Column.DisplayIndex;

            // Check if multiple cells were selected
            var multipleSelection =
                args.PointerPressedEventArgs.KeyModifiers != KeyModifiers.None;
            if (multipleSelection == false)
            {
                (this.DataContext as MainWindowViewModel)?.SelectCell(rowIndex, columnIndex);
            }
            else
            {
                (this.DataContext as MainWindowViewModel)?.ToggleCellSelection(rowIndex, columnIndex);
            }
        };

        // Add function to DataGrid cell event when a cell enters editing mode
        this.SpreadsheetDataGrid.BeginningEdit += (_, args) =>
        {
            // Get the indices of the pressed DataGrid cell
            var rowIndex = args.Row.GetIndex();
            var columnIndex = args.Column.DisplayIndex;

            // Get the cell object that corresponds to the indices of the pressed DataGrid cell
            var cell = (this.DataContext as MainWindowViewModel)?.GetCell(rowIndex, columnIndex);

            // If the cell that was pressed is editable, reset the selection of the selected cells
            if (cell is { CanEdit: false })
            {
                args.Cancel = true;
            }
            else
            {
                (this.DataContext as MainWindowViewModel)?.ResetSelection();
            }
        };
    }

    // Set header template to be the row index + 1
    // ReSharper disable once UnusedParameter.Local
    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs dataGridRowEventArgs)
    {
        dataGridRowEventArgs.Row.Header = dataGridRowEventArgs.Row.GetIndex() + 1;
    }
}