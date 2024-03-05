// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

var running = true;

while (running)
{
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Enter an expression string");
    Console.WriteLine("2. Set a variable value");
    Console.WriteLine("3. Evaluate expression");
    Console.WriteLine("4. Quit");
    Console.Write("Enter your choice: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("Menu item 1 selected");
            break;
        case "2":
            Console.WriteLine("Menu item 2 selected");
            break;
        case "3":
            Console.WriteLine("Menu item 3 selected");
            break;
        case "4":
            running = false;
            break;
    }
}