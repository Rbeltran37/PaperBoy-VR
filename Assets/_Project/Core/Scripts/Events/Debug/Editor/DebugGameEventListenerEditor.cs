using Core.EditorTools;
using UnityEditor;
using UnityEngine;

namespace Core.Events
{
    [CustomEditor(typeof(DebugGameEventListener))]
    [CanEditMultipleObjects]
    public class DebugGameEventListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(40);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Methods", EditorStyles.boldLabel);
                EditorHelper.CreateButton(targets, nameof(DebugGameEventListener.AddGameEventListener));
                EditorHelper.CreateButton(targets, nameof(DebugGameEventListener.PositionRandomly));
                EditorHelper.CreateButton(targets, nameof(DebugGameEventListener.RandomRemove));
            }
        }
    }
}
