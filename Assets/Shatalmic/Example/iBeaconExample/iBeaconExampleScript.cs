using UnityEngine;
using System.Collections.Generic;
using System;

public class iBeaconExampleScript : MonoBehaviour
{
    public GameObject iBeaconItemPrefab;

    private float _timeout = 0f;
    private float _startScanTimeout = 10f;
    private float _startScanDelay = 0.5f;
    private bool _startScan = true;
    private Dictionary<string, iBeaconItemScript> _iBeaconItems;
    private Dictionary<float, string> _distance;
    public string[] distance_test = { "1", "2", "3" };
    private float[][] Coord = new float[3][];
    private float[][] Coord0 = new float[3][];
    private float a, b, c, d, e, f;

    private string[] UUIDS = new string[]
    {
        "01020304-0506-0708-0910-111213141516:Pit01", "01122334-4556-6778-899A-ABBCCDDEEFF0:Pit01",
        "01122334-4556-6778-899A-ABBCCDDEE000:Pit01"
    };

    private string[] check_UUID =
    {
        "01020304050607080910111213141516", "0112233445566778899AABBCCDDEEFF0",
        "0112233445566778899AABBCCDDEE000"
    };

    private int count;

    public float Coord_x;
    public float Coord_y;
    public float Coord_z;

    void Start()
    {
        count = 0;
        _iBeaconItems = new Dictionary<string, iBeaconItemScript>();

        BluetoothLEHardwareInterface.Initialize(true, false, () =>
            {
                _timeout = _startScanDelay;

                BluetoothLEHardwareInterface.BluetoothScanMode(BluetoothLEHardwareInterface.ScanMode.LowLatency);
                BluetoothLEHardwareInterface.BluetoothConnectionPriority(BluetoothLEHardwareInterface.ConnectionPriority
                    .High);
            },
            (error) =>
            {
                BluetoothLEHardwareInterface.Log("Error: " + error);

                if (error.Contains("Bluetooth LE Not Enabled"))
                    BluetoothLEHardwareInterface.BluetoothEnable(true);
            });
        

        Coord[0] = new float[2];
        Coord[1] = new float[2];
        Coord[2] = new float[2];
        Coord[0][0] = 0;
        Coord[0][1] = 0;
        Coord[1][0] = 0;
        Coord[1][1] = -5;
        Coord[2][0] = 3;
        Coord[2][1] = 0;

        Coord0 = Coord;
        Coord_y = transform.GetComponent<LocationTrackManager>().xrOrigin.CameraYOffset;
    }

    public float Distance(float signalPower, float rssi, float nValue)
    {
        return (float)Math.Pow(10, ((signalPower - rssi) / (10 * nValue)));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(distance_test[0]);
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                if (_startScan)
                {
                    // Debug.Log("begin");
                    _startScan = false;
                    _timeout = _startScanTimeout;

                    // scanning for iBeacon devices requires that you know the Proximity UUID and provide an Identifier
                    //"01020304-0506-0708-0910-111213141516:Pit01"
                    // 开始搜索beacon信标，传入的string数组里是要识别的ibeacon的UUID，下面的action是搜索到之后要执行的内容
                    BluetoothLEHardwareInterface.ScanForBeacons(UUIDS, (iBeaconData) =>
                    {
                        // Debug.Log(iBeaconData.UUID);
                        // Debug.Log("success");
                        if (!_iBeaconItems.ContainsKey(iBeaconData.UUID))
                        {
                            BluetoothLEHardwareInterface.Log("item new: " + iBeaconData.UUID);
                            var newItem = Instantiate(iBeaconItemPrefab);
                            if (newItem != null)
                            {
                                BluetoothLEHardwareInterface.Log("item created: " + iBeaconData.UUID);
                                newItem.transform.SetParent(transform);
                                newItem.transform.localScale = new Vector3(1f, 1f, 1f);

                                var iBeaconItem = newItem.GetComponent<iBeaconItemScript>();
                                if (iBeaconItem != null)
                                    _iBeaconItems[iBeaconData.UUID] = iBeaconItem;
                            }

                            for (int i = 0; i < 3; i++)
                            {
                                if (iBeaconData.UUID == check_UUID[i])
                                {
                                    _iBeaconItems[iBeaconData.UUID].Num = i;
                                    Debug.Log(i);
                                }
                            }

                            count++;
                        }

                        if (_iBeaconItems.ContainsKey(iBeaconData.UUID))
                        {
                            var iBeaconItem = _iBeaconItems[iBeaconData.UUID];
                            iBeaconItem.TextUUID.text = iBeaconData.UUID;
                            iBeaconItem.TextRSSIValue.text = iBeaconData.RSSI.ToString();

                            iBeaconItem.TextAndroidSignalPower.text = iBeaconData.AndroidSignalPower.ToString();

                            iBeaconItem.TextiOSProximity.text = iBeaconData.iOSProximity.ToString();

                            //计算与信标的距离
                            if (iBeaconData.AndroidSignalPower != 0)
                            {
                                iBeaconItem.TextDistance.text =
                                    Distance(iBeaconData.AndroidSignalPower, iBeaconData.RSSI, 2.5f).ToString();
                                distance_test[iBeaconItem.Num] = Math.Sqrt(Math.Pow(Coord_y, 2) -
                                                                           Math.Pow(
                                                                               float.Parse(iBeaconItem.TextDistance
                                                                                   .text)/10, 2)).ToString();
                                Debug.Log(distance_test);
                                // distance_test = iBeaconItem.TextDistance.text;
                            }
                        }
                    });
                }
                else
                {
                    BluetoothLEHardwareInterface.StopScan();
                    _startScan = true;
                    _timeout = _startScanDelay;
                }
            }
        }
        
        if (count > 2) //三点定位
        {
            Debug.Log("location");
            a = Coord[0][0] - Coord[2][0];
            b = Coord[0][1] - Coord[2][1];
            c = (float)(Math.Pow(Coord[0][0], 2) - Math.Pow(Coord[2][0], 2) + Math.Pow(Coord[0][1], 2) -
                        Math.Pow(Coord[2][1], 2) + Math.Pow(float.Parse(distance_test[2]), 2) -
                        Math.Pow(float.Parse(distance_test[0]), 2));
            d = Coord[1][0] - Coord[2][0];
            e = Coord[1][1] - Coord[2][1];
            f = (float)(Math.Pow(Coord[1][0], 2) - Math.Pow(Coord[2][0], 2) + Math.Pow(Coord[1][1], 2) -
                        Math.Pow(Coord[2][1], 2) + Math.Pow(Double.Parse(distance_test[2]), 2) -
                        Math.Pow(Double.Parse(distance_test[1]), 2));
            Coord_x = (b * f - e * c) / (2 * b * d - 2 * a * e);
            Coord_z = (a * f - d * c) / (2 * a * e - 2 * b * d);
            Debug.Log(Coord_x);
            for (int i = 0; i < 3; i++)
            {
                Coord0[i][0] -= Coord_x;
                Coord0[i][1] -= Coord_z;
            }

            Vector3 B = new Vector3(Coord0[0][0], 0f, Coord0[0][1]);
            Vector3 C = new Vector3(Coord0[1][0], 0f, Coord0[1][1]);
            Vector3 A = new Vector3(Coord0[2][0], 0f, Coord0[2][1]);
            transform.GetComponent<LocationTrackManager>().EnvironmentUpdate(A, B, C);
        }
    }
}