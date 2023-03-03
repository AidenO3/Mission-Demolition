using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Marble : MonoBehaviour
{
    private bool aboutToSleep = false;
    const int LOOKBACK_COUNT = 10;
    static List<Marble> MARBLES = new List<Marble>();

    [SerializeField]
    private bool _awake = true;
    public bool awake
    {
        get { return _awake; }
        private set { _awake = value; }
    }

    private Vector3 prevPos;

    private List<float> deltas = new List<float>();
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000, 1000, 0);
        deltas.Add(1000);
        MARBLES.Add(this);
        aboutToSleep = false;
    }

    private void FixedUpdate()
    {
        if (rigid.isKinematic || !awake)
            return;
        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while(deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach(float f in deltas)
        {
            if (f > maxDelta)
                maxDelta = f;
        }

        if (maxDelta <= Physics.sleepThreshold && aboutToSleep == false)
        {
            aboutToSleep = true;
            Invoke("GoToSleep", 2f);
        }
    }

    private void GoToSleep()
    {
        awake = false;
        rigid.Sleep();
    }

    private void OnDestroy()
    { 
        MARBLES.Remove(this);
    }

    static public void DESTROY_MARBLES()
    {
        foreach(Marble m in MARBLES)
        {
            Destroy(m.gameObject);
        }
    }

}
