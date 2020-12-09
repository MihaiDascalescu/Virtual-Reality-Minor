using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using FloatParameter = UnityEngine.Rendering.FloatParameter;
using Vignette = UnityEngine.Rendering.PostProcessing.Vignette;
using ColorAdjustments = UnityEngine.Rendering.Universal.ColorAdjustments;
using ColorParameter = UnityEngine.Rendering.ColorParameter;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{

    private Health health;
    [SerializeField] private Volume processVolume;
    private bool isDead = false;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    [SerializeField] private float vignetteIntensity = 0.1f;
    [SerializeField] private float screenChangeTimer = 0.5f;
    

    private void Awake()
    {
        health = GetComponent<Health>();
        colorAdjustments = (ColorAdjustments) processVolume.profile.components[5] ;
        vignette = processVolume.GetComponent<Vignette>(); //processVolume.profile.components[4] ;
    }

    private void OnEnable()
    {
        health.Died += Die;
        health.HealthNegativelyChanged += OnHealthNegativelyChanged;
        health.HealthPositivelyChanged += OnHealthPositivelyChanged;
    }

    private void OnDisable()
    {
        health.Died -= Die;
        health.HealthNegativelyChanged -= OnHealthNegativelyChanged;
        health.HealthPositivelyChanged -= OnHealthPositivelyChanged;
    }

    public void Die()
    {
        Debug.Log("YOU DIED");
    }

    public bool IsPlayerDead()
    {
        if (health.currentHealth <= 0)
        {
            isDead = true;
        }

        return isDead;
    }

    private void OnHealthNegativelyChanged(int amount)
    {
        FloatParameter vignetteIncrease = new FloatParameter(vignetteIntensity,false);
        vignette.intensity.value += vignetteIncrease.value;
        if (vignette.intensity.value >= 0.9f)
        {
            vignette.intensity.value = 0.9f;
        }

        StartCoroutine(UpdateRedScreenColor());
    }

    private IEnumerator UpdateRedScreenColor()
    {
        colorAdjustments.colorFilter = new ColorParameter(Color.red);
        yield return new WaitForSeconds(screenChangeTimer);
        colorAdjustments.colorFilter = new ColorParameter(Color.white);
    }

    private void OnHealthPositivelyChanged(int amount)
    {
          FloatParameter vignetteIncrease = new FloatParameter(vignetteIntensity,false);
          vignette.intensity.value -= vignetteIncrease.value;
          if (vignette.intensity.value <= 0.4f)
          {
              vignetteIntensity = 0.4f;
          }

          StartCoroutine(UpdateGreenScreenColor());

    }

    private IEnumerator UpdateGreenScreenColor()
    {
        colorAdjustments.colorFilter = new ColorParameter(Color.green);
        yield return new WaitForSeconds(screenChangeTimer);
        colorAdjustments.colorFilter = new ColorParameter(Color.white);
    }
}
