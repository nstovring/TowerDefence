using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selecter : MonoBehaviour {
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    int isSelectableMask;               // A layer mask so that a ray can be cast just at gameobjects on the IsSelectable layer.

    public List<Stats> selectedObjectList; 
    // Use this for initialization
    void Start () {
        isSelectableMask = LayerMask.GetMask("isSelectable");
        //isSelectableMask = 1 << isSelectableMask;
        selectedObjectList = new List<Stats>();
    }

    // Update is called once per frame
    void Update () {
        if (selectedObjectList.Count > 0)
        {
            foreach (Stats stats in selectedObjectList)
            {
                stats.isSelected = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            SendRayCastForSelectables();
        }
    }

    public void SendRayCastForSelectables()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit isSelectableHit;
        EmptySelectedList(); //Put this somewhere else
        if (Physics.Raycast(camRay, out isSelectableHit, camRayLength, isSelectableMask))
        {
            Stats hitObjectStats = isSelectableHit.transform.GetComponentInParent<Stats>();
            selectedObjectList.Add(hitObjectStats);
        }
    }

    public void EmptySelectedList()
    {
        if (selectedObjectList.Count > 0)
        {
            foreach (Stats stats in selectedObjectList)
            {
                stats.isSelected = false;
            }
        }
        selectedObjectList = new List<Stats>();
    }
}
