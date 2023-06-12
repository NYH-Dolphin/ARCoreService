using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEntity : MonoBehaviour {

    public bool IsBeSelected = false;
    private bool isMouseDrag = false;
    private Vector3 screenPosition;
    private Vector3 offset;

    private Color normalColor;
    private Color beSelectedColor;
    private MeshRenderer mr;
    // Use this for initialization
    void Start () {
        mr = GetComponent<MeshRenderer>();
        normalColor = mr.material.color;
        beSelectedColor = new Color32(210, 137, 242,255);
	}
	
	// Update is called once per frame
	void Update () {

        mr.material.color = IsBeSelected ? beSelectedColor : normalColor;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            isMouseDrag = true;
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (isMouseDrag&&IsBeSelected)
            {
                //Debug.Log("开始拖拽了");
                Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
                gameObject.transform.position = currentPosition;
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMouseDrag = false;
        }
		
	}
}
