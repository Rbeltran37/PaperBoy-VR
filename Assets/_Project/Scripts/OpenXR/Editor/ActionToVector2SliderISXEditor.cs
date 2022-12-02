using System;
using System.Reflection;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

namespace PaperBoy.OpenXR
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActionToVector2SliderISX))]
    public class ActionToVector2SliderISXEditor : Editor
    {
        public Slider xAxisSlider;
        public Slider yAxisSlider;
        
        private ActionToVector2SliderISX _actionToVector2SliderIsx;
        private Type _type;
        private FieldInfo _fieldInfoSliderX;
        private FieldInfo _fieldInfoSliderY;


        protected void OnEnable()
        {
            _actionToVector2SliderIsx = target as ActionToVector2SliderISX;

            _type = typeof(ActionToVector2SliderISX);
            
            _fieldInfoSliderX = _type.GetField(nameof(xAxisSlider), BindingFlags.Instance | BindingFlags.Public);
            xAxisSlider = _fieldInfoSliderX.GetValue(_actionToVector2SliderIsx) as Slider;
            
            _fieldInfoSliderY = _type.GetField(nameof(yAxisSlider), BindingFlags.Instance | BindingFlags.Public);
            yAxisSlider = _fieldInfoSliderX.GetValue(_actionToVector2SliderIsx) as Slider;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(this, nameof(SetSliders));
        }
        
        public void SetSliders()
        {
            Slider[] sliders = _actionToVector2SliderIsx.GetComponentsInChildren<Slider>();
            xAxisSlider = sliders[0];
            yAxisSlider = sliders[1];
            
            _fieldInfoSliderX.SetValue(_actionToVector2SliderIsx, xAxisSlider);
            _fieldInfoSliderY.SetValue(_actionToVector2SliderIsx, yAxisSlider);
        }
    }
}