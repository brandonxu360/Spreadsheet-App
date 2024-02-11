// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3.ViewModels;

using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

/// <summary>
/// The MainWindowViewModel.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    // ReSharper disable once InconsistentNaming
    private string fibonacciNumbers = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
        // Create an interaction between the view model and the view for the file to be loaded:
        this.AskForFileToLoad = new Interaction<Unit, string?>();

        // Similarly to load, there is a need to create an interaction for saving into a file:
        // TODO: Your code goes here.
    }

    /// <summary>
    /// Gets and sets fibonacciNumber attribute value
    /// TODO: You need to bind this property in the .axaml file.
    /// </summary>
    public string FibonacciNumbers
    {
        get => this.fibonacciNumbers;
        private set => this.RaiseAndSetIfChanged(ref this.fibonacciNumbers, value);
    }

    /// <summary>
    /// Gets prompts the view to allow the user to select a file for loading by triggering the DoOpenFile method in the MainWindow.
    /// </summary>
    public Interaction<Unit, string?> AskForFileToLoad { get; }

    /// <summary>
    /// This method will be executed when the user wants to load content from a file.
    /// </summary>
    public async void LoadFromFile()
    {
        // Wait for the user to select the file to load from.
        var filePath = await this.AskForFileToLoad.Handle(default);
        if (filePath == null)
        {
            return;
        }

        // If the user selected a file, create the stream reader and load the text.
        var textReader = new StreamReader(filePath);
        this.LoadText(textReader);
        textReader.Close();
    }

    public async void SaveToFile()
    {
        // TODO: Implement this method.
    }

    // other code...

    /// <summary>
    /// Reads all the characters from a file using the input TextReader and replaces the existing content in the _fibonacci field.
    /// </summary>
    /// <param name="sr">The input TextReader for the file text contents.</param>
    private void LoadText(TextReader sr)
    {
        // Replace
        this.FibonacciNumbers = sr.ReadToEnd();
    }
}