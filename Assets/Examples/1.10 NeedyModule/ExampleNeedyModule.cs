using UnityEngine;
using System.Collections;

public class ExampleNeedyModule : MonoBehaviour
{
    public KMSelectable SolveButton;

    void Awake()
    {
        GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;
        SolveButton.OnInteract += Submit;
        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;
    }

    protected bool Submit()
    {
        GetComponent<KMNeedyModule>().OnPass();

        return false;
    }

    protected void OnNeedyActivation()
    {

    }

    protected void OnNeedyDeactivation()
    {

    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
    }
}