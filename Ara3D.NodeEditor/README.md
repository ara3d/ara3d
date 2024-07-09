# Ara 3D Node Editor 

## MVC+B Architectures

This node editor is built using a variation of the model view controller (MVC) architectural pattern which introduces behaviors (MVC+B).
Behaviors are small reusable components that store state, can respond to user input, can update presentation data,
and can draw on a canvas. 

There is a very thin layer between this GUI system and WPF. It is designed to be agnostic of the window rendering framework used.

## Details 

A control is a container for a view class and contains a list of child controls and a list of behaviors.

A model is a class derived from `IModel`, which represents the underlying data that is manipulated by the UI.
An `IModel` provides a GUID and is used to generate the views. Models don't know anything about views. 
If a model is changed, the GUID will stay the same. A model should not change, but this is up to the application. 

A view is a class derived from `IView` which holds UI presentation state, styling data, dimensions, and logic. 
A view is immutable, and knows how to draw itself on a canvas. A view has a pointer to a model, without knowing anything 
other than its ID. 

A controller is a class derived from `IController` which acts as a bridge between views (specifically controls) and models.  
It can either create or update a control given a model, and can update a model from a control. 

A behavior is an immutable class derived `IBehavior` which holds state and manages additional drawing and presentation logic in a 
generic way. Behaviors are updated (transformed) in response to user input.

A behavior trigger is a class that that adds a new behavior to a control based on user input. 

User input includes mouse position, keyboard events, and clock updates.