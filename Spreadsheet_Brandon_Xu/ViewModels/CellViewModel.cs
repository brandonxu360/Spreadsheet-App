// <copyright file="CellViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using ReactiveUI;
using SpreadsheetEngine;

/// <summary>
/// The cell viewmodel to wrap each cell and prepare its data for the UI.
/// </summary>
public sealed class CellViewModel : ViewModelBase
{
    /// <summary>
    /// The cell domain object that is wrapped by this cell viewmodel.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Cell cell;

    /// <summary>
    /// The field that indicates if a cell can be edited.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private bool canEdit;

    /// <summary>
    /// Field that indicates if a cell is selected.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private bool isSelected;

    /// <summary>
    /// Initializes a new instance of the <see cref="CellViewModel"/> class.
    /// </summary>
    /// <param name="cell">The cell domain object to be wrapped by this cell viewmodel.</param>
    public CellViewModel(Cell cell)
    {
        // Initialize the isSelected and canEdit fields to false
        this.cell = cell;
        this.isSelected = false;
        this.canEdit = false;

        // We forward the notifications from the cell model to the view model
        // note that we forward using args.PropertyName because Cell and CellViewModel properties have the same
        // names to simplify the procedure. If they had different names, we would have a more complex implementation to
        // do the property names matching
        this.cell.PropertyChanged += (_, args) => { this.RaisePropertyChanged(args.PropertyName); };
    }

    /// <summary>
    /// Gets the cell object wrapped by this viewmodel.
    /// </summary>
    public Cell Cell => this.cell;

    /// <summary>
    /// Gets or sets a value indicating whether the cell is selected.
    /// </summary>
    public bool IsSelected
    {
        get => this.isSelected;
        set => this.RaiseAndSetIfChanged(ref this.isSelected, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the value indicating whether a cell can be edited.
    /// </summary>
    public bool CanEdit
    {
        get => this.canEdit;
        set => this.RaiseAndSetIfChanged(ref this.canEdit, value);
    }

    /// <summary>
    /// Gets or sets the text property of the Cell object that the viewmodel contains.
    /// </summary>
    public string? Text
    {
        get => this.cell.Text;
        set
        {
            if (value != null)
            {
                this.cell.Text = value;
            }
        }
    }

    /// <summary>
    /// Gets the value property of the Cell object that the viewmodel contains.
    /// </summary>
    public string Value => this.cell.Value;

    /// <summary>
    /// Gets or sets the background color property of the Cell object that the viewmodel contains.
    /// </summary>
    public uint BackgroundColor
    {
        get => this.cell.BackgroundColor;
        set => this.cell.BackgroundColor = value;
    }
}