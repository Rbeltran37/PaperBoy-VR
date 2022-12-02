using UnityEditor;
using UnityEngine;

namespace Core.AudioEvents
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SimpleAudioEvent))]
    public class SimpleAudioEventEditor : Editor
    {
        private SimpleAudioEvent _simpleAudioEvent;
        private GameObject _gameObject;
        private AudioSource _audioSource;


        private void OnEnable()
        {
            _simpleAudioEvent = target as SimpleAudioEvent;
            if (!_gameObject)
            {
                _gameObject = new GameObject("[Editor_GameObject]");
                _gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
            
            if (!_audioSource)
            {
                _audioSource = _gameObject.AddComponent<AudioSource>();
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
            }
            
            //DestroyImmediate(_gameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button(nameof(_simpleAudioEvent.Play)))
            {
                _simpleAudioEvent.Play(_audioSource);
            }
        }
    }
}
