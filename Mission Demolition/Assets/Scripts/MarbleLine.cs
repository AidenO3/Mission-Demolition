using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MarbleLine : MonoBehaviour
{
    static List<MarbleLine> PROJ_LINES = new List<MarbleLine>();
    private const float DIM_MULT = 0.6f;

    private LineRenderer _line;
    private bool _drawing = true;
    private Marble _marble;


    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);
        _marble = GetComponentInParent<Marble>();
        ADD_LINE(this);
    }

    private void OnDestroy()
    {
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(MarbleLine newLine)
    {
        Color col;

        //dim previous lines
        foreach(MarbleLine ml in PROJ_LINES)
        {
            col = ml._line.startColor;
            col = col * DIM_MULT;
            ml._line.startColor = ml._line.endColor = col;
        }
        PROJ_LINES.Add(newLine);
    }

    void FixedUpdate()
    {
        if (_drawing)
        {
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, transform.position);
            if(_marble != null)
            {
                if (!_marble.awake)
                {
                    _drawing = false;
                    _marble = null;
                }
            }
        }
    }
}
