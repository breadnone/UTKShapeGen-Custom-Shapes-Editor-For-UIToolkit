# UTKShapeGen-Custom-Shapes-Editor-For-UIToolkit
Editor drawing tool to generate custom meshes/shapes as vector graphics for UIToolkit.  

![U9fgHbYY4P](https://user-images.githubusercontent.com/64100867/235291530-592eec69-a165-4b67-b64c-a212e03291a9.gif)  
![VCX6qdMum6](https://user-images.githubusercontent.com/64100867/235291635-fcad07d6-2017-4023-aed1-057b8d4bad2a.gif)


  
Features :  
- Line
- Curves
- Fill
- Mirror-mode
- Export as VectorGraphic

How-To :  
Remove points while in normal mode, just click the grid where it was originally assigned to. In point mode, right-click and choose 'Delete'  

Installation :  
- Download the .zip and extract UTKShapeGen to Assets folder
Note : Generated VectorImage will be exported to Assets/UTKShapeGen/Resources  
Note 2: This is just prove-of-concept stuff that I worked on on last weekend so other can work their way/improve it.
  
Requirement:
Unity3D 2022.2 or above (due to the new vector api)  

  
Made completely in c# with [UTK-Fluent extension for UIToolkit](https://github.com/breadnone/UTK-Fluent-extension-for-UIToolkit) 

  
TODO:  
- Masking support (partially implemented, hidden for now).  
- Multi drawing.  
- BezierCurves drawing (implemented, buggy still and hidden for now)  
