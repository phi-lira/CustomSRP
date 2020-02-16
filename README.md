# Custom SRP Template
HelloSRP is a template Scriptable Render Pipeline (SRP) package for Unity.
It contains boiler plate code to be used as starting point when creating custom render pipelines.
It also already creates setups base files to distribute this custom SRP as a package for Unity.

## Create your custom SRP package from this template
+ Create a Github repository from this template by clicking on the `Use this template` button at the top.
+ In `com.unity.render-pipelines.custom` folder, edit `package.json` file to configure the [package manifest].(https://docs.unity3d.com/Manual/upm-manifestPkg.html).
+ Update `README.md` and `LICENSE` files to reflect the contents of your package.

## How to install your package in your Unity project
+ In Unity, click at the top bar on `Window -> Package Manager`. 
+ Follow these [instructions](https://docs.unity3d.com/Manual/upm-ui-local.html) to install it as a local package.

## How to use

1) Create a render pipeline asset by clicking on `Assets -> Create -> Custom Render Pipeline Asset`.
2) In [Graphics Settings](https://docs.unity3d.com/Manual/class-GraphicsSettings.html), assign the newly created render pipeline asset in the `Scriptable Render Pipeline Settings` field.

## What's in the package

 + Runtime
    + CustomRenderPipeline.cs -  Contains rendering related code. Edit this file to add the desired rendering logic.
    + CustomRenderPipelineAsset.cs - Creates render pipeline instance. Can hold render pipeline resources.
 + ShaderLibrary
    + Core.hlsl - Includes all basic shader necessary files.
    + UnityInput.hlsl - Contains declaration of all built-in Unity shader constants.
 + Shaders
    + Unlit.shader - Unlit shader example.
