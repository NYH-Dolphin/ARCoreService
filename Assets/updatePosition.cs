using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updatePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CH = gameObject.transform.GetComponentsInChildren<Transform>(true);
        posTarget = CH[1];
        posOrigin = CH[2];
        
    }

    private Transform posOrigin;
    private Transform posTarget;
    private Transform[] CH;
    // Update is called once per frame
    void Update()
    {
        
        posTarget.position = posOrigin.position;
    }
}
