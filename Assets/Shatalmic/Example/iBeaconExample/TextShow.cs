
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextShow : MonoBehaviour
{

    private Text m_Text;
    private iBeaconExampleScript i;
    // [SerializeField]
    // private GetCompassData CompassData;
    
    private double[][] Coord=new double[3][];
    private double Coord_x, Coord_y;
    private double a,b,c,d,e,f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponent<Text>();
        i = GetComponentInParent<iBeaconExampleScript>();
        Coord[0] = new double[2];
        Coord[1] = new double[2];
        Coord[2] = new double[2];
        Coord[0][0] = 0;
        Coord[0][1] = 0;
        Coord[1][0] = 0;
        Coord[1][1] = 2;
        Coord[2][0] = 2;
        Coord[2][1] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // location();
        m_Text.text = i.distance_test[0] + "  \n" + i.distance_test[1] + "  \n" + i.distance_test[2]+"\n"+i.Coord_x+" "+i.Coord_z;
        // Debug.Log(CompassData.GetDir());
        // Debug.Log(i.distance_test);
    }

    private void location()
    {
        Debug.Log(Coord[0][0]);
        a=Coord[0][0]-Coord[2][0];
        b=Coord[0][1]-Coord[2][1];
        c= Math.Pow(Coord[0][0], 2) - Math.Pow(Coord[2][0], 2) + Math.Pow(Coord[0][1], 2) - Math.Pow(Coord[2][1], 2) + Math.Pow(Double.Parse(i.distance_test[2]), 2) - Math.Pow(Double.Parse(i.distance_test[0]), 2);
        d=Coord[1][0]-Coord[2][0];
        e=Coord[1][1]-Coord[2][1];
        f=Math.Pow(Coord[1][0], 2) - Math.Pow(Coord[2][0], 2) + Math.Pow(Coord[1][1], 2) - Math.Pow(Coord[2][1], 2) + Math.Pow(Double.Parse(i.distance_test[2]), 2) - Math.Pow(Double.Parse(i.distance_test[1]), 2);
        Coord_x=(b*f-e*c)/(2*b*d-2*a*e);
        Coord_y=(a*f-d*c)/(2*a*e-2*b*d);
    }
}
