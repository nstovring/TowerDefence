using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour
{
    public List<Stats> partyMembers;
    public Stats partyLeader;
    public Grid grid;

    public Transform partyDestination;

	// Use this for initialization
	void Start () {
	    //foreach (var partyMember in partyMembers)
	    //{
	    //   if(partyMember.isLeader)
     //           partyMember.myParty.
	    //}
	}

    void GetGridDestination(Transform dest)
    {
        partyDestination = dest;
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    void LateUpdate()
    {
        //foreach (var partyMember in partyMembers)
        //{
        //    if (!partyMember.isLeader)
        //    {
        //        var engine = partyMember.myMover as VehicleMover;
        //        engine.myEngine.standardVelocity = partyLeader.myMover.currentVelocity;
        //    }
        //}
    }
}
