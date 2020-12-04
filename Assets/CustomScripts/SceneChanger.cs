using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void GoToStart() {  SceneManager.LoadScene(1); }
    public static void GoToTutorial() {  SceneManager.LoadScene(0); }
    
    public void GoToStartWrapper() {  GoToStart(); }

    public void GoToTutorialWrapper() { GoToTutorial();  }
   

  
}
