using System;
using System.Reflection;
using Core.Debug;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;
using Core.EditorTools;

namespace PaperBoy.OpenXR
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActionToControl))]
    public class ActionToControlEditor : Editor
    {
        private ActionToControl _actionToControl;
        private InputActionReference _actionReference;
        private Text _text;
        private Type _type;
        private FieldInfo _fieldInfoText;
        private FieldInfo _fieldInfoActionReference;
        
        protected const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        protected virtual void OnEnable()
        {
            _actionToControl = target as ActionToControl;

            _type = typeof(ActionToControl);
            
            _fieldInfoActionReference = _type.GetField(nameof(_actionReference), BINDING_FLAGS);
            _actionReference = _fieldInfoActionReference.GetValue(_actionToControl) as InputActionReference;
            
            _fieldInfoText = _type.GetField(nameof(_text), BINDING_FLAGS);
            _text = _fieldInfoText.GetValue(_actionToControl) as Text;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(this, nameof(SetText));
        }

        public void SetText()
        {
            _text = _actionToControl.GetComponentInChildren<Text>();
            
            _fieldInfoText.SetValue(_actionToControl, _text);
        }
    }
}

