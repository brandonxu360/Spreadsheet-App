// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW3.ViewModels;

using System.IO;
using System.Reactive;
using System.Reactive.Linq;

// ReSharper disable once RedundantNameQualifier
using HW3.Models;
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
        this.AskForFileToSave = new Interaction<Unit, string?>();
    }

    /// <summary>
    /// Gets or sets fibonacciNumber attribute value
    /// TODO: You need to bind this property in the .axaml file.
    /// </summary>
    public string FibonacciNumbers
    {
        get => this.fibonacciNumbers;
        set => this.RaiseAndSetIfChanged(ref this.fibonacciNumbers, value);
    }

    /// <summary>
    /// Gets prompts the view to allow the user to select a file for loading by triggering the DoOpenFile method in the MainWindow.
    /// </summary>
    public Interaction<Unit, string?> AskForFileToLoad { get; }

    /// <summary>
    /// Gets prompts the view to allow the user to select a file for loading by triggering the DoOpenFile method in the MainWindow.
    /// </summary>
    public Interaction<Unit, string?> AskForFileToSave { get; }

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

    /// <summary>
    /// This method will be executed when the user wants to load content from a file.
    /// </summary>
    public async void SaveToFile()
    {
        // Wait for user to select file to save to.
        var filePath = await this.AskForFileToSave.Handle(default);
        if (filePath == null)
        {
            return;
        }

        // If the user has selected a file, create the stream read
        // Open writing stream from the file.
        await using var streamWriter = new StreamWriter(filePath);

        // Write the contents of FibonacciNumbers to the file
        await streamWriter.WriteLineAsync(this.FibonacciNumbers);
    }

    /// <summary>
    /// Replaces the content of FibonacciNumbers with the first 50 numbers in the fibonacci sequence using FibonacciTextReader.
    /// </summary>
    public void LoadFirst50Fibonacci()
    {
        this.FibonacciNumbers = new FibonacciTextReader(50).ReadToEnd();
    }

    /// <summary>
    /// Replaces the content of FibonacciNumbers with the first 100 numbers in the fibonacci sequence using FibonacciTextReader.
    /// </summary>
    public void LoadFirst100Fibonacci()
    {
        this.FibonacciNumbers = new FibonacciTextReader(100).ReadToEnd();
    }

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