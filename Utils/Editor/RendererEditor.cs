using LibNoise;
using XNode;
using XNodeEditor;

namespace XNoise
{
    [CustomNodeEditor(typeof(RendererNode))]
    public class RendererNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            RendererNode node = target as RendererNode;
            NodePort port = node.GetPort("Input");
            NodeEditorGUILayout.PortField(port);
            RendererInspectorUI.Draw(node.renderer, (ModuleBase)node.Run(), true);
        }
    }
}