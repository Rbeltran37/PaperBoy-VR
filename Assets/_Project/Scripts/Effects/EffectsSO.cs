using Core.AudioEvents;
using UnityEngine;

namespace PaperBoy.Effects
{
    [CreateAssetMenu(fileName = "EffectsSO_", menuName = "ScriptableObjects/PaperBoy/Effects/EffectSO", order = 0)]
    public class EffectsSO : ScriptableObject
    {
        public bool IsParentedToHandler;
        [Range(.5f, 5f)] public float Lifetime;
        public SimpleAudioEvent SimpleAudioEvent;
    }
}