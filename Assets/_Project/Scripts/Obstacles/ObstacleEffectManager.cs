using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject obstacleGO;
    [SerializeField] private GameObject effectGO;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource effectAudioSource;

    [ContextMenu("Initialize Effect")]
    public void InitializeEffect()
    {
        //obstacleGO.SetActive(false);
        effectGO.SetActive(true);
        particleSystem.Play();
        
        if(effectAudioSource)
            effectAudioSource.Play();
    }
}
