using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightswitchController : MonoBehaviour
{
    [SerializeField] private GameObject lightSwitchPrefab;

    [HideInInspector] public bool electricity = true;

    private bool _isActive = false;

    public void InteractSwitch()
    {
        if (electricity)
        {
            _isActive = !_isActive;

            lightSwitchPrefab.SetActive(_isActive);
        }
    }
}
