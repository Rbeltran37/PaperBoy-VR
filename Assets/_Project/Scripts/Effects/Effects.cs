using UnityEngine;

namespace PaperBoy.Effects
{
    public class Effects : MonoBehaviour
    {
        [SerializeField] private EffectsSO effectsSO;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem particleSystem;

        public EffectsSO EffectsSO => effectsSO;


        private void OnValidate()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }

            if (!particleSystem)
            {
                particleSystem = GetComponent<ParticleSystem>();
                if (particleSystem)
                {
                    ParticleSystem.MainModule main = particleSystem.main;
                    main.playOnAwake = false;
                }
            }
        }

        private void Awake()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }

        private void OnEnable()
        {
            PlayAudio();
            PlayParticles();
        }

        private void OnDisable()
        {
            StopAudio();
            StopParticles();
        }

        private void PlayAudio()
        {
            effectsSO.SimpleAudioEvent.Play(audioSource);
        }

        private void StopAudio()
        {
            effectsSO.SimpleAudioEvent.Stop(audioSource);
        }

        private void PlayParticles()
        {
            if (!particleSystem)
            {
                return;
            }
            
            particleSystem.Play();
        }

        private void StopParticles()
        {
            if (!particleSystem)
            {
                return;
            }
            
            particleSystem.Stop();
        }
    }
}