// <copyright file="ExpressionTreeTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests;

using System.Reflection;
using SpreadsheetEngine;

/// <summary>
/// Class to unit test the ExpressionTree functionality.
/// </summary>
[TestFixture]
internal class ExpressionTreeTests
{
    /// <summary>
    /// Tests the evaluate method of ValueNode in a normal case.
    /// </summary>
    [Test]
    public void ValueNodeEvaluateTest()
    {
        // Arrange
        const double value = 10.5;
        var valueNode = new ValueNode(value);

        // Act
        var result = valueNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests the evaluate method of VariableNode in a normal case.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateTest()
    {
        // Arrange
        const string name = "x";
        const double expectedValue = 20.7;
        var varDict = new Dictionary<string, double>
        {
            { "x", expectedValue },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    /// <summary>
    /// Tests that the evaluate method of VariableNode returns the default value of 0 when a value is not found
    /// in the dictionary.
    ///
    /// * If variables are not set by the user, they can be default to 0 for this HW. In later HWs, once we
    ///   learn how to deal with exceptions, we will change this.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateReturnsDefaultWhenVariableNotInDictionary()
    {
        // Arrange
        const string name = "y"; // Variable name not present in dictionary
        var varDict = new Dictionary<string, double>
        {
            { "x", 20.7 },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests the evaluate method of AdditionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double sum of the right and left values.</returns>
    [Test]
    [TestCase(5, 3, ExpectedResult = 8)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = -7)] // Two negative inputs
    [TestCase(5.5, 2.5, ExpectedResult = 8)] // Fractional inputs
    public double AdditionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var additionNode = new AdditionNode
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = additionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of SubtractionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double difference of the left and right values.</returns>
    [Test]
    [TestCase(5, 3, ExpectedResult = 2)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = 1)] // Two negative inputs
    [TestCase(5.5, 2.5, ExpectedResult = 3)] // Fractional inputs
    public double SubtractionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var subtractionNode = new SubtractionNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = subtractionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of MultiplicationNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double result of multiplying the left and right values.</returns>
    [Test]
    [TestCase(2, 3, ExpectedResult = 6)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = 12)] // Two negative inputs
    [TestCase(0.5, 4, ExpectedResult = 2)] // Fractional inputs
    public double MultiplicationNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var multiplicationNode = new MultiplicationNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = multiplicationNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of DivisionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double result of dividing the left value by the right value.</returns>
    [Test]
    [TestCase(6, 3, ExpectedResult = 2)] // Two positive inputs
    [TestCase(0, 1, ExpectedResult = 0)] // Left input zero
    [TestCase(-12, -4, ExpectedResult = 3)] // Two negative inputs
    [TestCase(4, 0.5, ExpectedResult = 8)] // Fractional inputs
    public double DivisionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var divisionNode = new DivisionNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = divisionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the construction of ExpressionTree in exceptional cases.
    /// </summary>
    /// <param name="expression">The string expression used to build the expression tree.</param>
    [Test]
    [TestCase("+")] // Expression of only a single add operator missing both operands
    [TestCase("2+")] // Expression with an add operator missing the second operand
    public void ExpressionTreeConstructionExceptional(string expression)
    {
        // InvalidOperationException expected - tree will not build properly
        Assert.That(
            () => new ExpressionTree(expression),
            Throws.TypeOf<InvalidOperationException>());
    }

    /// <summary>
    /// Tests the evaluate method of ExpressionTree in normal cases.
    /// </summary>
    /// <param name="expression">The string expression used to build the expression tree.</param>
    /// <returns>The double evaluated value of the expression.</returns>
    [Test]
    [TestCase("3+7", ExpectedResult = 10)] // Expression with a single add operator
    [TestCase("3+7+2+1", ExpectedResult = 13)] // Expression with multiple add operators
    [TestCase("3/7", ExpectedResult = 3.0 / 7.0)] // Expression with a single division operator
    [TestCase("3/7/2/1", ExpectedResult = 3.0 / 7.0 / 2.0 / 1.0)] // Expression with multiple division operators
    public double ExpressionTreeEvaluateTestNormal(string expression)
    {
        var exp = new ExpressionTree(expression);
        return exp.Evaluate();
    }

    /// <summary>
    /// Tests the private tokenize method of ExpressionTree in a normal case with operators and values.
    /// </summary>
    [Test]
    public void TokenizePrivateMethodTestNormal()
    {
        // Arrange
        const string expression = "3+7+1";
        var expectedTokens = new List<string> { "3", "+", "7", "+", "1" };

        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var tokenizeMethod =
            typeof(ExpressionTree).GetMethod("Tokenize", BindingFlags.NonPublic | BindingFlags.Instance);

        if (tokenizeMethod == null)
        {
            Assert.Fail("Tokenize method not found");
            return;
        }

        // Act
        var result = (List<string>)tokenizeMethod.Invoke(expressionTree, new object[] { expression })!;

        // Assert
        Assert.That(result, Is.EqualTo(expectedTokens));
    }

    /// <summary>
    /// Tests the private tokenize method of ExpressionTree in a normal case with operators, values, and multi character variable names.
    /// </summary>
    [Test]
    public void TokenizePrivateMethodTestNormalWithVariables()
    {
        // Arrange
        const string expression = "3+7+hello+2+b";
        var expectedTokens = new List<string> { "3", "+", "7", "+", "hello", "+", "2", "+", "b" };

        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var tokenizeMethod =
            typeof(ExpressionTree).GetMethod("Tokenize", BindingFlags.NonPublic | BindingFlags.Instance);

        if (tokenizeMethod == null)
        {
            Assert.Fail("Tokenize method not found");
            return;
        }

        // Act
        var result = (List<string>)tokenizeMethod.Invoke(expressionTree, new object[] { expression })!;

        // Assert
        Assert.That(result, Is.EqualTo(expectedTokens));
    }

    /// <summary>
    /// Tests the private ConvertInfixToPostfix method of ExpressionTree in normal cases.
    /// </summary>
    [Test]
    public void ConvertInfixToPostfixPrivateMethodTestNormal()
    {
        // Arrange
        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var convertInfixToPostfixMethod =
            typeof(ExpressionTree).GetMethod("ConvertInfixToPostfix", BindingFlags.NonPublic | BindingFlags.Instance);

        if (convertInfixToPostfixMethod == null)
        {
            Assert.Fail("ConvertInfixToPostfix method not found");
            return;
        }

        var infixExpressions = new Dictionary<List<string>, List<string>>
        {
            { ["3", "+", "7"], ["3", "7", "+"] },
            { ["3", "-", "2", "-", "8", "-", "8"], ["3", "2", "-", "8", "-", "8", "-"] },

            // Precedence not to be implemented yet
            /* { "5*4+2", ["5", "4", "*", "2", "+"] },
            { "5*(4+2)", ["5", "4", "2", "+", "*"] },
            { "(5-3)*(7+2)", ["5", "3", "-", "7", "2", "+", "*"] },
            { "3+4*2/(1-5)^2^3", ["3", "4", "2", "*", "1", "5", "-", "2", "3", "^", "^", "/", "+"] }, */
        };

        foreach (var infixExpression in infixExpressions)
        {
            // Act
            var result =
                (List<string>)convertInfixToPostfixMethod.Invoke(expressionTree, new object[] { infixExpression.Key })!;

            // Assert
            Assert.That(result, Is.EqualTo(infixExpression.Value));
        }
    }

    /// <summary>
    /// Tests the private BuildExpressionTree method of ExpressionTree in normal cases.
    /// </summary>
    [Test]
    public void BuildExpressionTreePrivateMethodTestNormal()
    {
        // Arrange
        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var buildExpressionTreeMethod =
            typeof(ExpressionTree).GetMethod("BuildExpressionTree", BindingFlags.NonPublic | BindingFlags.Instance);

        if (buildExpressionTreeMethod == null)
        {
            Assert.Fail("BuildExpressionTree method not found");
            return;
        }

        // Define postfix expression along with its expected expression tree
        List<string> postfixExpression = ["5", "4", "+", "3", "+",];
        var expectedTree = new AdditionNode()
        {
            LeftChild = new AdditionNode()
            {
                LeftChild = new ValueNode(5),
                RightChild = new ValueNode(4),
            },
            RightChild = new ValueNode(3),
        };

        // Act
        var actualTree =
            buildExpressionTreeMethod.Invoke(expressionTree, new object?[] { postfixExpression });

        // Assert

        // Check root (should be AdditionNode)
        var expectedRoot = expectedTree;
        var actualRoot = actualTree; // Should be AdditionNode

        Assert.That(actualRoot?.GetType(), Is.EqualTo(expectedRoot.GetType()));

        // Check left child (Should be AdditionNode)
        var expectedLeftChild = expectedRoot.LeftChild;
        var actualLeftChild = ((AdditionNode)actualRoot!).LeftChild; // Should be AdditionNode

        Assert.That(actualLeftChild?.GetType(), Is.EqualTo(expectedLeftChild.GetType()));

        // Check right child (Should be ValueNode(3))
        var expectedRightChild = expectedTree.RightChild;
        var actualRightChild = ((AdditionNode)actualRoot).RightChild; // Should be ValueNode with value 3
        Assert.Multiple(() =>
        {
            Assert.That(actualRightChild?.GetType(), Is.EqualTo(expectedRightChild.GetType()));
            Assert.That(actualRightChild?.Evaluate(), Is.EqualTo(expectedRightChild.Evaluate()));
        });

        // Check left-left child
        var expectedLeftLeftChild = ((AdditionNode)expectedTree.LeftChild!).LeftChild;
        var actualLeftLeftChild = ((AdditionNode)((AdditionNode)actualRoot).LeftChild!).LeftChild;
        Assert.Multiple(() =>
        {
            Assert.That(actualLeftLeftChild?.GetType(), Is.EqualTo(expectedLeftLeftChild!.GetType()));
            Assert.That(actualLeftLeftChild?.Evaluate(), Is.EqualTo(expectedLeftLeftChild.Evaluate()));
        });

        // Check left-right child
        var expectedLeftRightChild = ((AdditionNode)expectedTree.LeftChild!).RightChild;
        var actualLeftRightChild = ((AdditionNode)((AdditionNode)actualRoot).LeftChild!).RightChild;
        Assert.Multiple(() =>
        {
            Assert.That(actualLeftRightChild?.GetType(), Is.EqualTo(expectedLeftRightChild!.GetType()));
            Assert.That(actualLeftRightChild?.Evaluate(), Is.EqualTo(expectedLeftRightChild.Evaluate()));
        });
    }

    /// <summary>
    /// Tests the SetVariable method to create a new variable name-value pair in the ExpressionTree class.
    /// </summary>
    [Test]
    public void SetVariableTestNewVariable()
    {
        // Arrange
        var expressionTree = new ExpressionTree();
        const string variableName = "A1";
        const double variableValue = 5.0;

        // Act
        expressionTree.SetVariable(variableName, variableValue);

        // Assert
        Assert.That(expressionTree.VariableDict.ContainsKey(variableName));
        Assert.That(expressionTree.VariableDict[variableName], Is.EqualTo(variableValue));
    }

    /// <summary>
    /// Tests the SetVariable method to update a variable value in the ExpressionTree class.
    /// </summary>
    [Test]
    public void SetVariableTestUpdateExistingVariable()
    {
        // Arrange
        var expressionTree = new ExpressionTree();
        const string variableName = "A1";
        const double initialValue = 5.0;
        const double updatedValue = 10.0;

        // Act
        expressionTree.SetVariable(variableName, initialValue);
        expressionTree.SetVariable(variableName, updatedValue);

        // Assert
        Assert.That(expressionTree.VariableDict.ContainsKey(variableName));
        Assert.That(expressionTree.VariableDict[variableName], Is.EqualTo(updatedValue));
    }

    /// <summary>
    /// Tests the SetVariable method to create an invalid new variable name-value pair in the ExpressionTree class.
    /// The name will not follow the required conventions stated in the method summary.
    /// </summary>
    [Test]
    public void SetVariableTestNewVariableException()
    {
        // Arrange
        var expressionTree = new ExpressionTree();
        const string variableName = "1A"; // Variable names must start with an alphabetical character
        const double variableValue = 5.0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => expressionTree.SetVariable(variableName, variableValue));
        Assert.IsFalse(expressionTree.VariableDict.ContainsKey(variableName));
    }
}