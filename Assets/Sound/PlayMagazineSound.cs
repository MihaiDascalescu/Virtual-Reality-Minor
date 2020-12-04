using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMagazineSound : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    [SerializeField] private AudioClip sound;
    void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    // Update is called once per frame
    public void PlaySound()
    {
        soundPlayer.PlaySoundOneShot(sound);
    }
}
