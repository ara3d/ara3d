# Ara 3D Node Editor 

## MVC+B Architectures

This node editor is built using a variation of the model view controller (MVC) architectural pattern which introduces behaviors (MVC+B).
Behaviors are small reusable components that store state, can respond to user input, can update presentation data,
and can draw on a canvas. 

There is a thin layer between this GUI system and WPF. It is designed to be agnostic of the window rendering framework used.

The GUI only has a single controller, which is responsible for creating and updating controls based on models, and updating models based on controls.

# Motivation 

So why a new GUI framework? Non-trivial GUI systems are hard to build and maintain.
THey are also tightly coupled to specific windowing systems, and are often not reusable.

The goal of this system is to make it easier to build complex GUI systems by providing a set of 
reusable components that can be combined in different ways.

We want to be able to model the data that we care about, and keep it separated from the details 
of how it is presented, interacted with, and animated.

Controls have internal state that is not part of the domain model. This state is used to manage how the control is 
displayed.

There are four things to consider:

1. Domain model - the data we care about in our core application
1. Style - options that control how the data is presented in a particular context
1. Behavior - how the data is interacted with, animated, and updated
1. Control state - data specific to a particular view, and is not part of the domain model

The goal was to separate these things into functional components that are not coupled, and can be composed easily.  

## Details 

A `Control` is an immutable class that acts as a container for a view class, a list of child controls, 
and a list of behaviors. A control itself is not customized. 

### Model 

A model is a class derived from `IModel`, which represents the underlying data that is manipulated by the UI.
An `IModel` provides a GUID and is used to generate the views. Models don't know anything about views. 
Models don't change, new models with the same GUID (and same underlying data type) are created as needed 
to represent updated states.  

### View 

A view is a class derived from `IView` which holds UI presentation state, styling data, dimensions, and logic. 
A view is immutable, and knows how to draw itself on a canvas. A view has a pointer to a model, without knowing anything 
other than its ID. 

### Style 

A style is a class derived fomr `IView` which holds information about how to draw specific views. They are like a set of 
'options' for controlling a specific view. 

### Controller 

A controller is a class derived from `IController` which acts as a bridge between views (specifically controls) and models.  
It can create or update a control given a model, and it can update a model from a control. 

There is one controller for the entire user interface.

### Behavior 

A behavior is an immutable class derived `IBehavior` which holds state and manages additional drawing and presentation logic in a 
generic way. Behaviors are updated (transformed) in response to user input.

### Behavior Trigger 

A behavior trigger is a class derived from `IBehaviorTrigger` that that adds a new behavior to a control based 
on user input. User input includes mouse position, keyboard events, and clock updates.