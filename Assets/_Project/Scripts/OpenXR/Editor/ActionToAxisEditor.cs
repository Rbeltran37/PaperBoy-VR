using System;
using System.Reflection;
using Core.EditorTools;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

namespace PaperBoy.OpenXR
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActionToAxis))]
    public class ActionToAxisEditor : ActionToControlEditor
    {
        private ActionToAxis _actionToAxis;
        private Slider _slider;
        private Type _type;
        private FieldInfo _fieldInfoSlider;


        protected override void OnEnable()
        {
            base.OnEnable();
            
            _actionToAxis = target as ActionToAxis;

            _type = typeof(ActionToAxis);

            _fieldInfoSlider = _type.GetField(nameof(_slider), BINDING_FLAGS);
            _slider = _fieldInfoSlider.GetValue(_actionToAxis) as Slider;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(this, nameof(SetSlider));
        }

        public void SetSlider()
        {
            _slider = _actionToAxis.GetComponentInChildren<Slider>();
            
            _fieldInfoSlider.SetValue(_actionToAxis, _slider);
        }
    }
}