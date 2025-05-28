using LibNoise.Operator;
using LibNoise;
using UnityEngine;
using System.IO;
using LibNoise.Generator;

namespace Xnoise
{
    [System.Serializable]
    [CreateNodeMenu("NoiseGraph/Modifier/Curve")]
    public class CurveNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public AnimationCurve InputCurve;

        public Texture2D animCurve;
        public Texture2D InputTexture;
        public Texture2D RenderedTexture;
        public Material gpuMaterial;

        public override object Run()
        {
            Curve curve = new Curve(
                GetInputValue<SerializableModuleBase>("Input", this.Input));

            curve.mathematicalCurve = GetInputValue<AnimationCurve>("InputCurve", this.InputCurve);
            curve.SetCurve(animCurve);//UtilsFunctions.GetCurveAsTexture(InputCurve));
            gpuMaterial = curve._materialGPU;

            return curve;

            //Curve curve = new Curve(
            //    GetInputValue<SerializableModuleBase>("Input", this.Input));

            //var rt = GetInputValue<SerializableModuleBase>("Input", this.Input).GetSphericalValueGPU(Vector2.one * 512);

            //InputTexture = new Texture2D(rt.width, rt.height);
            //InputTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            //InputTexture.Apply();

            //animCurve = UtilsFunctions.GetCurveAsTexture(InputCurve);
            //curve.SetCurve(animCurve);//GetInputValue<AnimationCurve>("InputCurve", this.InputCurve));
            //animCurve.wrapMode = TextureWrapMode.Clamp;
            //animCurve.filterMode = FilterMode.Point;
            //animCurve.Apply();

            //curve.mathematicalCurve = InputCurve;

            //foreach (var point in InputCurve.keys)
            //{
            //    curve.Add(point.time, point.value);
            //}

            //return curve;
        }

        //[ContextMenu("RunEditor")]
        //public void RunEditor()
        //{
        //    //var _materialGPU = new Material(Shader.Find("Xnoise/Modifiers/Curve"));

        //    ////animCurve.wrapMode = TextureWrapMode.Clamp;
        //    ////animCurve.filterMode = FilterMode.Point;
        //    ////animCurve.Apply();

        //    ////InputTexture.wrapMode = TextureWrapMode.Clamp;
        //    ////InputTexture.filterMode = FilterMode.Point;
        //    ////InputTexture.Apply();

        //    //_materialGPU.SetTexture("_MainTex", InputTexture);
        //    //_materialGPU.SetTexture("_Curve", animCurve);
        //    //RenderTexture rdB = new RenderTexture((int)512, (int)512, 16);

        //    //RenderTexture.active = rdB;
        //    //Graphics.Blit(Texture2D.whiteTexture, rdB, _materialGPU);
        //    //UtilsFunctions.SaveImage(animCurve, "curve2");

        //    RenderTexture rdB = ((SerializableModuleBase)Run()).GetSphericalValueGPU(Vector2.one * 512);

        //    RenderedTexture = new Texture2D(rdB.width, rdB.height);
        //    RenderedTexture.ReadPixels(new Rect(0, 0, rdB.width, rdB.height), 0, 0);

        //    RenderedTexture.Apply();
        //}

        //public Material mat;

        //[ContextMenu("RunEditor2")]
        //public void RunEditor2()
        //{
        //    mat = new Material(Shader.Find("Xnoise/Modifiers/Curve"));

        //    mat.SetTexture("_MainTex", InputTexture);
        //    mat.SetTexture("_Curve", new Perlin().GetSphericalValueGPU(Vector2.one * 512));
        //    RenderTexture rdB = new RenderTexture(512, 512, 16);

        //    RenderTexture.active = rdB;
        //    Graphics.Blit(Texture2D.blackTexture, rdB, mat);
        //    RenderedTexture = new Texture2D(rdB.width, rdB.height);
        //    RenderedTexture.ReadPixels(new Rect(0, 0, rdB.width, rdB.height), 0, 0);

        //    RenderedTexture.Apply(); 
        //    Save();
        //}
        //string DataPath = "/";

        //public void Save()
        //{
        //    if (RenderedTexture == null) return;

        //    UnityEngine.Debug.Log(Application.dataPath + DataPath + "welslsls" + ".png");

        //    File.WriteAllBytes(Application.dataPath + DataPath + "welslsls" + ".png", RenderedTexture.EncodeToPNG());
        //}
    }
}