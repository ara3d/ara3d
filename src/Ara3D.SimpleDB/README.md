# Ara3D.SimpleDB

An extremely simple database-like data structure. 

It provides a mechanism for representing related collections of objects 
as binary tables, and uses indices to store relationships. 

It offers the advantages of:

* Easy serialization in a structured binary format (BFAST) 
* BFAST can be read and written in different languages efficiently
* Reduced redundancy of data   
* Strings are deduplicated 

It does not support a query language, transactions, concurrency, 
or validation. Tables are limited to approximately 2billion rows

The primary use case is as an efficient representation for serialization 
or transmission of data, such as a native file format (e.g., 
https://www.sqlite.org/appfileformat.html). 

The secondary use case is as an adapater layer to arrange data so that ]
it can subsequently be easily brought into a modern database system. 

## Why not SQLite? 

This library is designed for programmers who have very simple and specialized 
use cases of reading, writing, and transmitting relational data, but who don't
need the features of a modern relational database management system (RDMS) 
and don't want to bring in the additional complexity and dependencies. 




	 