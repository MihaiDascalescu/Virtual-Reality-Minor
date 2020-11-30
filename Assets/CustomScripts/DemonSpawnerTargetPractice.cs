using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using StateMachineScripts;
using UnityEngine;
using System.Linq;

public class DemonSpawnerTargetPractice : MonoBehaviour
{
    [SerializeField] private GameObject[] Demons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Called from UnityEvent
    public void OnPressed()
    {
        for (int i = 0; i < Demons.Length; i++)
        {
            
            GameObject newDemon = Demons[i];
            if (newDemon.activeSelf)
            {
                return;
            }
            newDemon.SetActive(true);
            newDemon.GetComponent<PracticeTargetDemon>().health.SetHealthToFull();
        }
    }
    
    
}
