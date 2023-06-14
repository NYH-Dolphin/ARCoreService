using System.Collections;
using DefaultNamespace;
using LitJson;
using NetworkTools;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] public string serverIp;
    [SerializeField] public string serverPort;
    [SerializeField] private string[] subCatalogue;
    public GameObject mGameObject;
    private HttpManager _httpManager;

    private void Start()
    {
        StartCoroutine(Getter());
    }

    public IEnumerator Getter()
    {
        while (true)
        {
            // StartCoroutine(SendGetRequest());
            StartCoroutine(SendSaveRequest());
            yield return new WaitForSeconds(1f);
        }
    }


    public IEnumerator SendGetRequest()
    {
        SMessage smsg = new SMessage("GET_OBJECT", "a", new Vector3(), new Vector3(), new Vector3());
        RMessage rmsg = new RMessage();
        HttpManager httpManager = new HttpManager(rmsg, smsg);
        string uri = httpManager.GetParser(serverIp, serverPort);
        Debug.Log(uri);
        StartCoroutine(httpManager.Get(uri));


        while (httpManager.Result == NetworkResult.Waiting)
        {
            yield return null;
        }

        if (httpManager.Result == NetworkResult.Success)
        {
            Debug.Log("update success!");
            //todo: create a new object
            GameObject cube = GameObject.Find("MovableObject(Clone)");
            if (cube == null)
            {
                cube = Instantiate(mGameObject, rmsg.prefabs[0].position, rmsg.prefabs[0].rotation);
            }
            else
            {
                if (rmsg.prefabs.Count > 0)
                {
                    cube.transform.position = rmsg.prefabs[0].position;
                    StartCoroutine(Move((rmsg.prefabs[0].position - cube.transform.position) / 50, cube));
                }
                else
                {
                    Destroy(cube);
                }
            }
        }
    }

    public IEnumerator Move(Vector3 toward, GameObject cube)
    {
        int t = 0;
        while (t < 50)
        {
            t += 1;
            cube.transform.position += toward;
            yield return new WaitForSeconds(0.02f);
        }
    }


    public IEnumerator SendSaveRequest()
    {
        while (GameObject.Find("MovableObject(Clone)") == null)
        {
            yield return new WaitForSeconds(1f);
        }
        SMessage smsg = new SMessage("UPDATE_OBJECT", GetComponent<Position>().cube.name,
            GetComponent<Position>()._currentPosition,
            Quaternion.Euler(GetComponent<Position>().cube.transform.rotation.eulerAngles) * Vector3.forward,
            GetComponent<Position>().cube.transform.localScale);

        RMessage rmsg = new RMessage();
        _httpManager = new HttpManager(rmsg, smsg);
        // Get the URI for the GET request
        string uri = _httpManager.GetParser(serverIp, serverPort, subCatalogue);

        StartCoroutine(_httpManager.Get(uri));
        while (_httpManager.Result == NetworkResult.Waiting)
        {
            yield return null;
        }

        if (_httpManager.Result == NetworkResult.Success)
        {
            Debug.Log("Save success!");
        }
    }
}