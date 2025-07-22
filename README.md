### XNoise (MIT)

Want to generate procedural noise with real-time previews and GPU acceleration? XNoise lets you build and evaluate LibNoise-style graphs directly in Unity. Just import the .unitypackage and start creating.  
This is essentially libnoise on the CPU and GPU with a node editor (alpha).
<br> 
<p align="center">
<img alt="Perlin_1024x1024_ Seed _9042___Frequency_3_Lacunarity_2_Persistence_0 5_Octaves_6" src="https://github.com/user-attachments/assets/53ec51dc-b8e3-4bc3-919a-ff846329b73a" width="250"/>
<img alt="Wood_1024x1024_ Seed _9042___Perlin_Frequency_48_Perlin_Lacunarity_2 20703125_Perlin_Persistence_0 5_Scale_X_0_Scale_Y_0 25_Scale_Z_0_ScaleBias_Bias_0 125_ScaleBias_Scale_0 25_Turbulence1_Power_0 1_Cylinders_Frequency_16_" src="https://github.com/user-attachments/assets/b7ae940f-c11c-4688-bc88-878fca390d01"  width="250"/>
<img alt="Voronoi_1024x1024_ Seed _9042___Distance_True_Frequency_22_Displacement_0" src="https://github.com/user-attachments/assets/724925b4-01cd-47cc-bd7a-6407fca29de4" width="250"/>
</p>

<p align="center">
<img width="250" height="250" alt="Jade_1024x1024_ Seed _9042___Cylinders_Frequency_2_Rotate_X_90_Rotate_Y_25_Rotate_Z_0_Turbulence1_Power_1_ScaleBias_Bias_0_ScaleBias_Scale_0 25_Ridged_Frequency_2_Ridged_Lacunarity_2 20703125_Turbulence_Power_1_Turbulence" src="https://github.com/user-attachments/assets/c7a4dd8f-3107-4259-8666-71ffa3d371cc" />
<img width="250" height="250" alt="Ridged Multifractal_1024x1024_ Seed _9042___Frequency_2_Lacunarity_2_Octaves_222" src="https://github.com/user-attachments/assets/4c3c16f6-1944-47c3-b1c3-e3cd6e73f68d" />
<img width="250" height="250" alt="Island_1024x1024_ Seed _4176__" src="https://github.com/user-attachments/assets/d6afd27e-16eb-4d0b-a2ce-0787454ba6f2" />
</p>
<br>

### XNoise is a glue layer that
* ports the classic libnoise algorithms to Unity surface shaders.
* lets you combine them in a node graph powered by xNode.
* CRUD the graph in C# too.

### What you don’t get (yet)
* CPU <-> GPU parity.
* easy area selection when rendering either on planar, spherical or cylindrical.
* ShaderGraph style shader baking.
* compute shader evaluation.
* Burst-compiled code.
* Noise complexity selection (like switching between 2-3-4D noise, although it is in the code).

### Folder layout

XNoise
 - SubModules
   - Libnoise
   - CustomGraphs
   - XNode


### Requirements
* Unity 6 LTS or newer

Install either with Git or the .unitypackage.

### Quick start
* Create a Noise Graph.
* Right Click in the assets -> XNoise -> Graph.
* Double-click the asset to open the XNoise graph editor.
* Right click in the graph to add a node, connect it to Output, tweak the sliders.
* To have the graph outputing a value connect your end module.

    In your script, evaluate the graph on the GPU:

### example:
```csharp
public class NoiseExample : MonoBehaviour
{
    public XNoiseGraph graph;

    void Start()
    {
        // this renders on the GPU
        graph.renderer.renderMode = XNoise.Renderer.RenderMode.GPU;
        graph.renderer.Render();
        // moves data into a texture2d
        graph.renderer.StoreFinalizedTexture();
        // save the rendered texture as HelloWorld.png in /Assets/
        graph.renderer.Save("HelloWorld");
    }
}
```

You can find more examples here :
https://github.com/Arghonot/XNoise_Demo
And a live version here :
https://arghonot.itch.io/xnoise


See LICENSE and THIRD_PARTY.md for full texts.
Status

You can see the project's trello here :
https://trello.com/b/5cTlM4xy/xnoise-nodes


**Early alpha – APIs will break, performance is unoptimised, and docs are thin.**  
<br>
Feedback is welcome!
