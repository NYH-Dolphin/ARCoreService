using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setActiveAxis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CH = gameObject.transform.GetComponentsInChildren<Transform>(true);
        target = CH[2].gameObject;
        
        Debug.Log(target.name);
    }

    private GameObject target;
    private bool active = true;
    private Transform[] CH;

    public void onClick()
    {   
        active = !active;
        target.SetActive(active);
        
    }

}
