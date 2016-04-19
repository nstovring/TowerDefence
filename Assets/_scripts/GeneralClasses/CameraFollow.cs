using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.
    private Quaternion rotation;

    private Vector3 originalOffset;
    private Quaternion originalRotation;

    public Transform tacticalViewTransform;

    public enum ViewType
    {
        PlayerView,
        TacticalView
    };

    public ViewType myCurrentViewType;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
        originalOffset = offset;

        originalRotation = transform.rotation;
        rotation = originalRotation;
        myCurrentViewType = ViewType.PlayerView;
    }

    private Vector3 targetCamPos;
    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        if (myCurrentViewType == ViewType.PlayerView)
        {
            targetCamPos = target.position + offset;
        }

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothing*Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Space) && myCurrentViewType != ViewType.TacticalView)
        {
            targetCamPos = tacticalViewTransform.position;
            rotation = tacticalViewTransform.rotation;
            myCurrentViewType = ViewType.TacticalView;
        }else if (Input.GetKeyDown(KeyCode.Space) && myCurrentViewType != ViewType.PlayerView)
        {
            //targetCamPos = target.position + offset;
            rotation = originalRotation;
            myCurrentViewType = ViewType.PlayerView;
        }




        if (myCurrentViewType == ViewType.TacticalView)
        {
            TacticalView();
        }
    }

    public Transform tacticalViewOrientation;

    void TacticalView()
    {
        targetCamPos += new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
    }
}
