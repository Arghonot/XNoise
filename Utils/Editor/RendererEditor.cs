using LibNoise;
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
            RendererInspectorUI.Draw(node.renderer, (ModuleBase)node.Run(), true);
        }
    }
}