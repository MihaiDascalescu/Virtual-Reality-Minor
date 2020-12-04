using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonColor : MonoBehaviour
{
    [SerializeField] private Color buttonColor;
    [SerializeField] private Button button;
    [SerializeField]private float timeBeforeColorIsBack;
    
    public void ChangeColor()
    {
        var buttonColors = button.colors;
        buttonColors.normalColor = buttonColor;
        StartCoroutine(ChangeColorBack());
    }

    private IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(timeBeforeColorIsBack);
        var buttonColors = button.colors;
        buttonColors.normalColor = Color.white;
    }
}
