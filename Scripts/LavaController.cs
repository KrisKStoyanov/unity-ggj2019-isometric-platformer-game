using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    public GameObject floor;
    public GameObject LavaPlane;
    Vector3 m_lavaStartPosition;
    Vector3 m_lavaEndPosition;
    public Transform overridePos;

    float timeToRise = 30;
    float time;
    public static bool isRising;
    public static LavaController instance;

    // Start is called before the first frame update
    void Start()
    {
        isRising = false;
        instance = this;
        MoveLavaPlane();
        ToggleLava();
    }

    // Update is called once per frame
    void Update()
    {
     
        if (isRising)
        {
            time += Time.deltaTime / timeToRise;
            transform.position = Vector3.Lerp(m_lavaStartPosition, m_lavaEndPosition, time);
        }
    }

    public void MoveLavaPlane()
    {
        if (overridePos != null)
        {
            m_lavaStartPosition = overridePos.position;
        }
        else
        {
            m_lavaStartPosition = floor.transform.position;
        }
        m_lavaStartPosition -= new Vector3(0.0f, 0.01f, 0.0f);

        if (overridePos != null)
        {
            m_lavaEndPosition = overridePos.position;
        }
        else
        {
            m_lavaEndPosition = floor.transform.position;
        }
        m_lavaEndPosition += new Vector3(0.0f, 0.50f, 0.0f);

        LavaPlane.transform.position = m_lavaStartPosition;
    }

    public void ToggleLava()
    {
        if (!isRising)
        {
            isRising = true;
        }
        else
        {
            isRising = false;
        }
    }
}
