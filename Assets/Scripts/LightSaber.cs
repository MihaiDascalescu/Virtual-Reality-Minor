using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaber : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    public AudioClip openBeamAudioClip;
    public AudioClip closeBeamAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TriggerBeam()
    {
        bool isOn = animator.GetBool("isActive");
        if (!isOn)
        {
            _audioSource.PlayOneShot(openBeamAudioClip);
        }
        else
        {
            _audioSource.PlayOneShot(closeBeamAudioClip);
           
        }
        animator.SetBool("isActive", !isOn);
    }
}
