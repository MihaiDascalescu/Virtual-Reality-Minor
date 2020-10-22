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
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerBeam()
    {
        bool isOn = animator.GetBool("isActive");
        if (!isOn)
        {
            audioSource.PlayOneShot(openBeamAudioClip);
        }
        else
        {
            audioSource.PlayOneShot(closeBeamAudioClip);
           
        }
        animator.SetBool("isActive", !isOn);
    }
}
