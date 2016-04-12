using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingSpawn : MonoBehaviour
{

    public static List<GameObject> buildingsSpawned; 
	// Use this for initialization
	void Start () {
        Builder.OnSpawn += CheckForDistance;
        buildingsSpawned = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CheckForDistance(GameObject target)
    {
        //Temp code placement here
        buildingsSpawned.Add(target);
        //

        float dist = (target.transform.position - gameObject.transform.position).magnitude;
        if (target.tag == "Buff")
        {
            Buffs buff = target.GetComponent<Buffs>();
            if (dist < buff.range)
            {
                Debug.Log(gameObject.name + " has " + dist + " units to " + target.name);
                buff.DecreasedRange += DecreasedRangeHandler;
                //do something
            }
            else
            {
                buff.IncreasedRange += IncreasedRangeHandler;
            }
        }
    }
    public void IncreasedRangeHandler(GameObject target, float range)
    {
        CheckForDistance(target, range);
    }
    public void DecreasedRangeHandler(GameObject target, float range)
    {
        CheckIfOutsideRange(target, range);
    }
    public void CheckForDistance(GameObject target, float range)
    {
        Debug.Log(gameObject.name + " checking for changed range");
        float dist = (target.transform.position - gameObject.transform.position).magnitude;
        if (target.tag == "Buff")
        {
            Buffs buff = target.GetComponent<Buffs>();
            if (dist < buff.range)
            {
                Debug.Log(gameObject.name + " has " + dist + " units to " + target.name);
                Debug.Log(gameObject.name + " is now inside range");
                buff.DecreasedRange += DecreasedRangeHandler;
                buff.IncreasedRange -= IncreasedRangeHandler;
                //do something
            }
        }
    }
    public void CheckIfOutsideRange(GameObject target, float range)
    {
        float dist = (target.transform.position - gameObject.transform.position).magnitude;
        if (dist <= range)
        {
            Debug.Log(gameObject.name + " still inside");
            //do something
        }
        else
        {
            Debug.Log(gameObject.name + " is now outside range");
            target.GetComponent<Buffs>().DecreasedRange -= DecreasedRangeHandler;
            target.GetComponent<Buffs>().IncreasedRange += IncreasedRangeHandler;
        }
    }
    public void AddHandlers(Buffs sender, float range, float distance)
    {
        Debug.Log("adding handlers");
        if(distance <= range)
        {
            sender.DecreasedRange += DecreasedRangeHandler;
        }
        else
        {
            sender.IncreasedRange += IncreasedRangeHandler;
        }
    }
    public void AddMethod()
    {
        Builder.OnSpawn += CheckForDistance;
    }
    public void RemoveMethod()
    {
        Builder.OnSpawn -= CheckForDistance;
    }
}
