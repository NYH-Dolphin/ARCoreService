using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class ChangeColor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            mr = GetComponent<MeshRenderer>();
            System.Random rd = new System.Random();
            a = rd.Next(0, 3);
            string n = Convert.ToString(a);
            color = (ColorList)Enum.Parse(typeof(ColorList), n);
        }

        public static Color RED = Color.red;
        public static Color GREEN = Color.green;
        public static Color BLUE = Color.blue;
        private int a;
        
        private MeshRenderer mr;
        private ColorList color;

        // Update is called once per frame
        void Update()
        {
            switch (color)
            {
                case ColorList.Blue:
                    mr.material.color = BLUE;
                    break;
                case ColorList.Red:
                    mr.material.color = RED;
                    break;
                case ColorList.Green:
                    mr.material.color = GREEN;
                    break;
            }
        }

        public void changeColor()
        {
            a = (a + 1) % 3;
            string n = Convert.ToString(a);
            color = (ColorList)Enum.Parse(typeof(ColorList), n);
        }

        public void changeColorRd()
        {
            System.Random rd = new System.Random();
            int last = a;
            while(last == a)
            {
                a = rd.Next(0, 3);
            }
            string n = Convert.ToString(a);
            color = (ColorList)Enum.Parse(typeof(ColorList), n);
        }

        public int getColor()
        {
            int ret = 0;
            switch (color)
            {
                case ColorList.Blue:
                    ret = 1;
                    break;
                case ColorList.Red:
                    ret = 2;
                    break;
                case ColorList.Green:
                    ret = 0;
                    break;
            }
            return -1;
        }

        public void setColor(int c)
        {
            switch (c)
            {
                case 0:
                    this.color = ColorList.Green;
                    break;
                case 1:
                    this.color = ColorList.Blue;
                    break;
                case 2:
                    this.color = ColorList.Red;
                    break;
            }
        }
    }
    public enum ColorList
    {
        Green, Blue, Red
    }
}
