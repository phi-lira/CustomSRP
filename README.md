# HelloSRP
Bare Bones Scriptable Render Pipeline. Use it as a base to create your own. 

## How to use
Clone this github repository in your Unity's project `Packages` folder.
This will import HelloSRP package into your project.

1) Create a render pipeline asset by clicking on Assets -> Create -> Custom Render Pipeline Asset.
2) Assign the newly created render pipeline asset in the graphics settings.

## What's in the package

 + Runtime
    + CustomRenderPipeline.cs -  Contains rendering related code. Edit this file to desired rendering logic.
    + CustomRenderPipelineAsset.cs - Creates render pipeline instance. Can hold render pipeline resources.
 + ShaderLibrary
    + Core.hlsl - Includes all basic shader necessary files.
    + UnityInput.hlsl - Contains declaration of all built-in Unity shader constants.
 + Shaders
    + Unlit.shader - Unlit shader example.
