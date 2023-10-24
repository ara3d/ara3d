# Ara3D.ScriptEditor

This project contains a C# script editor, that is intended to be used as a service, by a host 
application that manages its own compilation. 
Communication is managed by WCF. See `ServiceAPI.cs`.

This project is used by the Ara3D.3dsMaxBridge so that 3ds Max users can 
launch an editor for C# files in a separate process. 

Normally I would have made this a class library, but due to some pecularities of 3ds Max
I had to make this its own process. However the advantage is that multiple tools can 
use the same process/tool for the script editor.  

Because this is a library potentially used by multiple programs 
it is built to `$(ProgramData)\Ara3D\ScriptEditor`.
    
