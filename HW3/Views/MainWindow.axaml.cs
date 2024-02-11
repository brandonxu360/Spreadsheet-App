// <copyright file="MainWindow.axaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3.Views;

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using HW3.ViewModels;
using ReactiveUI;

/// <summary>
/// This class handles all necessary UI events to communicate with the view model and sub windows.
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.DataContext = new MainWindowViewModel();

        this.WhenActivated(d =>
        {
            var mainWindowViewModel = this.ViewModel;
            if (mainWindowViewModel != null)
            {
                d(mainWindowViewModel.AskForFileToLoad.RegisterHandler(this.DoOpenFile));
            }
        });

        // TODO: add code for saving
    }

    /// <summary>
    /// Opens a dialog to select a file which will be used to load content.
    /// </summary>
    /// <param name="interaction">Defines the Input/Output necessary for this workflow to complete successfully.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task DoOpenFile(InteractionContext<Unit, string?> interaction)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);
        Console.Write(topLevel);

        // List of filtered types
        var fileTypes = new List<FilePickerFileType> { FilePickerFileTypes.TextPlain };

        if (topLevel != null)
        {
            // Start async operation to open the dialog.
            var filePath = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
                FileTypeFilter = fileTypes,
            });

            interaction.SetOutput(filePath.Count == 1 ? filePath[0].Path.AbsolutePath : null);
        }
    }

    /// <summary>
    /// Opens a dialog to select a file in which content will be saved.
    /// </summary>
    /// <param name="interaction">Defines the Input/Output necessary for this workflow to complete successfully.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task DoSaveFile(InteractionContext<Unit, string?> interaction)
    {
        // TODO: your code goes here.
    }
}
