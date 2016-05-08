using UnityEngine;
using System.Collections;

public class TurretMover : Mover
{
    private NavMeshObstacle myMeshObstacle;
    public bool isGrounded = false;

    public Transform targetTransform;
    // Use this for initialization

    private Vector3 positionalOffset;

	void Start () {
        InitializeMover();
	    myMeshObstacle = GetComponent<NavMeshObstacle>();
	    positionalOffset = targetTransform.position - transform.position;
	    target = targetTransform;
	}
	
	// Update is called once per frame
	void Update () {
	    //if (!isGrounded)
	    //{
	    //    if (myStats.isAlive && !CheckIfAtDestination())
	    //    {
	    //        GotToTarget();
	    //        return;
	    //    }
	    //    if (!myStats.isAlive)
	    //    {
	    //        StopMovement();
	    //    }
	    //}
	    GetTarget();
        GotToTarget();
	}

    public void Ground()
    {
        myAgent.enabled = false;
        myMeshObstacle.enabled = true;
        isGrounded = true;
    }


    public void UnGround()
    {
        myMeshObstacle.enabled = false;
        myAgent.enabled = true;
        isGrounded = false;
    }


    public bool CheckIfAtDestination()
    {
        if (myAgent.hasPath && myAgent.remainingDistance <= 1)
        {
            targetTransform.parent = gameObject.transform;
            target = null;
            //Ground();
            return true;
        }
        return false;
    }

    public override void GotToTarget()
    {
        base.GotToTarget();
    }

    public void GetTarget()
    {
       
        if (myStats.isSelected)
        {
            if (Input.GetMouseButton(1))
            {
                Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Create a RaycastHit variable to store information about what was hit by the ray.
                RaycastHit isSelectableHit;
                if (Physics.Raycast(camRay, out isSelectableHit, 1000))
                {
                        targetTransform.parent = null;
                        targetTransform.position = CameraFollow.mouseToWorldPosition;
                        targetTransform.parent = GameObject.FindGameObjectWithTag("Grid").transform;
                        target = targetTransform;
                }
            }
        }
    }
}
