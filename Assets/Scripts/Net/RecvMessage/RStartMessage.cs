using LitJson;

namespace NetworkTools
{
    public class RStartMessage : RecvMessage
    {
        public bool start;
        

        public override void SetJsonData(JsonData data)
        {
            start = bool.Parse(data["start"].ToString());
        }
    }
}