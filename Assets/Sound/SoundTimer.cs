using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimer : MonoBehaviour
{
    [SerializeField] private float soundTimer = 5.0f;
    private AudioSource source;
    [SerializeField]private AudioClip playedSound;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer >= 0)
        {
            source.PlayOneShot(playedSound);
            soundTimer = 0;
        }
    }
}
