using System;
using System.Xml;

namespace HW1
{
    public class BST
    {
        // Root node of the BST instance with an initial value of null
        public BSTNode Root { get; private set; } = null;
        
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
    }
}