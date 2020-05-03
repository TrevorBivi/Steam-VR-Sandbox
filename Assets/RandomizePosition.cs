using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Random.Range(-4f,4f),0.5f,Random.Range(-4f,4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
