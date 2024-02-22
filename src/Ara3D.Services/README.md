# Ara3D.Services

Ara 3D Services is a set of interfaces 
designed to provide a foundation for software architecture 
that supports building scalable and decoupled applications. 

It uses the Domo state management library and provides 
additional concepts: 

* IService - Contains commands, optionally may house a repository, 
and subscribe to or publish events. Usually one service of each 
type is present in an application. 

* IEventBus - used to communicate messages (called events)
from publishers to subscribers in a thread safe way, without
the resource leaking problem 

* IApplication - Contains references to all services, repositories,
and an event bus. 

Additional concepts from Ara3D.Domo:

* IModel - a wrapper around a data object that has an identity 
if the underlying value changes, the identity stays the same, 
but observers can be easily notified. 

* IRepository - a static container for one or more models. 
Associates model values with the underlying 

