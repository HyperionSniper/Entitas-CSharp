## Overview
This is a fork of Entitas for my upcoming game, Machina (link to be added).

### Modifications required to build this yourself
If you download this for yourself, it should work properly, except for the project Entitas.Defs, which will require an reference to the assembly 1Hyperion.dll, which can be found in the Libraries folder (the reference uses a path to the Unity project for my game, which you will not have).

### If you ever need to pack the DLLs built from the projects for some reason
 I couldn't get sschmid's DLL packing script to work, so I made a tool called pack.exe in the root directory of the repo, which requires parameters and is run through pack.bat. You will need to edit the .bat for it to work. Change the 2nd parameter (see below) to "./Unity". It finds built dlls in the sln directory, then tries to find dlls with the same name in the target directory, then replaces those dlls if they are found.

pack.exe is used as such:
`pack.exe {replace|replaceT} {target directory containing a folder named Entitas to find/replace dlls} {sln directory to get dlls from}`
 
 
