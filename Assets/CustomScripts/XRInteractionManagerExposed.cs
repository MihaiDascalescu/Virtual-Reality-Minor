using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.Interaction.Toolkit;
using System.Reflection;


public class XRInteractionManagerExposed : MonoBehaviour
{
    private static XRInteractionManagerExposed instance = null;
    private XRInteractionManager manager = null;
    private MethodInfo unregisterInteractable = null;

    public static XRInteractionManagerExposed GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        instance = this;
        manager = this.GetComponent<XRInteractionManager>();

        System.Type t = typeof(XRInteractionManager);
		
        MethodInfo[] methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (MethodInfo mi in methods)
        {
            if (mi.Name == "UnregisterInteractable")
            {
                unregisterInteractable = mi;
                break; //leave the loop
            }
        }
    }

    public void UnregisterInteractable(XRBaseInteractable interactable)
    {
        object[] args = new Object[] { interactable };
        unregisterInteractable.Invoke(manager, args);
    }
}

