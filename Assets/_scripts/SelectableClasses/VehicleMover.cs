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

    [HideInInspector]
    public float power = 0.0f;
    [HideInInspector]
    public float brake = 0.0f;
    [HideInInspector]
    public float steer = 0.0f;

    public float KPH;

    [Range(0, 50)]
    public float minSpeedUpRange = 5;

    public bool reversing;

    private int powerMultiplier = 100;
    private float rangeFromTarget;

    private float reverseTimer = 1;
    private float curReverseTime = 0;

    public void AddPower(float rangeFromTarget, float currentVelocity)
    {
        KPH = (currentVelocity / 60) * 1000;
        int cruisingRange = 2;
        power = 0;
        this.rangeFromTarget = rangeFromTarget;
        if (rangeFromTarget- cruisingRange < minSpeedUpRange+ cruisingRange && currentVelocity > standardVelocity)
        {
           // Debug.Log("Slowing Down");
            float desiredVelocity = Mathf.Abs(currentVelocity / standardVelocity);
            brake = desiredVelocity * powerMultiplier;
            power = 0;
        }
        else if (rangeFromTarget + cruisingRange > minSpeedUpRange - cruisingRange && currentVelocity < maxVelocity)
        {
            //Debug.Log("Speeding Up");
            power += (enginePower * Time.deltaTime * powerMultiplier * (1-currentVelocity / maxVelocity));
            brake = 0.0f;
        }
        else
        {
            if (rangeFromTarget - cruisingRange < minSpeedUpRange + cruisingRange)
            {
                //Debug.Log("Slowing Down");
                float desiredVelocity = Mathf.Abs(currentVelocity / standardVelocity);
                brake = desiredVelocity * powerMultiplier;
                power = 0;
                return;
            }
            //Debug.Log("Cruising speed");
            power += (enginePower * Time.deltaTime * powerMultiplier * (1 - currentVelocity / standardVelocity));
            brake = 0.0f;
        }
    }

   
    public void Steering(float rangeFromTarget, float currentVelocity, float curlookAngle)
    {
        if ((curlookAngle >= -120 && curlookAngle <= 120) && !reversing)
        {
            steer =  (curlookAngle / 180f) * maxSteer;
            curReverseTime = 0;
            return;
        }
        if (curReverseTime < reverseTimer)
        {
            curReverseTime += Time.deltaTime;
        }
        reversing = true;
        if (reversing)
        {
            if (currentVelocity < standardVelocity)
            {
                power = -(enginePower * Time.deltaTime * powerMultiplier * (1 - currentVelocity / maxVelocity));
                brake = 0.0f;
            }
            steer = ((-curlookAngle / 180f) * maxSteer);

            if (curlookAngle <= 20 && curlookAngle >= -20 )
            {
                if (currentVelocity > 0.1)
                {
                    power = 0;
                    brake = 1f * enginePower;
                    //Debug.Log("Braking");
                    return;
                }
                curReverseTime = 0;
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
    private float rangeFromTarget;
    private int powerMultiplier = 100;

    public Transform[] wheels = new Transform[4];

    public WheelCollider[] wheelColliders = new WheelCollider[4];

    private float curlookAngle;

    private Stats myLeader;

    void Start()
    {
        InitializeMover();
        rb = GetComponent<Rigidbody>();
        grid = myStats.myParty.grid;
        //targetTransform = grid.GetClosestPosition(targetTransform.position + grid.transform.forward * (myEngine.minSpeedUpRange));
        //target = targetTransform;
        myAgent.SetDestination(target.position);

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i] = wheels[i].gameObject.GetComponent<WheelCollider>();
        }

        myLeader = myStats.myParty.partyLeader;

        StartCoroutine(GoToTarget());
    }

    void FixedUpdate()
    {
        GetTarget();
    }

    private Vector3 myDesiredVelocity = Vector3.zero;
    void Update()
    {
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
            if (myAgent.pathStatus == NavMeshPathStatus.PathComplete) 
            {
                myAgent.transform.parent = transform;
                myAgent.acceleration = 0;
                myAgent.transform.position = transform.position;
                yield return StartCoroutine(VelocityControl());
                ApplyForceToWheels();
            }
            yield return new WaitForFixedUpdate();
        }
    }


    bool IsVehicleWithinCameraRange()
    {
        Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 middleOfTheSCreen = new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);

        if (Vector3.Distance(middleOfTheSCreen, targetScreenPoint) > Camera.main.pixelWidth*4)
        {
            return false;
        }
        return true;
    }

    void ApplyForceToWheels()
    {
        GetCollider(0).steerAngle = myEngine.steer;
        GetCollider(1).steerAngle = myEngine.steer;
        if (myEngine.brake > 0.0)
        {
            GetCollider(0).brakeTorque = myEngine.brake;
            GetCollider(1).brakeTorque = myEngine.brake;
            GetCollider(2).brakeTorque = myEngine.brake;
            GetCollider(3).brakeTorque = myEngine.brake;
            GetCollider(2).motorTorque = 0.0f;
            GetCollider(3).motorTorque = 0.0f;
        }
        else
        {
            GetCollider(0).brakeTorque = 0;
            GetCollider(1).brakeTorque = 0;
            GetCollider(2).brakeTorque = 0;
            GetCollider(3).brakeTorque = 0;
            GetCollider(2).motorTorque = (int)myEngine.power;
            GetCollider(3).motorTorque = (int)myEngine.power;
        }
    }

    IEnumerator VelocityControl()
    {
        myDesiredVelocity = myAgent.desiredVelocity;
        if (!myStats.isLeader)
        {
            rangeFromTarget = Vector3.Distance(myLeader.transform.position, transform.position);
        }else
        {
            rangeFromTarget = Vector3.Distance(target.transform.position, transform.position);
        }
        currentVelocity = rb.velocity.magnitude;
        Vector3 desiredDirection = myDesiredVelocity;

        float stepValue = 180f / curlookAngle;
        curlookAngle = Mathf.Abs(curlookAngle) * stepValue;
        curlookAngle = Vector3.Angle(desiredDirection, transform.forward);
        // assume the sign of the cross product's Y component:
        curlookAngle *= Mathf.Sign(Vector3.Cross(desiredDirection, transform.forward).y) * -1;

        myEngine.Steering(rangeFromTarget, currentVelocity, curlookAngle);
        yield return new WaitForFixedUpdate();
        if (!myEngine.reversing)
        {
            myEngine.AddPower(rangeFromTarget, currentVelocity);
        }
        yield return new WaitForFixedUpdate();
    }

    void SwitchToAgentControl()
    {
        myAgent.transform.parent = null;
        myAgent.speed = myEngine.maxVelocity;
        myAgent.acceleration = 5;
        Vector3 offsetAgentVector = myAgent.transform.position;
        offsetAgentVector.y = transform.position.y;
        rb.MovePosition(offsetAgentVector);
        rb.MoveRotation(myAgent.transform.rotation);
    }

    public void LateUpdate()
    {
        if (IsVehicleWithinCameraRange())
        {
            myAgent.transform.position = transform.position;
        }
    }


    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        // visualWheel is parented to wheel collider, so we need to do a
        //little space transformation here
        rotation.eulerAngles += new Vector3(0,0,90);
        visualWheel.position = position;
        visualWheel.rotation = rotation;
    }
    WheelCollider GetCollider(int i)
    {
        ApplyLocalPositionToVisuals(wheelColliders[i]);
        return wheelColliders[i];
    }

    public void GetTarget()
    {
        if (myStats.isSelected)
        {
            if (Input.GetMouseButton(1))
            {
                //targetTransform = grid.GetClosestPosition(CameraFollow.GetMouseScreenToRay() + grid.transform.forward * (myEngine.minSpeedUpRange));
                targetTransform = grid.GetClosestPosition(CameraFollow.GetMouseScreenToRay() ,(int)(myEngine.minSpeedUpRange));

                if (targetTransform != null)
                {
                    target = targetTransform;
                }
            }
        }
    }
}
