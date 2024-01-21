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
            Console.Write('[');
            DisplayInOrder(Root); // Call private recursive in-order function on root
            Console.Write(']');
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
    }
}