using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Sets
{
    [CustomEditor(typeof(UpdateRuntimeSet))]
    public class UpdateRuntimeSetEditor : Editor
    {
        private string _countString;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            UpdateRuntimeSet runtimeSet = (UpdateRuntimeSet)target;
            
            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _countString = runtimeSet.Items.Count.ToString();
                GUILayout.Label("List<IUpdateable> Items.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                int i = 0;
                foreach (var item in runtimeSet.Items)
                {
                    bool allowSceneObjects = !EditorUtility.IsPersistent (target);
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i++);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField (item as Object, typeof(Object), allowSceneObjects);
                    }
                    
                    _stringBuilder.Clear();
                }
            }
        }
    }
}
