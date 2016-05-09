using UnityEngine;
using System.Collections;
using System.Linq;

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
        rigidbody = GetComponent<Rigidbody>();
        grid = myStats.myParty.grid;
        targetTransform = grid.GetClosestPosition(targetTransform);
        positionalOffset = targetTransform.position - transform.position;
        target = targetTransform;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i] = wheels[i].gameObject.GetComponent<WheelCollider>();
        }
        //targetTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
        //grid.SetDestination(targetTransform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetTarget();
        GoToTarget();
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

    public float enginePower = 150.0f;
    public float maxVelocity = 10;
    public float standardVelocity = 5;
    public float maxSteer = 25.0f;


    public float power = 0.0f;
    public float brake = 0.0f;
    public float steer = 0.0f;


    public float minSpeedUpRange = 5;
    public float maxTargetRange = 10;

    private Rigidbody rigidbody;
    public override void GoToTarget()
    {
        //base.GoToTarget();
        if (wheels[0] == null)
        {
            base.GoToTarget();
            return;
        }

        if (myAgent.isOnNavMesh)
        {
            myAgent.SetDestination(grid.GetNearestCellOnNavmesh(target).position);
        }
        myAgent.transform.position = transform.position;
        //Checks if current target is on the navmesh else replace it with one that is

        Vector3 desiredDirection = myAgent.desiredVelocity;
        float curlookAngle = Vector3.Angle(desiredDirection, transform.forward);

        // assume the sign of the cross product's Y component:
        curlookAngle *= Mathf.Sign(Vector3.Cross(desiredDirection, transform.forward).y)* -1;

        Debug.DrawRay(transform.position, desiredDirection* 5,Color.red);

        Debug.DrawLine(transform.position, target.position, Color.blue);

        float rangeFromTarget = Vector3.Distance(target.position, transform.position) + maxTargetRange;

        if (rangeFromTarget < minSpeedUpRange && rigidbody.velocity.magnitude < standardVelocity)
        {
            power = (enginePower*Time.deltaTime*250f);
            brake = 0.0f;
        }
        else if (rangeFromTarget > minSpeedUpRange && rigidbody.velocity.magnitude < maxVelocity)
        {
            power = (enginePower*Time.deltaTime*250f);
            brake = 0.0f;
        }
        else
        {
            float desiredVelocity = Mathf.Abs(rigidbody.velocity.magnitude - standardVelocity);
            brake = desiredVelocity*10;
            power = 0;
            //brake = 1 + (150-(rangeFromTarget*5));
        }
        //if (curlookAngle < 0)
        //{
        //    steer = Mathf.Abs(curlookAngle);
        //}

        steer = (curlookAngle/360f)*maxSteer;
       
        GetCollider(0).steerAngle = steer;
        GetCollider(1).steerAngle = steer;

        if (brake > 0.0)
        {
            GetCollider(0).brakeTorque = brake;
            GetCollider(1).brakeTorque = brake;
            GetCollider(2).brakeTorque = brake;
            GetCollider(3).brakeTorque = brake;
            GetCollider(2).motorTorque = 0.0f;
            GetCollider(3).motorTorque = 0.0f;
        }
        else {
            GetCollider(0).brakeTorque = 0;
            GetCollider(1).brakeTorque = 0;
            GetCollider(2).brakeTorque = 0;
            GetCollider(3).brakeTorque = 0;
            GetCollider(2).motorTorque = power;
            GetCollider(3).motorTorque = power;
        }

    }

    public Transform[] wheels = new Transform[4];
    public WheelCollider[] wheelColliders = new WheelCollider[4];


    WheelCollider GetCollider(int i)
    {
        return wheelColliders[i];
    }


    public void GetTarget()
    {
        if (myStats.isSelected)
        {
            if (Input.GetMouseButton(1))
            {
                targetTransform = grid.GetClosestPosition(CameraFollow.mouseToWorldPosition + transform.forward * (-maxTargetRange + minSpeedUpRange));
                if (targetTransform != null)
                {
                    target = targetTransform;
                }
            }
        }
    }
}
