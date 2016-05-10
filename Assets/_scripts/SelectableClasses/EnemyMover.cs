using UnityEngine;
using System.Collections;

public class EnemyMover : Mover
{
	// Use this for initialization
	void Start ()
	{
	    InitializeMover();
	    target = GameObject.FindGameObjectWithTag(targetTag).transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (myStats.isAlive)
	    {
	        StartCoroutine(GoToTarget());
            return;
	    }
	    if (!myStats.isAlive)
	    {
	        StopMovement();
	    }
	}

    public override IEnumerator GoToTarget()
    {
        if (myStats.myAimer.curTarget)
        {
            target = myStats.myParty.grid.GetClosestPosition(myStats.myAimer.curTarget.transform);
            //target = myStats.myAimer.curTarget.transform;
            yield return null;
        }
        base.GoToTarget();
    }
}
