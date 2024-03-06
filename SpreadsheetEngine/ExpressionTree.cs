// <copyright file="ExpressionTree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// The class responsible for creating, manipulating, and evaluating expression trees.
/// </summary>
public class ExpressionTree
{
    /// <summary>
    /// Root of the tree.
    /// </summary>
    private ExpTreeNode? root;

    /// <summary>
    /// Dictionary mapping operator symbols to OperatorNode types.
    /// </summary>
    private Dictionary<char, Type> operatorNodeTypes = new Dictionary<char, Type>()
    {
        { AdditionNode.OperatorSymbol, typeof(AdditionNode) },
        { SubtractionNode.OperatorSymbol, typeof(SubtractionNode) },
        { MultiplicationNode.OperatorSymbol, typeof(MultiplicationNode) },
        { DivisionNode.OperatorSymbol, typeof(DivisionNode) },
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="expression">The expression to construct the tree from.</param>
    public ExpressionTree(string expression)
    {
        // TODO: Modularize construction (tokenize, postfix -> infix, build tree) and create test cases for each step.
        // Create the stacks to nodes and operators as the expression tree is being created
        var ops = new Stack<char>();
        var nodes = new Stack<ExpTreeNode>();

        // Iterate through each character in the expression
        foreach (var c in expression)
        {
            // If the character is a digit (value)
            if (c is >= '0' and <= '9')
            {
                // Push a ValueNode containing the value onto the node stack
                nodes.Push(new ValueNode(char.GetNumericValue(c)));
            }

            // If the character is an operator symbol (+, -, *, /)
            else if (this.operatorNodeTypes.ContainsKey(c))
            {
                // If an operator of equal or higher precedence is on the stack, append it to the top of the tree
                if (ops.Count > 0)
                {
                    // Pop the operator off the stack
                    var op = ops.Pop();
                    OperatorNode? newNode = null;

                    // Create a new instance of the corresponding operator node type
                    newNode = Activator.CreateInstance(this.operatorNodeTypes[op]) as OperatorNode;

                    // Append the new operator node to the top of the tree by assigning the top nodes on the stack as children
                    if (newNode != null)
                    {
                        newNode.RightChild = nodes.Pop();
                        newNode.LeftChild = nodes.Pop();
                        nodes.Push(newNode);
                    }
                }

                // Push the new operator onto the stack
                ops.Push(c);
            }
        }

        // Finalize the tree (last two nodes into one for the root)
        while (nodes.Count > 1)
        {
            var op = ops.Pop();
            OperatorNode? newNode = null;

            // Check if the operator symbol exists in the dictionary
            if (this.operatorNodeTypes.TryGetValue(op, value: out var type))
            {
                // Create a new instance of the corresponding node type
                newNode = Activator.CreateInstance(type) as OperatorNode;
            }

            // Append the new operator node to the top of the tree by assigning the top nodes on the stack as children
            if (newNode != null)
            {
                newNode.RightChild = nodes.Pop();
                newNode.LeftChild = nodes.Pop();
                nodes.Push(newNode);
            }
        }

        this.root = nodes.Pop();
    }

    /// <summary>
    /// Sets the specified variable within the ExpressionTree variables dictionary.
    /// </summary>
    /// <param name="variableName">The name of the variable (key).</param>
    /// <param name="variableValue">The value of the variable (value).</param>
    public void SetVariable(string variableName, double variableValue)
    {
        // TODO: Implement method to set the specified variable within the ExpressionTree variables dictionary
    }

    /// <summary>
    /// Evaluates the expression to a double value.
    /// </summary>
    /// <returns>Double value that the expression evaluates to.</returns>
    /// <exception cref="NotImplementedException">Placeholder exception while the method is not implemented.</exception>
    public double Evaluate()
    {
        var expTreeNode = this.root;
        if (expTreeNode != null)
        {
            return expTreeNode.Evaluate();
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}