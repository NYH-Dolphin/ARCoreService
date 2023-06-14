using System;
using UnityEngine;

namespace NetworkTools
{
    public class prefab
    {
        public String name;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public int color;

        public prefab()
        {
            //todo: color
            this.color = 0;
        }
    }
}