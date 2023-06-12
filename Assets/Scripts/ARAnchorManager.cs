using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARAnchorManager : MonoBehaviour
{
    public static ARAnchorManager Instance { get; private set; }
    public GameObject realEnvironmentPrefab;
    private GameObject _metaEnvironment;

    // the environment attribute
    public float WIDTH = 5;
    public float HEIGHT = 3;

    // the new origin position -> used as reference
    public static Vector3 OriginPosition;

    ARTrackedImageManager _mTrackedImageManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _mTrackedImageManager = GetComponent<ARTrackedImageManager>();
        InitializeEnvironment();
    }


    void OnEnable()
    {
        _mTrackedImageManager.trackedImagesChanged += OnDetectMarker;
    }

    void OnDisable()
    {
        _mTrackedImageManager.trackedImagesChanged -= OnDetectMarker;
    }

    void OnDetectMarker(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            SetAnchor(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            SetAnchor(trackedImage);
        }
    }


    void InitializeEnvironment()
    {
        _metaEnvironment = Instantiate(realEnvironmentPrefab);
        _metaEnvironment.transform.localScale = new Vector3(HEIGHT, 1f, WIDTH);
        _metaEnvironment.SetActive(false);
    }

    void SetAnchor(ARTrackedImage trackedImage)
    {
        Debug.Log("add a new anchor");
        Vector3 offset = new Vector3(HEIGHT / 2, 0, WIDTH / 2);
        OriginPosition = trackedImage.gameObject.transform.position;
        Vector3 envPosition = OriginPosition + offset;
        _metaEnvironment.transform.position = envPosition;
        _metaEnvironment.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}