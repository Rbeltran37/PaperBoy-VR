using System;
using System.Reflection;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;

namespace PaperBoy.Hands
{
    [CustomEditor(typeof(HandPoseTool))]
    public class HandPoseToolEditor : Editor
    {
        private HandPoseTool _handPoseTool;
        private Transform[] fingers;
        
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        private void OnEnable()
        {
            _handPoseTool = target as HandPoseTool;
            
            Type type = typeof(HandPoseTool);
            FieldInfo fieldInfo = type.GetField(nameof(fingers), BINDING_FLAGS);
            fingers = fieldInfo.GetValue(_handPoseTool) as Transform[];
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorHelper.CreateButton(this, nameof(SetHandPose));
        }

        public void SetHandPose()
        {
            HandPoseSO handPoseSO;
            
            Type type = typeof(HandPoseTool);
            FieldInfo fieldInfo = type.GetField(nameof(handPoseSO), BINDING_FLAGS);
            handPoseSO = fieldInfo.GetValue(_handPoseTool) as HandPoseSO;
            
            for (int i = 0; i < HandPoseSO.NUM_FINGERS; i++)
            {
                handPoseSO.HandPose.FingerPoses[i].Set(fingers[i]);
                fieldInfo.SetValue(_handPoseTool, handPoseSO);
            }
            
            EditorUtility.SetDirty(handPoseSO);
        }
    }
}