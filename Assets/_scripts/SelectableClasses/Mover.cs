using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public Stats myStats;
    protected NavMeshAgent myAgent;
    public Transform target;

    public string targetTag;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitializeMover()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody>();
    }

    public void StopMovement()
    {
        myAgent.Stop();
    }

    public virtual void GotToTarget()
    {
        if (target == null)
        {
            return;
        }
        if(myAgent.isOnNavMesh)
        myAgent.SetDestination(target.position);
    }

    private NavMeshPath myPath;

    private Rigidbody rb;

    public virtual void GotToTargetVelocity()
    {
        if (target == null)
        {
            return;
        }
    }
}
