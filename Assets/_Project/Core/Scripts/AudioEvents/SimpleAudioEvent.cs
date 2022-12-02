using UnityEngine;
using Core.Debug;
using Random = UnityEngine.Random;

namespace Core.AudioEvents
{
    [CreateAssetMenu(fileName = "SimpleAudioEvent_", menuName="ScriptableObjects/Core/AudioEvent/SimpleAudioEvent")]
    public class SimpleAudioEvent : ScriptableObject
    {
        [SerializeField] private bool isLooping;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private RangedFloat volume;
        [MinMaxFloatRange(0, 3)]
        [SerializeField] private RangedFloat pitch;
    
        private int _numClips;
    
        public void Play(AudioSource audioSource)
        {
            if (clips == null)
            {
                CustomLogger.Error(nameof(Play), $"{nameof(clips)} is null.", this);
                return;
            }
            
            if (_numClips == 0) _numClips = clips.Length;
            if (_numClips == 0)
            {
                CustomLogger.Error(nameof(Play), $"{nameof(_numClips)} is 0.", this);
                return;
            }
    
            if (!audioSource)
            {
                CustomLogger.Error(nameof(Play), $"{nameof(audioSource)} is null.", this);
                return;
            }
    
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
            audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            audioSource.loop = isLooping;
            audioSource.Play();
        }
        
        public void PlayWithPitch(AudioSource audioSource, float pitchValue)
        {
            if (clips.Length == 0) return;
    
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
            audioSource.pitch = pitchValue;
            audioSource.loop = isLooping;
            audioSource.Play();
        }
    
        public void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}