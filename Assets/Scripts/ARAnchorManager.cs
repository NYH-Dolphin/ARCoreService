using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARAnchorManager : MonoBehaviour
{
    public static ARAnchorManager Instance { get; private set; }
    public GameObject realEnvironmentPrefab;

    
    ARTrackedImageManager m_TrackedImageManager;
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

        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }


    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnDetectMarker;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnDetectMarker;
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

    void SetAnchor(ARTrackedImage trackedImage)
    {
        GameObject realEnvironment = Instantiate(realEnvironmentPrefab);
        realEnvironment.transform.position = trackedImage.gameObject.transform.position;
        
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