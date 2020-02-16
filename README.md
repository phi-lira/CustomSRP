# HelloSRP
Custom boiler plate render pipeline to be used as starting point when creating custom render pipelines.

## How to use

Clone this github repository in your Unity's project `Packages` folder.
This will import HelloSRP package into your project.

1) Create a render pipeline asset by clicking on Assets -> Create -> Custom Render Pipeline Asset.
2) Assign the newly created render pipeline asset in the graphics settings.

## What's in the package

 + Runtime
    + CustomRenderPipeline.cs -  Contains rendering related code. Edit this file to add the desired rendering logic.
    + CustomRenderPipelineAsset.cs - Creates render pipeline instance. Can hold render pipeline resources.
 + ShaderLibrary
    + Core.hlsl - Includes all basic shader necessary files.
    + UnityInput.hlsl - Contains declaration of all built-in Unity shader constants.
 + Shaders
    + Unlit.shader - Unlit shader example.
