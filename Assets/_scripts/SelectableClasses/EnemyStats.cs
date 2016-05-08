using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : Stats
{
    private GameObject[] turretsSpawned;
	// Use this for initialization
	void Awake ()
	{
        Initialize();
        turretsSpawned = GameObject.FindGameObjectsWithTag("Turret");
        //Destroy(transform.GetChild(0).gameObject,2);
	}

    public GameObject explosionPrefab;

	// Update is called once per frame
	void Update () {
	    if (health <= 0 && isAlive)
	    {
	        GetComponent<Collider>().enabled = false;
            turretsSpawned = GameObject.FindGameObjectsWithTag("Player");
            isAlive = false;
            foreach (GameObject turretGameObject in turretsSpawned)
            {
                turretGameObject.GetComponent<Stats>().myAimer.RemoveEnemy(this);
            }

	        Transform[] childrenTransforms = GetComponentsInChildren<Transform>();

	        foreach (var childrenTransform in childrenTransforms)
	        {
	            childrenTransform.gameObject.AddComponent<Rigidbody>();
	            childrenTransform.parent = null;
	        }

	        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject,2);
	    }
        ResizeHealthBar();
	}

    void OnCollisionEnter(Collision other)
    {
        //if (other.transform.tag == "m_Projectile")
        //{
        //    health -= other.transform.GetComponent<Projectile>().damage;
        //    Destroy(other.transform.gameObject);
        //}
    }
}
