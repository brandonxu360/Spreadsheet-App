/*
 * NAME: Brandon Xu
 * SID:  11815117
 */

using System;

namespace HW1
{
    public class BST
    {
        // Root node of the BST instance with an initial value of null
        public BSTNode Root { get; private set; }
        
        // Adds new BSTNode that contains the input int value
        public void Add(int value)
        {
            if (Root == null)
            {
                // If no root exists, create a new node with input value and set as root
                Root = new BSTNode(value);
            } else
            {
                // If root exists, call on recursive add function in BSTNode class
                Root.Add(value);
            }
        }
        
        // Public function to display BST node values in order
        public void DisplayInOrder()
        {
            Console.Write("BST In Order: [");
            DisplayInOrder(Root); // Call private recursive in-order function on root
            Console.Write("]\n");
        }

        // Private recursive function to traverse the BST in order and write the values of the nodes to console
        // Used in public DisplayInOrder function
        private void DisplayInOrder(BSTNode node) 
        {
            // If left subtree exists, traverse left
            if (node.LNode != null) DisplayInOrder(node.LNode);
            
            // Write current node value to console
            Console.Write($" {node.Value} ");
            
            // If right subtree exists, traverse right
            if (node.RNode != null) DisplayInOrder(node.RNode);
        }

        // Public count function that returns int number of nodes in BST
        public int NodeCount()
        {
            return NodeCount(Root); // Call private recursive count function on root
        }

        // Private recursive count function to return int number of nodes in the subtree defined by the input node as root
        private int NodeCount(BSTNode node)
        {
            if (node == null) return 0; // Null node check (base case)
            
            var count = 1; // Current node increments count by 1

            // Increment count by number of nodes in left and right subtrees (if they are not null)
            count += NodeCount(node.RNode); 
            count += NodeCount(node.LNode);

            return count;
        }
        
        // Public function to return number of levels in BST
        public int LevelCount()
        {
            return LevelCount(Root);
        }
        
        // Private recursive function to return number of levels in BST defined by input node as root
        private int LevelCount(BSTNode node)
        {
            if (node == null) return 0; // Null node check (base case)
            
            // Recursively determine number of levels for the left and right subtrees
            var rightLevelCount = LevelCount(node.RNode);
            var leftLevelCount = LevelCount(node.LNode);

            // Return the higher level count (incremented by one for current node level)
            return (rightLevelCount > leftLevelCount) ? rightLevelCount + 1 : leftLevelCount + 1;
        }

        // Public function to return minimum level based on number of nodes
        public int MinLevelCount()
        {
            return (int)Math.Ceiling(Math.Log(NodeCount(Root), 2));
        }
    }
}