using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.XR.CoreUtils;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LocationTrackManager : MonoBehaviour
{
    public XROrigin xrOrigin;
    
    // the corresponding environment (meta)
    public GameObject MetaEnvironmentPrefab;
    private GameObject MetaEnvironment;
    
    private float WIDTH;
    private float HEIGHT;


    // Start is called before the first frame update
    void Start()
    {
        // EnvironmentUpdate(new Vector3(2,0,0), new Vector3(0,0,0), new Vector3(0,0,-5));
    }


    // Update is called once per frame
    void Update()
    {
    }


    
    
    /**
     * Beacons' Position Settlement
     *         width
     * A —————————————————
     * |                  |
     * |                  |  height
     * |       dir        |
     * B ←——————————————— C
     * ↑
     * original Pos
     */
    public void EnvironmentUpdate(Vector3 A, Vector3 B, Vector3 C)
    {
        float height = Vector3.Distance(A, B);
        float width = Vector3.Distance(B, C);

        OnSetScale(height, width);
        OnSetPosition(B, Vector3.Normalize(B - C));
    }


    /// <summary>
    /// Set the correct scale of the meta environment
    /// </summary>
    /// <param name="height"></param> AB's distance
    /// <param name="width"></param> BC's distance
    void OnSetScale(float height, float width)
    {
        WIDTH = width;
        HEIGHT = height;

        MetaEnvironment = Instantiate(MetaEnvironmentPrefab);
        MetaEnvironment.transform.localScale = new Vector3(height, 1f, width);
    }

    /// <summary>
      /// Set the correct position of the meta environment
      /// </summary>
      /// <param name="originalPos"></param> Require the iBeacon B's position
      /// <param name="dir"></param> Require the direction vector of CB
      void OnSetPosition(Vector3 originalPos, Vector3 dir)
    {
        // Set Original Position
        Vector3 offset = new Vector3(HEIGHT / 2, -xrOrigin.CameraYOffset, -WIDTH / 2);
        MetaEnvironment.transform.position = originalPos + offset;

        // Set Rotation
        float angle = Vector3.Angle(MetaEnvironment.transform.forward, dir);
        MetaEnvironment.transform.RotateAround(originalPos, Vector3.up, angle);
    }
}