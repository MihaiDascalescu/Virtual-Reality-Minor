using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using StateMachineScripts;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class DemonSpawnerTargetPractice : MonoBehaviour
{
    [FormerlySerializedAs("DemonsStationary")] [SerializeField] private GameObject[] demonsStationary;
    [FormerlySerializedAs("MovableDemons")] [SerializeField] private GameObject[] movableDemons;

    public void OnPressedStationary()
    {
        foreach (var newDemon in demonsStationary)
        {
            if (newDemon.activeInHierarchy)
            {
                continue;
            }
            newDemon.SetActive(true);
            newDemon.GetComponent<PracticeTargetDemon>().health.SetHealthToFull();
        }

        foreach (var movableDemon in movableDemons)
        {
            var movableDemonTargetPractice = movableDemon.GetComponent<MovableTarget>();
            if (!movableDemon.activeInHierarchy)
            {
                continue;
            }
            movableDemonTargetPractice.health.SetHealthToFull();
            movableDemon.SetActive(false);
            foreach (var t in movableDemonTargetPractice.walkpoints)
            {
                movableDemon.transform.position = t.transform.position;
            }
        }
    }

    public void OnPressedMovable()
    {
        foreach (var newDemon  in movableDemons)
        {
            if (newDemon.activeInHierarchy)
            {
                continue;
            }
            var newDemonTargetPractice = newDemon.GetComponent<MovableTarget>();
            
            newDemon.SetActive(true);
            newDemonTargetPractice.health.SetHealthToFull();
            foreach (var t in newDemonTargetPractice.walkpoints)
            {
                newDemon.transform.position = t.transform.position;
            }
        }

        foreach (var stationaryDemon in demonsStationary)
        {
            var stationaryDemonTargetPractice = stationaryDemon.GetComponent<PracticeTargetDemon>();
            if (!stationaryDemon.activeInHierarchy)
            {
                continue;
            }
            stationaryDemonTargetPractice.health.SetHealthToFull();
            stationaryDemon.SetActive(false);
        }
    }
    
    
}
