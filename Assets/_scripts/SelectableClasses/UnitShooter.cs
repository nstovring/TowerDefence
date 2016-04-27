using UnityEngine;
using System.Collections;

public class UnitShooter : Shooter {

	// Use this for initialization
	void Start () {
        Initialize();
    }
	
	// Update is called once per frame
	void Update () {
	    TakeAim();
	}

    public override void TakeAim()
    {
        UnitMovement myMovement = myStats.myMover as UnitMovement;
        if (Input.GetMouseButton(0) && myMovement.isInPlayerControl)
        {
            timePassed += Time.deltaTime;
            if (timePassed > fireRate)
            {
                timePassed = 0;
                Fire(projectilePrefab);
            }
        }
    }
}
