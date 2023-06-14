using Unity.XR.OpenVR;
using UnityEngine;

namespace NetworkTools
{
    public class SMessage : SendMessage
    {
        public string CMD;
        public string prefab_name;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public int color;


        public string Vector2str(Vector3 v)
        {
            return "[" + v[0] + "," + v[1] + "," + v[2] + "]";
        }

        public SMessage(string CMD, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale, int color)
        {
            this.CMD = CMD;
            this.prefab_name = prefabName;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.color = color;
        }

        public override string ToJsonString()
        {
            return
                $"CMD={CMD}&prefab_name={prefab_name}&position={Vector2str(position)}&rotation={Vector2str(rotation)}&scale={Vector2str(scale)}&color={color}";
        }

        public override WWWForm ToWWWForm()
        {
            WWWForm form = new WWWForm();
            form.AddField("CMD", CMD);
            form.AddField("prefab_name", prefab_name);
            form.AddField("position", Vector2str(position));
            form.AddField("rotation", Vector2str(rotation));
            form.AddField("scale", Vector2str(scale));
            return form;
        }
    }
}