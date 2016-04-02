using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    private NavMeshAgent myAgent;
    public Stats myStats;
    public Transform target;
	// Use this for initialization
	void Start ()
	{
	    myAgent = GetComponent<NavMeshAgent>();
	    target = GameObject.FindGameObjectWithTag("m_Base").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (myStats.isAlive)
	    {
	        FindTarget();
            return;
	    }
	    if (!myStats.isAlive)
	    {
	        StopMovement();
	    }
	}

    public void StopMovement()
    {
        myAgent.Stop();
    }

    void FindTarget()
    {
        if (target == null)
        {
            return;
        }
        myAgent.SetDestination(target.position);
    }
}
