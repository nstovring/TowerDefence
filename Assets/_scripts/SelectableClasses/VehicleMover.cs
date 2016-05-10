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
        rb = GetComponent<Rigidbody>();
        grid = myStats.myParty.grid;
        targetTransform = grid.GetClosestPosition(targetTransform);
        positionalOffset = targetTransform.position - transform.position;
        target = targetTransform;

        if (wheels[0] != null)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheelColliders[i] = wheels[i].gameObject.GetComponent<WheelCollider>();
                wheelPrefabs[i] = wheels[i].GetChild(0);
            }
        }
        StartCoroutine(GoToTarget());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetTarget();
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
    bool reversing;

    public override IEnumerator GoToTarget()
    {
        while (true)
        {
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

            //Debug.DrawRay(transform.position, desiredDirection* 5,Color.red);

            //Debug.DrawLine(transform.position, target.position, Color.blue);

            float rangeFromTarget = Vector3.Distance(target.position, transform.position) + maxTargetRange;
            int range = 5;

            if (curlookAngle >= -120 && curlookAngle - range <= 120)
            {
                reversing = false;
            }
            else
            {
                reversing = true;
            }

            if ((curlookAngle + range >= -120 - range && curlookAngle - range <= 120 + range) && rangeFromTarget > maxTargetRange)
            {
                steer = (curlookAngle / 360f) * maxSteer;
            }
            else
            {
                power = -(enginePower * Time.deltaTime * 250f);
                brake = 0.0f;
                steer = -((curlookAngle / 360f) * maxSteer) / 2;
            }
            yield return new WaitForEndOfFrame();

            if (rangeFromTarget < minSpeedUpRange && rb.velocity.magnitude < standardVelocity)
            {
                power = (enginePower*Time.deltaTime*250f);
                brake = 0.0f;
            }
            else if (rangeFromTarget > minSpeedUpRange && rb.velocity.magnitude < maxVelocity)
            {
                power = (enginePower*Time.deltaTime*250f);
                brake = 0.0f;
            }
            else
            {
                float desiredVelocity = Mathf.Abs(rb.velocity.magnitude - standardVelocity);
                brake = desiredVelocity*10;
                power = 0;
            }
            yield return new WaitForEndOfFrame();
            

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
            yield return new WaitForFixedUpdate();
        }
    }

    public Transform[] wheels = new Transform[4];
    public Transform[] wheelPrefabs = new Transform[4];

    private WheelCollider[] wheelColliders = new WheelCollider[4];


    WheelCollider GetCollider(int i)
    {
        Vector3 wheelPos;
        Quaternion wheelOrientation;
        wheelColliders[i].GetWorldPose(out wheelPos, out wheelOrientation);
        wheelOrientation.eulerAngles += new Vector3(0,0,90);//= new Vector3(wheelOrientation.eulerAngles.x, wheelOrientation.eulerAngles.y, wheelOrientation.eulerAngles.z + 90);
        wheelPrefabs[i].position = wheelPos;
        wheelPrefabs[i].rotation = wheelOrientation;
        return wheelColliders[i];
    }


    public void GetTarget()
    {
        if (myStats.isSelected)
        {
            if (Input.GetMouseButton(1))
            {
                targetTransform = grid.GetClosestPosition(CameraFollow.GetMouseScreenToRay() + transform.forward * (-maxTargetRange + minSpeedUpRange));
                if (targetTransform != null)
                {
                    target = targetTransform;
                }
            }
        }
    }
}
