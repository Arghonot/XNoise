using LibNoise;
using XNodeEditor;

namespace XNoise
{
    [CustomNodeEditor(typeof(RendererNode))]
    public class RendererNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            base.OnBodyGUI();
            RendererNode node = target as RendererNode;
            CustomGraph.NodeBaseEditor.DrawNonInstantiableTypeInputPorts(target);
            RendererInspectorUI.Draw(node.renderer, (ModuleBase)node.Run(), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}