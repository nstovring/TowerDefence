using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public Stats myStats;
    protected NavMeshAgent myAgent;
    public Transform target;
    public float currentVelocity;

    public string targetTag;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitializeMover()
    {
        if (GetComponent<NavMeshAgent>())
        {
            myAgent = GetComponent<NavMeshAgent>();
        }
        else
        {
            myAgent = GetComponentInChildren<NavMeshAgent>();
        }
        myStats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody>();
    }

    public void StopMovement()
    {
        myAgent.Stop();
    }

    public virtual IEnumerator GoToTarget()
    {
        if (target == null)
        {
            yield return null;
        }
        if(myAgent.isOnNavMesh)
        myAgent.SetDestination(target.position);
        yield return new WaitForFixedUpdate();
    }

    private NavMeshPath myPath;

    protected Rigidbody rb;

    public virtual void GotToTargetVelocity()
    {
        if (target == null)
        {
            return;
        }
    }
}
