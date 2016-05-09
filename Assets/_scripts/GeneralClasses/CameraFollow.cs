using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public float zoomSpeed = 2;
    Vector3 offset;                     // The initial offset from the target.
    private Quaternion rotation;

    private Quaternion originalRotation;

    public Transform tacticalViewTransform;
    public static Vector3 mouseToWorldPosition;

    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 1000f;          // The length of the ray from the camera into the scene.

    public enum ViewType
    {
        PlayerView,
        TacticalView
    };

    public static ViewType myCurrentViewType;
    private Camera mainCamera;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
    }
    void Start()
    {
        mainCamera = Camera.main;
        // Calculate the initial offset.
        offset = transform.position - target.position;
        //originalOffset = offset;

        originalRotation = transform.rotation;
        rotation = originalRotation;
        myCurrentViewType = ViewType.PlayerView;
    }

    private Vector3 targetCamPos;
    public float zoomSmoothing = 1;
    float scrollDelta;
    private float newSize;

    public Transform lookTransform;

    void FixedUpdate()
    {
        GetMouseScreenToRay();
        // Create a postion the camera is aiming for based on the offset from the target.
        if (myCurrentViewType == ViewType.PlayerView)
        {
            //Vector3 newPos = new Vector3(transform.forward.);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTransform.position - transform.position), smoothing * Time.deltaTime);

            //transform.LookAt(lookTransform);
            //transform.lo
            //offset.x += Input.GetAxis("Horizontal");
            //offset.z += Input.GetAxis("Vertical");
            //offset += (Camera.main.transform.forward*Input.GetAxis("Vertical")) +
            //(Camera.main.transform.right*Input.GetAxis("Horizontal"));
            //offset += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) + Camera.main.transform.forward;
            targetCamPos = target.position + offset;
        }
        if (myCurrentViewType == ViewType.TacticalView)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                scrollDelta = Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
                newSize = mainCamera.orthographicSize - scrollDelta;
            }
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newSize, zoomSmoothing * Time.deltaTime);
        }

        // Smoothly interpolate between the camera's current position and it's target position.
        //if (Vector3.Distance(transform.position, targetCamPos) > 10 )
        //{
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothing*Time.deltaTime);
        //}

        if (Input.GetKeyDown(KeyCode.Space) && myCurrentViewType != ViewType.TacticalView)
        {
            targetCamPos = tacticalViewTransform.position;
            rotation = tacticalViewTransform.rotation;
            mainCamera.orthographic = true;
            newSize = 25;
            myCurrentViewType = ViewType.TacticalView;
        }else if (Input.GetKeyDown(KeyCode.Space) && myCurrentViewType == ViewType.TacticalView)
        {
            rotation = originalRotation;
            newSize = 0;
            mainCamera.orthographic = false;
            myCurrentViewType = ViewType.PlayerView;
        }

        if (myCurrentViewType == ViewType.TacticalView)
        {
            TacticalViewMovement();
        }
        ChangePositions();
    }

    void ChangePositions()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            offset.x = 8.3f;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            offset.x = -8.3f;
        }
    }

    public Transform tacticalViewOrientation;

    void TacticalViewMovement()
    {
        targetCamPos += new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
    }

    public void GetMouseScreenToRay()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            mouseToWorldPosition = floorHit.point;
        }
    }
}
