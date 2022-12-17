using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]

public class ARPlaneController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    // Start is called before the first frame update
    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        PlaceDartboard.onPlacedObject += DisablePlaneDetection;
    }

    void OnDisable()
    {
        PlaceDartboard.onPlacedObject -= DisablePlaneDetection;
    }

    void DisablePlaneDetection(){
        SetAllPlanesActive(false);
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
    }
}
