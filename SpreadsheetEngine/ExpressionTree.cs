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
    /// The factory responsible for instantiating specific instances of operator nodes.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly OperatorNodeFactory operatorNodeFactory;

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
        // Initialize the variable dictionary
        this.VariableDict = new Dictionary<string, double>();

        // Initialize the infix expression to a default expression
        this.InfixStringExpression = "A1+B1+C1";

        // Initialize an instance of the OperatorNodeFactory
        this.operatorNodeFactory = new OperatorNodeFactory();

        // Set the expression tree given the default expression
        this.SetExpressionTree(this.InfixStringExpression);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="expression">The expression to construct the tree from.</param>
    public ExpressionTree(string expression)
    {
        // Initialize the infix expression to a default expression
        this.InfixStringExpression = "A1+B1+C1"; // This property will immediately be updated through SetExpressTree

        // Initialize an instance of the OperatorNodeFactory
        this.operatorNodeFactory = new OperatorNodeFactory();

        // Initialize the variable dictionary
        this.VariableDict = new Dictionary<string, double>();

        // Build and set the expression tree with the given expression
        this.SetExpressionTree(expression);
    }

    /// <summary>
    /// Gets the infix string representation of the expression the expression tree holds.
    /// For display purposes in the console application demo.
    /// </summary>
    public string InfixStringExpression { get; private set; }

    /// <summary>
    /// Gets or sets the variable dictionary for the expression, containing name-value key-value pairs.
    /// </summary>
    public Dictionary<string, double> VariableDict { get; set; }

    /// <summary>
    /// Builds the expression tree based on the infix string expression input and sets the root node accordingly.
    /// </summary>
    /// <param name="expression">The infix string expression to create the expression tree from.</param>
    public void SetExpressionTree(string expression)
    {
        // Set the infix string expression
        this.InfixStringExpression = expression;

        // Tokenize the expression
        var tokenizedExpression = this.Tokenize(expression);

        // Convert the expression to postfix
        var postFixTokenizedExpression = this.ConvertInfixToPostfix(tokenizedExpression);

        // Build the expression tree
        this.root = this.BuildExpressionTree(postFixTokenizedExpression);
    }

    /// <summary>
    /// Sets the specified variable within the ExpressionTree variables dictionary.
    /// NOTE: variable names must start with a an alphabet character, upper or lower-case, and be followed by any
    ///       number of alphabet characters and numerical digits (0-9).
    /// </summary>
    /// <param name="variableName">The name of the variable (key).</param>
    /// <param name="variableValue">The value of the variable (value).</param>
    public void SetVariable(string variableName, double variableValue)
    {
        // Throw argument exception if an invalid variable name is inputted
        if (string.IsNullOrWhiteSpace(variableName) || string.IsNullOrEmpty(variableName) ||
            !char.IsLetter(variableName[0]))
        {
            throw new ArgumentException(
                "Variable name must start with an alphabetical character.",
                nameof(variableName));
        }

        // Update or add new variable with the given name and value
        this.VariableDict[variableName] = variableValue;
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
            throw new NotSupportedException("Evaluating an empty expression tree is not supported.");
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
            else if (this.operatorNodeFactory.OperatorNodeTypes.ContainsKey(c.ToString()))
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
                while (i < expression.Length &&
                       !this.operatorNodeFactory.OperatorNodeTypes.ContainsKey(expression[i].ToString()) &&
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
    /// Converts a tokenized infix expression to postfix.
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
            if (this.operatorNodeFactory.OperatorNodeTypes.ContainsKey(token))
            {
                // Handle precedence and parentheses
                while (operatorStack.Count > 0 &&
                       this.operatorNodeFactory.Precedence.TryGetValue(operatorStack.Peek(), out var stackPrecedence) &&
                       this.operatorNodeFactory.Precedence[token] <= stackPrecedence)
                {
                    outputList.Add(operatorStack.Pop());
                }

                operatorStack.Push(token);
            }

            // If the token is a left parenthesis, push it onto the operator stack
            else if (token == "(")
            {
                operatorStack.Push(token);
            }

            // If the token is a right parenthesis, pop operators from the stack until a left parenthesis is encountered
            else if (token == ")")
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                {
                    outputList.Add(operatorStack.Pop());
                }

                if (operatorStack.Count == 0)
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                operatorStack.Pop(); // Discard the left parenthesis
            }

            // If the token is a number or variable, add it to the output queue
            else
            {
                outputList.Add(token);
            }
        }

        // Catch the last operators on the operator stack
        while (operatorStack.Count > 0)
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
    private ExpTreeNode BuildExpressionTree(List<string> postFixTokenizedExpression)
    {
        var stack = new Stack<ExpTreeNode>();

        // Iterate over each token in the postfix expression
        foreach (var token in postFixTokenizedExpression)
        {
            // Try creating a node with the token (null if not able to)
            var operatorNode = this.operatorNodeFactory.CreateOperatorNode(token);

            // If the token is an operator, pop two nodes from the stack and create a new operator node with them as children
            if (operatorNode is not null)
            {
                // Pop the right and left child nodes from the stack
                var rightChild = stack.Pop();
                var leftChild = stack.Pop();

                // Assign the right and left child nodes to the operator node
                operatorNode.LeftChild = leftChild;
                operatorNode.RightChild = rightChild;

                // Push the operator node onto the stack
                stack.Push(operatorNode);
            }

            // If the token is a variable (variables must start with a letter)
            else if (char.IsLetter(token[0]))
            {
                // Create variableNode and push to stack
                var variableNode = new VariableNode(this.VariableDict, token);
                stack.Push(variableNode);
            }

            // If the token is a number, create a new value node and push it onto the stack
            else
            {
                // Convert token to double
                if (double.TryParse(token, out var value))
                {
                    var valueNode = new ValueNode(value);
                    stack.Push(valueNode);
                }
                else
                {
                    throw new FormatException($"Invalid token: {token}");
                }
            }
        }

        // The root of the expression tree will be the only node left in the stack
        return stack.Pop();
    }
}