// <copyright file="MainWindow.axaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;

// ReSharper disable once RedundantNameQualifier
using Spreadsheet_Brandon_Xu.ViewModels;
using SpreadsheetEngine;

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
            var column = new DataGridTemplateColumn()
            {
                Header = (char)('A' + columnIndex),
                CellTemplate = new FuncDataTemplate<IEnumerable<Cell>>((_, _) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding($"[{columnIndex}].Value"),
                        TextAlignment = TextAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = Thickness.Parse("5,0,5,0"),
                    }),
                CellEditingTemplate = new FuncDataTemplate<IEnumerable<Cell>>((_, _) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding($"[{columnIndex}].Text"),
                    }),
            };
            this.SpreadsheetDataGrid.Columns.Add(column);
        }
    }

    // Set header template to be the row index + 1
    // ReSharper disable once UnusedParameter.Local
    private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs dataGridRowEventArgs)
    {
        dataGridRowEventArgs.Row.Header = dataGridRowEventArgs.Row.GetIndex() + 1;
    }
}