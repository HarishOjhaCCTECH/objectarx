# ObjectARX and .NET Assignments Repository

This repository contains assignments focusing on ObjectARX and .NET programming for AutoCAD plugin development. The assignments cover various aspects of plugin development, including project setup, custom command creation, manipulation of drawing elements, interaction with the user, and more.

## Assignments Overview

1. **Assignment 1: Create a New ObjectARX Project**
   - Creating a new project from scratch in Microsoft Visual Studio.
   - Configuring the project to build an ObjectARX application.
   - Defining the `acrxEntryPoint` function and writing minimal code.

2. **Assignment 2: Compile and Load an ObjectARX Project**
   - Building a debug version of the ObjectARX application.
   - Loading and unloading an ObjectARX application.

3. **Assignment 3: Define a Custom Command**
   - Defining a function to display a message at the command prompt.
   - Registering a custom command.
   - Removing a command group.
   - Testing global and local command names.

4. **Assignment 4: Add a Line to Model Space**
   - Defining a function to add a new graphical object to model space.
   - Getting a reference to model space.
   - Creating a new line object and appending it to model space.

5. **Assignment 5: Create a New Layer**
   - Defining a function to add a new non-graphical object (LayerTableRecord) to the LayerTable.
   - Checking for the existence of an existing layer.
   - Creating and modifying a new layer.
   - Appending the new LayerTableRecord to the LayerTable.

6. **Assignment 6: Step through All Objects in the Database**
   - Defining a function to step through the block of the current space.
   - Outputting the class name of each entity in the current space.
   - Displaying the AutoCAD Text Window or expanding the Command Line window.

7. **Assignment 7: Add a Line Using User Input**
   - Defining a function to prompt for two points.
   - Adding a line based on the two points provided by the user.

8. **Assignment 8: Select Objects and Request a Keyword**
   - Defining a function to prompt for objects.
   - Prompts for a keyword.
   - Stepping through the objects in the selection set.
   - Modifying the color of the selected objects based on the chosen keyword.

## Repository Structure

The repository is structured as follows:

- `Assignment1_ObjectARX/`: Contains code and resources for Assignment 1 written in C++ with ObjectARX.
- `Assignment2_ObjectARX/`: Contains code and resources for Assignment 2 written in C++ with ObjectARX.
- `Assignment3_to_8_DOTNET/`: Contains code and resources for Assignments 3 to 8 written in .NET.
