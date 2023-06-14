using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class PointAxis : MonoBehaviour
    {

        public static Color ColorNormalX = new Color32(255, 100, 100, 255);
        public static Color ColorNormalY = new Color32(100, 255, 100, 255);
        public static Color ColorNormalZ = new Color32(100, 100, 255, 255);

        public static Color ColorBeSelectedX = Color.red;
        public static Color ColorBeSelectedY = Color.green;
        public static Color ColorBeSelectedZ = Color.blue;

        public bool IsBeSelected;
        [SerializeField] private PointAxis_Type curPAType;

        private MeshRenderer mr;
        private Color colorNormal;
        private Color colorBeSelected;

        private bool isMouseDrag = false;
        private Vector3 screenPosition;
        private Vector3 offset;
        private Transform parentTransform;

        private Vector3 dragBeforeGameObjPos;

        // Use this for initialization
        void Start()
        {

            parentTransform = transform.parent.parent;
            mr = GetComponent<MeshRenderer>();
            switch (curPAType)
            {
                case PointAxis_Type.Axis_X:
                    colorNormal = ColorNormalX;
                    colorBeSelected = ColorBeSelectedX;
                    break;
                case PointAxis_Type.Axis_Y:
                    colorNormal = ColorNormalY;
                    colorBeSelected = ColorBeSelectedY;
                    break;
                case PointAxis_Type.Axis_Z:
                    colorNormal = ColorNormalZ;
                    colorBeSelected = ColorBeSelectedZ;
                    break;
            }

        }

        // Update is called once per frame
        void Update()
        {

            mr.material.color = IsBeSelected ? colorBeSelected : colorNormal;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                dragBeforeGameObjPos = parentTransform.transform.position;
                screenPosition = Camera.main.WorldToScreenPoint(dragBeforeGameObjPos);
                offset = transform.position -
                         Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                             screenPosition.z));
                isMouseDrag = true;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isMouseDrag && IsBeSelected)
                {
                    //Debug.Log("Start Dragging Axis");
                    Vector3 currentScreenSpace =
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
                    float tempLength = Vector3.Distance(currentPosition, transform.position);
                    Vector3 tempPos = Vector3.zero;
                    switch (curPAType)
                    {
                        case PointAxis_Type.Axis_X:
                            tempPos = Vector3.Project(currentPosition, transform.forward) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.up) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.right);
                            break;
                        case PointAxis_Type.Axis_Y:
                            tempPos = Vector3.Project(currentPosition, transform.up) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.right) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.forward);
                            break;
                        case PointAxis_Type.Axis_Z:
                            tempPos = Vector3.Project(currentPosition, transform.right) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.up) +
                                      Vector3.Project(dragBeforeGameObjPos, transform.forward);
                            break;
                    }

                    parentTransform.transform.position = tempPos;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isMouseDrag = false;
            }

        }
    }

    public enum PointAxis_Type
    {
        Axis_X,
        Axis_Y,
        Axis_Z
    }
}