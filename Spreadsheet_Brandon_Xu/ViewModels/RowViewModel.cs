// <copyright file="RowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Brandon_Xu.ViewModels;

using System.Collections.Generic;
using System.ComponentModel;
using ReactiveUI;

/// <summary>
/// The row viewmodel to wrap the rows and prepare the row data for the UI.
/// </summary>
public class RowViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RowViewModel"/> class.
    /// </summary>
    /// <param name="cells">The list of CellViewModels that represent a row of CellViewModels.</param>
    public RowViewModel(List<CellViewModel> cells)
    {
        this.Cells = cells;

        // Subscribe the row to changes in any of the cells it contains
        foreach (var cell in this.Cells)
        {
            cell.PropertyChanged += this.CellOnPropertyChanged;
        }

        this.SelfReference = this;
    }

    /// <summary>
    /// Gets or sets the list of CellViewModels that make up the row that the RowViewModel represents.
    /// </summary>
    public List<CellViewModel> Cells { get; set; }

    /// <summary>
    /// Gets the property that provides a way to notify the value converter that it needs to update.
    /// </summary>
    public RowViewModel SelfReference { get; private set; }

    /// <summary>
    /// Gets the CellViewModel from the RowViewModel given an index.
    /// </summary>
    /// <param name="index">The index of the cell.</param>
    public CellViewModel this[int index] => this.Cells[index];

    /// <summary>
    /// Method called when a style property is changed in a CellViewModel contained in this RowViewModel.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The event arguments.</param>
    private void CellOnPropertyChanged(object? sender, PropertyChangedEventArgs
        args)
    {
        var styleImpactingProperties = new List<string>
        {
            nameof(CellViewModel.IsSelected),
            nameof(CellViewModel.CanEdit),
            nameof(CellViewModel.BackgroundColor),
        };
        if (args.PropertyName != null && styleImpactingProperties.Contains(args.PropertyName))
        {
            this.FireChangedEvent();
        }
    }

    /// <summary>
    /// Fire the event to announce that a property has changed.
    /// </summary>
    private void FireChangedEvent()
    {
        this.RaisePropertyChanged(nameof(this.SelfReference));
    }
}