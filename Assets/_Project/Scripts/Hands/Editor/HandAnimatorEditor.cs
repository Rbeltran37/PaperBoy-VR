using Core.EditorTools;
using UnityEditor;

namespace PaperBoy.Hands
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HandAnimator))]
    public class HandAnimatorEditor : Editor
    {
        private const string IDLE = "Idle";
        private const string GRAB = "Grab";
        

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(targets, IDLE);
            EditorHelper.CreateButton(targets, GRAB);
        }
    }
}