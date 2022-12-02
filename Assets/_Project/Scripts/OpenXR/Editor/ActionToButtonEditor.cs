using System;
using System.Reflection;
using Core.EditorTools;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

namespace PaperBoy.OpenXR
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActionToButton))]
    public class ActionToButtonEditor : ActionToControlEditor
    {
        private ActionToButton _actionToButton;
        private Image _image;
        private Type _type;
        private FieldInfo _fieldInfoImage;


        protected override void OnEnable()
        {
            base.OnEnable();
            
            _actionToButton = target as ActionToButton;

            _type = typeof(ActionToButton);

            _fieldInfoImage = _type.GetField(nameof(_image), BINDING_FLAGS);
            _image = _fieldInfoImage.GetValue(_actionToButton) as Image;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(this, nameof(SetImage));
        }

        public void SetImage()
        {
            _image = _actionToButton.GetComponentInChildren<Image>();
            
            _fieldInfoImage.SetValue(_actionToButton, _image);
        }
    }
}

