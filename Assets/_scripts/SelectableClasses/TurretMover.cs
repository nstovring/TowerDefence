using UnityEngine;
using System.Collections;

public class TurretMover : Mover
{
    private NavMeshObstacle myMeshObstacle;
    public bool isGrounded = false;

    public Transform targetTransform;
    // Use this for initialization
	void Start () {
        InitializeMover();
	    myMeshObstacle = GetComponent<NavMeshObstacle>();
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

    public void GetTarget()
    {
        if (myStats.isSelected)
        {
            if (Input.GetMouseButton(1))
            {
                Debug.Log("selected new position");
                targetTransform.parent = null;
                targetTransform.position = CameraFollow.mouseToWorldPosition;
                target = targetTransform;
                //UnGround();
            }
        }
    }
}
