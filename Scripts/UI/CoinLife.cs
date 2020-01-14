using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLife : MonoBehaviour
{

    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = Time.time;
        gameObject.transform.localScale *= 5;
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, -1000, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > currentTime + 1)
        {
            Destroy(gameObject);
        }
    }
}
