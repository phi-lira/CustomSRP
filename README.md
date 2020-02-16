# Custom SRP Template
This is a template Scriptable Render Pipeline (SRP) package for Unity.
It contains boiler plate code to be used as starting point when creating custom render pipelines.
It also already creates base files to distribute and install this custom SRP as a package for Unity.

## Create your custom SRP package from this template
+ Create a Github repository from this template by clicking on the `Use this template` button at the top.
+ Edit `package.json` file to configure the [package manifest](https://docs.unity3d.com/Manual/upm-manifestPkg.html).
    + Note: If you change package name, be sure to all change shader includes.
+ Update `README.md`, `CHANGELOG.md` and `LICENSE.md` files to reflect the contents of your package.

## How to install your package in your Unity project
+ In Unity, click at the top bar on `Window -> Package Manager`. 
+ Follow these [instructions](https://docs.unity3d.com/Manual/upm-ui-local.html) to install it as a local package.

## How to use
+ Create a render pipeline asset by clicking on `Assets -> Create -> Custom Render Pipeline Asset`.
+ In [Graphics Settings](https://docs.unity3d.com/Manual/class-GraphicsSettings.html), assign the newly created render pipeline asset in the `Scriptable Render Pipeline Settings` field.

## What's in the package
The package follows the Unity's Package Manager [recommended layout](https://docs.unity3d.com/Manual/cus-layout.html)
+ `package.json` describes the [package metadata](https://docs.unity3d.com/Manual/upm-manifestPkg.html) required by Package Manager.
+ `README.md` contains project description used by Github.
+ `CHANGELOG.md` contains changelog entries. This is used by Package Manager to display changelog information in the Editor.
+ `LICENSE.md` containts package license information. This is used by Package Manager to display license information in the Editor.
+ Editor
    + `Unity.com.render-pipelines.Editor.asmdef` the [assembly definition](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) file for editor scripts.
+ Runtime
    + `CustomRenderPipeline.cs` -  Contains rendering related code. Edit this file to add the desired rendering logic.
    + `CustomRenderPipelineAsset.cs` - Creates render pipeline instance. Can hold render pipeline resources.
    + `Unity.com.render-pipelines.asmdef` - the [assembly definition](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) file for editor scripts.
+ ShaderLibrary
    + `Core.hlsl` - Includes all basic shader necessary files.
    + `UnityInput.hlsl` - Contains declaration of all built-in Unity shader constants.
+ Shaders
    + `Unlit.shader` - Unlit shader example.
+ Tests
    + Contains Editor and Runtime test folders. To be used with [UnityTestRunner](https://docs.unity3d.com/2020.1/Documentation/Manual/testing-editortestsrunner.html) to write automated package tests.
+ Documentation~
    + `index.md` - Contains index page for documentation. This is used by Package Manager to display package docs.
+ Samples~
    + Add any sample scenes to your package here. This will be displayed by Package Manager.
