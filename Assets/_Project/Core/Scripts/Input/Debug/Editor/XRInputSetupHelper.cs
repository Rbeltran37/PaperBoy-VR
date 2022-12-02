using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Debug;
using Core.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class XRInputSetupHelper : MonoBehaviour
    {
        private const string INPUT_ACTION_ASSET_PATH = "Assets/Samples/OpenXR Plugin/1.2.8/Controller/ActionMap/InputActions.inputactions";
        private const string LEFT_PATH = "Assets/_Project/ScriptableObjects/Input/InputEvents/XR/LeftHand/";
        private const string RIGHT_PATH = "Assets/_Project/ScriptableObjects/Input/InputEvents/XR/RightHand/";
        private const string LEFT = "InputEvent_LeftHand_";
        private const string RIGHT = "InputEvent_RightHand_";
        private const string ASSET = ".asset";
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

        
        [ContextMenu(nameof(SetupHand))]
        public void SetupHand()
        {
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var inputActionHandler = child.GetComponent<InputActionHandler>();
                if (!inputActionHandler)
                {
                    inputActionHandler = child.gameObject.AddComponent<InputActionHandler>();
                }

                Type type = typeof(InputActionReference);
                FieldInfo fieldInfo = type.GetField("inputActionReference", BINDING_FLAGS);
                InputActionReference inputActionReference = fieldInfo.GetValue(inputActionHandler) as InputActionReference;
                
                if (!inputActionReference)
                {
                    InputActionAsset inputActionAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(INPUT_ACTION_ASSET_PATH);
                    CustomLogger.Debug(nameof(SetupHand), $"{inputActionAsset}");        //TODO maybe remove
                    return;
                }

                string currentName;
                string currentHand;
                string currentPath;

                bool isLeftHand = transform.name.StartsWith("LeftHand");
                currentHand = isLeftHand ? LEFT : RIGHT;
                currentName = currentHand + child.name + "_";
                currentPath = isLeftHand ? LEFT_PATH : RIGHT_PATH;

                InputEvent startedInputEvent = AssetDatabase.LoadAssetAtPath<InputEvent>(currentPath + currentName + "Started" + ASSET);
                inputActionHandler.SetStartedInputEvent(startedInputEvent);
                
                InputEvent performedInputEvent = AssetDatabase.LoadAssetAtPath<InputEvent>(currentPath + currentName + "Performed" + ASSET);
                inputActionHandler.SetPerformedInputEvent(performedInputEvent);
                
                InputEvent canceledInputEvent = AssetDatabase.LoadAssetAtPath<InputEvent>(currentPath + currentName + "Canceled" + ASSET);
                inputActionHandler.SetCanceledInputEvent(canceledInputEvent);

                int childChildCount = child.childCount;
                if (childChildCount != 3)
                {
                    var started = new GameObject("Started");
                    started.transform.SetParent(child);
                    var performed = new GameObject("Performed");
                    performed.transform.SetParent(child);
                    var canceled = new GameObject("Canceled");
                    canceled.transform.SetParent(child);
                }

                SetupInputListeners(child, 0, startedInputEvent);
                SetupInputListeners(child, 1, performedInputEvent);
                SetupInputListeners(child, 2, canceledInputEvent);
            }
        }

        private static void SetupInputListeners(Transform child, int currentIndex, InputEvent inputEvent)
        {
            Transform childChild;
            EditorInputListener editorInputListener;
            RuntimeInputListener runtimeInputListener;
            childChild = child.GetChild(currentIndex);
            editorInputListener = childChild.GetComponent<EditorInputListener>();
            if (!editorInputListener)
            {
                editorInputListener = childChild.gameObject.AddComponent<EditorInputListener>();
            }

            editorInputListener.inputEvent = inputEvent;

            runtimeInputListener = childChild.GetComponent<RuntimeInputListener>();
            if (!runtimeInputListener)
            {
                runtimeInputListener = childChild.gameObject.AddComponent<RuntimeInputListener>();
            }

            runtimeInputListener.inputEvent = inputEvent;
        }
    }
}
