using XNode;
using XNodeEditor;

namespace XNoise
{
    [CustomNodeEditor(typeof(RootModuleBase))]
    public class RootModuleBaseEditor : XNodeEditor.NodeEditor
    {
        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            RootModuleBase node = (RootModuleBase)target;
            NodePort port = node.GetPort("Input");
            NodeEditorGUILayout.PortField(port);
        }
    }
}