using UnityEngine;
using System.Collections;

public class TurretShooter : Shooter
{
    //public TurretStats myStats;

    // Use this for initialization
    void Start ()
    {
        Initialize();
    }
	
	// Update is called once per frame
	void Update () {
	    if (myStats.isAlive)
	    {
	        TakeAim();
	    }
	}

    public override void TakeAim()
    {
        if (myStats.myAimer.curTarget != null)
        {
            timePassed += Time.deltaTime;
            if (timePassed > fireRate && myStats.myAimer.curTarget != null)
            {
                timePassed = 0;
                Fire(projectilePrefab);
            }
        }
    }
   
}
