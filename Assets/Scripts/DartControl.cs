using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class DartControl : MonoBehaviour
{
    public GameObject DartPrefab;
    public Transform DartThrowPoint;
    ARSessionOrigin ARSession;
    GameObject ARCam;

    Transform db;
    private GameObject DartTemp;
    private Rigidbody rb;
    
    
    private bool isDBSearched = false;
    private float dist = 0f;
    private float framespersec = 0f;
    public TMP_Text text_dist;
    public TMP_Text fps;

    private float timer = 0.0f;
    private float frames = 0.0f; 
    private float waitTime = 1.0f;

    void Start()
    {
        ARSession = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        ARCam = ARSession.transform.Find("AR Camera").gameObject;
    }

    void OnEnable()
    {
        PlaceDartboard.onPlacedObject += DartsInit;
    }

    void OnDisable()
    {
        PlaceDartboard.onPlacedObject -= DartsInit;
    }

    void Update()
    {
        timer += Time.deltaTime;
        frames += 1;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycasthit;
            if (Physics.Raycast(raycast, out raycasthit))
            {
                if (raycasthit.collider.CompareTag("dart"))
                {
                    raycasthit.collider.enabled = false;

                    DartTemp.transform.parent = ARSession.transform;

                    ThrowDart currentDartScript = DartTemp.GetComponent<ThrowDart>();
                    currentDartScript.isForceOK = true;
                
                    DartsInit();
                }
            }
        }

        if (isDBSearched)
        {
            dist = Vector3.Distance(db.position, ARCam.transform.position);
            text_dist.text = dist.ToString().Substring(0, 3);
        }

        if (timer > waitTime)
        {
            framespersec = frames/timer;
            timer -= waitTime;
            frames = 0.0f;
        }

        fps.text = framespersec.ToString();


    }

    void DartsInit()
    {
        db = GameObject.FindWithTag("dart_board").transform;
        if (db)
        {
            isDBSearched = true;
        }
        
        StartCoroutine(WaitAndSpawnDart());
    }

    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(0.1f);
        DartTemp = Instantiate(DartPrefab, DartThrowPoint.position, ARCam.transform.localRotation);
        DartTemp.transform.parent = ARCam.transform;
        rb = DartTemp.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}
