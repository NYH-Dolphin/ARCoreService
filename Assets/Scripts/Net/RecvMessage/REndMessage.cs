using LitJson;

namespace NetworkTools
{
    public class REndMessage : RecvMessage
    {
        public bool end;
        public override void SetJsonData(JsonData data)
        {
            end = bool.Parse(data["end"].ToString());
        }
    }
}