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
    }

    public void StopMovement()
    {
        myAgent.Stop();
    }

    public void GotToTarget()
    {
        if (target == null)
        {
            return;
        }
        myAgent.SetDestination(target.position);
    }
}
