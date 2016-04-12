using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Stats
{
    private GameObject[] turretsSpawned;
	// Use this for initialization
	void Start ()
	{
	    turretsSpawned = GameObject.FindGameObjectsWithTag("Turret");
	}
	
	// Update is called once per frame
	void Update () {
	    if (health <= 0 && isAlive)
	    {
	        GetComponent<Collider>().enabled = false;
            turretsSpawned = GameObject.FindGameObjectsWithTag("Turret");
            isAlive = false;
            foreach (GameObject turretGameObject in turretsSpawned)
            {
                turretGameObject.GetComponent<Stats>().myAimer.RemoveEnemy(this);
                //turretGameObject.SendMessage("RemoveEnemy", this);
            }
            Destroy(gameObject,2);
	    }
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "m_Projectile")
        {
            health -= other.transform.GetComponent<Projectile>().damage;
            Destroy(other.transform.gameObject);
        }
    }
}
