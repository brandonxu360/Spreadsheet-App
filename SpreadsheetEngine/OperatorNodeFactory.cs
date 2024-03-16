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
    /// Factory method for instantiating instances of OperatorNode.
    /// </summary>
    /// <param name="op">The string symbol for the specific OperatorNode type.</param>
    /// <returns>A specific OperatorNode instance.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    public OperatorNode CreateOperatorNode(string op)
    {
        // TODO: Implement factory method for operator nodes
        throw new NotImplementedException();
    }
}