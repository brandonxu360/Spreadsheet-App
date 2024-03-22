// <copyright file="OperatorNodeFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

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
    public readonly Dictionary<string, Type> OperatorNodeTypes = new Dictionary<string, Type>()
    #pragma warning restore SA1401
    {
        { AdditionNode.OperatorSymbol, typeof(AdditionNode) },
        { SubtractionNode.OperatorSymbol, typeof(SubtractionNode) },
        { MultiplicationNode.OperatorSymbol, typeof(MultiplicationNode) },
        { DivisionNode.OperatorSymbol, typeof(DivisionNode) },
    };

    /// <summary>
    /// Define operator precedence for each operator type.
    /// </summary>
    // Disable private suggestion = dictionary is accessed by the ExpressionTree class
    #pragma warning disable SA1401
    public readonly Dictionary<string, int> Precedence = new Dictionary<string, int>
    #pragma warning restore SA1401
    {
        { AdditionNode.OperatorSymbol, AdditionNode.Precedence },
        { SubtractionNode.OperatorSymbol, SubtractionNode.Precedence },
        { MultiplicationNode.OperatorSymbol, MultiplicationNode.Precedence },
        { DivisionNode.OperatorSymbol, DivisionNode.Precedence },
    };

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
}