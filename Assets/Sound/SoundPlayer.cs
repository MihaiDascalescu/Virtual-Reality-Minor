using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySoundOneShot(AudioClip soundToPlay)
    {
        source.PlayOneShot(soundToPlay);
    }
  
}
