// <copyright file="VariableNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Node class to represent a variable (value assigned to a name).
/// </summary>
public class VariableNode : IExpTreeNode
{
    /// <summary>
    /// The name of the variable.
    /// </summary>
    private string name;

    /// <summary>
    /// The dictionary holding the name-value pairs for the variables.
    /// </summary>
    private Dictionary<string, double> varDict;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableNode"/> class.
    /// </summary>
    /// <param name="varDict">The dictionary holding the name-value pairs for the variables.</param>
    /// <param name="name">The name of the variable.</param>
    public VariableNode(Dictionary<string, double> varDict, string name)
    {
        this.varDict = varDict;
        this.name = name;
    }

    /// <summary>
    /// Returns the value of the key (name) lookup in the variable dictionary - variables evaluate to their values.
    /// </summary>
    /// <returns>The double value associated with the variable name.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    public double Evaluate()
    {
        // TODO: Implement evaluate for VariableNode
        throw new NotImplementedException();
    }
}