using UnityEngine;
using System.Collections;

public class BuildingSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AddMethod();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CheckForDistance(GameObject target)
    {
        float dist = target.transform.position.magnitude - gameObject.transform.position.magnitude;
        if (dist < 200)
        {
            //do something
        }
    }
    public void AddMethod()
    {
        Builder.OnSpawn += CheckForDistance;
    }
    public void RemoveMethod()
    {
        Builder.OnSpawn -= CheckForDistance;
    }
}
