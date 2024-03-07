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
    /// Dictionary mapping operator symbols to OperatorNode types.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly Dictionary<string, Type> operatorNodeTypes = new Dictionary<string, Type>()
    {
        { AdditionNode.OperatorSymbol, typeof(AdditionNode) },
        { SubtractionNode.OperatorSymbol, typeof(SubtractionNode) },
        { MultiplicationNode.OperatorSymbol, typeof(MultiplicationNode) },
        { DivisionNode.OperatorSymbol, typeof(DivisionNode) },
    };

    /// <summary>
    /// Root of the tree.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private ExpTreeNode? root;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    public ExpressionTree()
    {
        const string expression = "A1+B1+C1"; // Have a default expression

        // Tokenize the expression
        var tokenizedExpression = this.Tokenize(expression);

        // TODO: Convert the tokenized expression to postfix order
        // var postFixTokenizedExpression = this.ConvertInfixToPostfix(tokenizedExpression);

        // TODO: Build tree using the tokenized postfix expression and assign the node returned as the root
        // this.root = this.BuildExpressionTree(postFixTokenizedExpression);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="expression">The expression to construct the tree from.</param>
    public ExpressionTree(string expression)
    {
        // Tokenize the expression
        var tokenizedExpression = this.Tokenize(expression);

        // TODO: Convert the tokenized expression to postfix order
        // var postFixTokenizedExpression = this.ConvertInfixToPostfix(tokenizedExpression);

        // TODO: Build tree using the tokenized postfix expression and assign the node returned as the root
        // this.root = this.BuildExpressionTree(postFixTokenizedExpression);
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
    /// <exception cref="NotSupportedException">Evaluating an empty expression tree is not supported.</exception>
    public double Evaluate()
    {
        // If the root is not null, call the evaluate method of the root, which will recursively evaluate the entire tree
        if (this.root != null)
        {
            return this.root.Evaluate();
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Tokenizes the expression string.
    /// </summary>
    /// <param name="expression">The string expression.</param>
    /// <returns>A list of string tokens, which could represent values or operators.</returns>
    private List<string> Tokenize(string expression)
    {
        var tokenList = new List<string>();
        var i = 0; // Iterator index

        // Iterate through the entire expression
        while (i < expression.Length)
        {
            var c = expression[i]; // Current character

            // Whitespace encountered
            if (char.IsWhiteSpace(c))
            {
                i++; // Skip whitespace
            }

            // Operator encountered
            else if (this.operatorNodeTypes.ContainsKey(c.ToString()))
            {
                // Add whitespace as token
                tokenList.Add(c.ToString());
                i++;
            }

            // Variable or value encounter
            else
            {
                // Read characters until whitespace, operator, or end of expression is encountered
                var start = i;
                while (i < expression.Length && !this.operatorNodeTypes.ContainsKey(expression[i].ToString()) &&
                       !char.IsWhiteSpace(expression[i]))
                {
                    i++;
                }

                tokenList.Add(expression.Substring(start, i - start));
            }
        }

        return tokenList;
    }

    /// <summary>
    /// Converts a tokenized infix expression to postfix. Only supports expressions of one operator type.
    /// </summary>
    /// <param name="tokenizedExpression">A list of string tokens representing an expression in infix order.</param>
    /// <returns>A list of string tokens representing the expression in postfix order.</returns>
    private List<string> ConvertInfixToPostfix(List<string> tokenizedExpression)
    {
        var outputList = new List<string>();
        var operatorStack = new Stack<string>();

        // Iterate over each token in the tokenized expression
        foreach (var token in tokenizedExpression)
        {
            // If the token is an operator
            if (this.operatorNodeTypes.ContainsKey(token))
            {
                if (operatorStack.Count > 0)
                {
                    // Push the operators off the operator stack onto the output stack if operators are on the operator stack
                    outputList.Add(operatorStack.Pop());
                }

                operatorStack.Push(token);
            }

            // If the token is a number or variable, add it to the output queue
            else
            {
                outputList.Add(token);
            }
        }

        // Catch the last operator on the operator stack
        if (operatorStack.Count > 0)
        {
            outputList.Add(operatorStack.Pop());
        }

        return outputList;
    }

    /// <summary>
    /// Builds the expression tree based on the postfix tokenized expression passed in.
    /// </summary>
    /// <param name="postFixTokenizedExpression">A tokenized expression in postfix order.</param>
    /// <returns>The ExpTreeNode root node of the resultant tree.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private ExpTreeNode BuildExpressionTree(List<string> postFixTokenizedExpression)
    {
        throw new NotImplementedException();
    }
}