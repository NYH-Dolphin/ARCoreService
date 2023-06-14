using LitJson;
using UnityEngine;

namespace NetworkTools
{
    public abstract class SendMessage
    {
        
        public virtual string ToJsonString()
        {
            return JsonMapper.ToJson(this);
        }

        public abstract WWWForm ToWWWForm();
    }
}