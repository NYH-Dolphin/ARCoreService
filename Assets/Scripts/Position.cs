using System;
using UnityEngine;
using System.Collections;

namespace DefaultNamespace
{
    public class Position:MonoBehaviour
    {
        public Vector3 _orignPosition=new Vector3(0,0,0);
        public Vector3 _currentPosition;
        public GameObject cube;

        void Start()
        {
            StartCoroutine(UpdateClock());
        }
        IEnumerator UpdateClock()   
        {
            while (true)
            {
                // 每1秒更新一次时钟
                yield return new WaitForSeconds(1f);
                UpdateOneSecond();
            }
        }
        
        private void UpdateOneSecond()
        {
            cube = GameObject.Find("MovableObject(Clone)");
            if (cube != null)
            {
                _currentPosition = cube.transform.position - ARAnchorManager.OriginPosition;
                Debug.Log(_currentPosition);
            }
        }
    }
}