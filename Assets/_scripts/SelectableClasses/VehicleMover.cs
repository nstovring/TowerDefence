using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Engine
{
    public float enginePower = 150.0f;
    public float maxVelocity = 10;
    public float standardVelocity = 5;
    public float maxSteer = 25.0f;


    public float power = 0.0f;
    public float brake = 0.0f;
    public float steer = 0.0f;

    [Range(0, 50)]
    public float minSpeedUpRange = 5;
    [Range(50, 0)]
    public float maxTargetRange = -10;
    public bool reversing;

    private int powerMultiplier = 100;

    public void AddPower(float rangeFromTarget, float currentVelocity)
    {
        if (rangeFromTarget < minSpeedUpRange && currentVelocity < standardVelocity)
        {
            power = (enginePower * Time.deltaTime * powerMultiplier * (currentVelocity / standardVelocity));
            brake = 0.0f;
        }
        else if (rangeFromTarget > minSpeedUpRange && currentVelocity < maxVelocity)
        {
            power = (enginePower * Time.deltaTime * powerMultiplier * (currentVelocity / maxVelocity));
            brake = 0.0f;
        }
        else
        {
            float desiredVelocity =  Mathf.Abs(currentVelocity / standardVelocity);
            brake = desiredVelocity * powerMultiplier;
            power = 0;
        }
    }

    public void Steering(float rangeFromTarget, float currentVelocity, float curlookAngle)
    {
        //curlookAngle = Mathf.Clamp(curlookAngle, -100, 100);
        
        if ((curlookAngle >= -120 && curlookAngle <= 120) && !reversing)
        {
            steer =  (curlookAngle / 180f) * maxSteer;
        }
        else
        {
            reversing = true;
        }
        if (reversing)
        {
            if (rangeFromTarget < minSpeedUpRange && currentVelocity < standardVelocity)
            {
                power = -(enginePower * Time.deltaTime * powerMultiplier * (1 - currentVelocity / maxVelocity));
                brake = 0.0f;
            }
            steer = ((-curlookAngle / 180f) * maxSteer);

            if (curlookAngle <= 120 && curlookAngle >= -120)
            {
                //power = (enginePower * Time.deltaTime * powerMultiplier * (1 - currentVelocity / maxVelocity));
                if (currentVelocity > 0)
                {
                    brake = 1000f;
                    return;
                }
                reversing = false;
            }
        }
    }
}

public class VehicleMover : Mover
{
    private NavMeshObstacle myMeshObstacle;
    public Engine myEngine;
    public Transform targetTransform;
    public Grid grid;
    // Use this for initialization

    private float rangeFromTarget;
    public float currentVelocity;
    private int powerMultiplier = 100;


    public Transform[] wheels = new Transform[4];
    public Transform[] wheelPrefabs = new Transform[4];

    private WheelCollider[] wheelColliders = new WheelCollider[4];

    void Start()
    {
        InitializeMover();
        rb = GetComponent<Rigidbody>();
        grid = myStats.myParty.grid;
        targetTransform = grid.GetClosestPosition(targetTransform);
        target = targetTransform;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i] = wheels[i].gameObject.GetComponent<WheelCollider>();
            wheelPrefabs[i] = wheels[i].GetChild(0);
        }
        StartCoroutine(GoToTarget());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetTarget();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, myAgent.desiredVelocity * 5, Color.red);

        Debug.DrawLine(transform.position, target.position, Color.blue);
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

    public override IEnumerator GoToTarget()
    {
        while (true)
        {
            if (myAgent.isOnNavMesh)
            {
                myAgent.SetDestination(grid.GetNearestCellOnNavmesh(target).position);
            }
            myAgent.transform.position = transform.position;
            myEngine.standardVelocity = grid.rb.velocity.magnitude;

            //Checks if current target is on the navmesh else replace it with one that is
            rangeFromTarget = Vector3.Distance(target.position, transform.position) + myEngine.maxTargetRange;
            currentVelocity= rb.velocity.magnitude;
            //Vector3 desiredDirection = myAgent.desiredVelocity;
            Vector3 desiredDirection =  myAgent.steeringTarget - transform.position;

            float curlookAngle = Vector3.Angle(desiredDirection, transform.forward);
            // assume the sign of the cross product's Y component:
            curlookAngle *= Mathf.Sign(Vector3.Cross(desiredDirection, transform.forward).y)* -1;

            myEngine.Steering(rangeFromTarget,currentVelocity,curlookAngle);

            yield return new WaitForFixedUpdate();
            if (!myEngine.reversing)
            {
                myEngine.AddPower(rangeFromTarget, currentVelocity);
            }
            yield return new WaitForFixedUpdate();

            GetCollider(0).steerAngle = myEngine.steer;
            GetCollider(1).steerAngle = myEngine.steer;

            if  (myEngine.brake > 0.0)
            {
                GetCollider(0).brakeTorque = myEngine.brake;
                GetCollider(1).brakeTorque = myEngine.brake;
                GetCollider(2).brakeTorque = myEngine.brake;
                GetCollider(3).brakeTorque = myEngine.brake;
                GetCollider(2).motorTorque = 0.0f;
                GetCollider(3).motorTorque = 0.0f;
            }
            else {
                GetCollider(0).brakeTorque = 0;
                GetCollider(1).brakeTorque = 0;
                GetCollider(2).brakeTorque = 0;
                GetCollider(3).brakeTorque = 0;
                GetCollider(2).motorTorque = myEngine.power;
                GetCollider(3).motorTorque = myEngine.power;

            }

            yield return new WaitForFixedUpdate();
        }
    }

   

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
                targetTransform = grid.GetClosestPosition(CameraFollow.GetMouseScreenToRay() + transform.forward * (-myEngine.maxTargetRange + myEngine.minSpeedUpRange));
                if (targetTransform != null)
                {
                    target = targetTransform;
                }
            }
        }
    }
}
