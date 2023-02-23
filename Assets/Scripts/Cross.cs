using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public float delay = 1f;
    void Start()
    {
        Destroy(this.gameObject, delay);
    }
}
