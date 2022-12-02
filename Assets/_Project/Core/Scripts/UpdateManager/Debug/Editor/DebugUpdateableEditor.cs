using System.Reflection;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;

namespace Core.UpdateManager
{
    [CustomEditor(typeof(DebugUpdateable))]
    [CanEditMultipleObjects]
    public class DebugUpdateableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(40);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorHelper.CreateButton(targets, nameof(DebugUpdateable.PublicCall));
                EditorHelper.CreateButton(targets, nameof(DebugUpdateable.PositionRandomly));
                EditorHelper.CreateButton(targets, nameof(DebugUpdateable.RandomRemove));
            }
        }
    }
}
