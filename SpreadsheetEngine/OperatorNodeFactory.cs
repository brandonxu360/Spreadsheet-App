// <copyright file="OperatorNodeFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.Reflection;

/// <summary>
/// The factory class for instantiating specific operator nodes based on the string passed in.
/// Also provides the precedence for the operators.
/// </summary>
public class OperatorNodeFactory
{
    /// <summary>
    /// Dictionary mapping operator symbols to OperatorNode types.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    // Disable private suggestion - dictionary is accessed by the ExpressionTree class
    #pragma warning disable SA1401
    public readonly Dictionary<string, Type> OperatorNodeTypes;
    #pragma warning restore SA1401

    /// <summary>
    /// Define operator precedence for each operator type.
    /// </summary>
    // Disable private suggestion = dictionary is accessed by the ExpressionTree class
    #pragma warning disable SA1401
    public readonly Dictionary<string, int> OperatorNodePrecedences;
    #pragma warning restore SA1401

    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
    /// </summary>
    public OperatorNodeFactory()
    {
        // Initialize operator type and precedence dictionaries
        this.OperatorNodeTypes = new Dictionary<string, Type>();
        this.OperatorNodePrecedences = new Dictionary<string, int>();

        // Dynamically populate the type and precedence dictionaries
        this.TraverseAvailableOperators((op, type, precedence) =>
        {
            this.OperatorNodeTypes.Add(op, type);
            this.OperatorNodePrecedences.Add(op, precedence);
        });
    }

    /// <summary>
    /// Delegate for the function to be executed when an operator is found in TraverseAvailableOperators.
    /// </summary>
    private delegate void OnOperator(string op, Type type, int precedence);

    /// <summary>
    /// Factory method for instantiating instances of OperatorNode.
    /// </summary>
    /// <param name="symbol">The string symbol for the specific OperatorNode type.</param>
    /// <returns>A specific OperatorNode instance.</returns>
    public OperatorNode? CreateOperatorNode(string symbol)
    {
        if (this.OperatorNodeTypes.TryGetValue(symbol, out var operatorNodeType))
        {
            return (OperatorNode)Activator.CreateInstance(operatorNodeType)!;
        }

        // Return null if the operator was not found in the operator dictionary
        return null;
    }

    private void TraverseAvailableOperators(OnOperator onOperator)
    {
        // Get the type declaration of OperatorNode
        Type operatorNodeType = typeof(OperatorNode);

        // Iterate over all loaded assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Get all the types that inherit from our OperatorNode class using LINQ
            IEnumerable<Type> operatorTypes = assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

            // Iterate over those subclasses of OperatorNode
            foreach (var type in operatorTypes)
            {
                // For each subclass, retrieve the OperatorSymbol and Precedence fields
                FieldInfo? operatorSymbolField = type.GetField("OperatorSymbol");
                FieldInfo? precedenceField = type.GetField("Precedence");

                if (operatorSymbolField != null && precedenceField != null)
                {
                    // Get the character of the Operator
                    object? operatorSymbolValue = operatorSymbolField.GetValue(type);

                    // Get the precedence of the Operator
                    object? precedenceValue = precedenceField.GetValue(type);

                    if (operatorSymbolValue is string && precedenceValue is int)
                    {
                        string operatorSymbol = (string)operatorSymbolValue;
                        int precedence = (int)precedenceValue;

                        // Invoke the function passed as parameter with the operator symbol and class
                        onOperator(operatorSymbol, type, precedence);
                    }
                }
            }
        }
    }
}