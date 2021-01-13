using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private Animator controller;
    [SerializeField] private string boolName;
    void Start()
    {
        controller = GetComponent<Animator>();
    }
    

    public void PlayTriggerAnimation()
    {
        controller.SetBool(boolName,true);
    }
}
