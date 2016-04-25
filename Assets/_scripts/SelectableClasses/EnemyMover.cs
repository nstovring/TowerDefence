using UnityEngine;
using System.Collections;

public class EnemyMover : Mover
{
	// Use this for initialization
	void Start ()
	{
	    InitializeMover();
	    target = GameObject.FindGameObjectWithTag("m_Base").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (myStats.isAlive)
	    {
	        GotToTarget();
            return;
	    }
	    if (!myStats.isAlive)
	    {
	        StopMovement();
	    }
	}

  
}
