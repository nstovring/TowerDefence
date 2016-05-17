using UnityEngine;
using System.Collections;

public class TurretStats : Stats
{

    //public TurretAimer myAimer;
    //public TurretShooter myShooter;
    // Use this for initialization
    void Start () {
        myMover = GetComponent<Mover>();
	
	}
	
	// Update is called once per frame
	void Update () {
        DisplaySelectedIcon();
	}

    
}
