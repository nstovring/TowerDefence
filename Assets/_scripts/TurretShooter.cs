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

    public override void Fire(Rigidbody projectile)
    {
        //base.Fire(projectile);
        GameObject shellInstance = Instantiate(projectile.gameObject, transform.position, transform.rotation) as GameObject;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        //shellInstance.GetComponent<Rigidbody>().velocity = launchForce * transform.forward;
        shellInstance.GetComponent<Projectile>().damage = damage;
        shellInstance.GetComponent<Projectile>().targetPos = myStats.myAimer.curTarget.transform;
        Destroy(shellInstance, 5);
    }
}
