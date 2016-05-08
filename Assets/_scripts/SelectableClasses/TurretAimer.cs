using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretAimer : Aimer
{
    public Stats myStats;


	// Use this for initialization
	void Start ()
	{
	    range = myStats.range;
	    oppositionTag = myStats.oppositionTag;
        Initialize();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (myStats.isAlive)
	    {
	        //Find enemies
	        FindEnemies(oppositionTag, layerMask);
	        if (curTarget)
	        {
	            AimAtTarget(curTarget.transform);
	        }
	    }
	}

}
