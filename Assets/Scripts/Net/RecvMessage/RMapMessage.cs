using LitJson;

namespace NetworkTools
{
    public class RMapMessage : RecvMessage
    {
        public int height;
        public int width;
        public int[,] matrix;
        public bool finish;


        public override void SetJsonData(JsonData data)
        {
            height = int.Parse(data["height"].ToString());
            width = int.Parse(data["width"].ToString());
            finish = bool.Parse(data["finish"].ToString());
            JsonData matrixData = data["matrix"];
            matrix = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = int.Parse(matrixData[i][j].ToString());
                }
            }
        }
    }
}