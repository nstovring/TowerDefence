﻿using UnityEngine;
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
            }
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