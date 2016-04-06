using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public List<GameObject> turretPrefabs;
    public List<GameObject> obstaclePrefabs;
    public List<GameObject> buffPrefabs;
    public Vector3 spawnYardPosition;

    public static ObjectManager instance;

	// Use this for initialization
	void Start ()
	{
	    instance = this;
	    spawnYardPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
