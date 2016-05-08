using UnityEngine;
using System.Collections;

public class VehicleMover : Mover
{
    private NavMeshObstacle myMeshObstacle;
    public bool isGrounded = false;

    public Transform targetTransform;
    public Grid grid;
    // Use this for initialization

    private Vector3 positionalOffset;

    void Start()
    {
        InitializeMover();
        grid = myStats.myParty.grid;
        targetTransform = grid.GetClosestPosition(targetTransform.position);
        positionalOffset = targetTransform.position - transform.position;
        target = targetTransform;
        //targetTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
        //grid.SetDestination(targetTransform);
    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        GotToTarget();
    }

    public bool CheckIfAtDestination()
    {
        if (myAgent.hasPath && myAgent.remainingDistance <= 1)
        {
            targetTransform.parent = gameObject.transform;
            target = null;
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
                targetTransform = grid.GetClosestPosition(CameraFollow.mouseToWorldPosition);
                if (targetTransform != null)
                {
                    target = targetTransform;
                }
            }
        }
    }
}
