using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCam : MonoBehaviour
{
    public static ControlCam M_Instance
    {
        get
        {
            if(null==_instance)
            {
                _instance = FindObjectOfType<ControlCam>();
            }
            return _instance;
        }
    }

    public bool M_IsCanControl
    {
        get
        {
            return isCanControl;
        }
    }

    public static ControlCam _instance;
    private bool isMouseDrag = false;
    private Vector3 lastMousePos;
    [SerializeField]
    private Vector3 offset;

    private bool isCanControl = true;
    private PointAxis PA;
    private PointEntity PE;
    // Use this for initialization
    void Start()
    {

    }

    public void setCanControl()
    {
        this.isCanControl = !this.isCanControl;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.LeftControl))
        {
            isCanControl = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCanControl = false;
        }*/
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            lastMousePos = Input.mousePosition;
            isMouseDrag = true;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (isMouseDrag)
            {
                offset = Input.mousePosition - lastMousePos;
                transform.RotateAround(Vector3.zero, Vector3.up, offset.x);
                transform.RotateAround(Vector3.zero, Vector3.right, offset.y);
                lastMousePos = Input.mousePosition;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isMouseDrag = false;
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {   
            Reset_SelectedState();
            /*isCanControl = false;*/
        }
        if (!isCanControl)
        {
            Reset_SelectedState();
            return;
        }
        Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(tempRay, out hit, 2000))
        {
            PointEntity tempHitPE = hit.collider.GetComponent<PointEntity>();
            PointAxis tempHitPA = hit.collider.GetComponent<PointAxis>();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (null != tempHitPE)
                {
                    Reset_SelectedState();
                    PE = tempHitPE;
                    PE.IsBeSelected = true;
                }
                else if (null != tempHitPA)
                {
                    Reset_SelectedState();
                    PA = tempHitPA;
                    PA.IsBeSelected = true;
                }
            }
        }
    }

    private void Reset_SelectedState()
    {
        if (null != PE)
        {
            PE.IsBeSelected = false;
            PE = null;
        }
        if (null != PA)
        {
            PA.IsBeSelected = false;
            PA = null;
        }
    }
}
