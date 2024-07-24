# Ara3D.SimpleDB

An extremely simple database-like data structure. 

This is a database in the loosest meaning of the word. 
It provides a mechanism for representing collections of objects 
as collections of binary tables.  

The primary use case is as an efficient representation for serialization 
or transmission.  

# Features 

One of the features is that string data is stored in a look-up table
to minimize repetition. This also allows tables rows to be stored 
as a fixed width binary representation. 
	
# Limitations 

Currently only integer and string fields are supported. 
The source code is simple, and extending it to support new field types 
is trivial.