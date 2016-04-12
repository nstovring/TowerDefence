using UnityEngine;
using System.Collections;

public class Enemy : Stats
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (health <= 0 && isAlive)
	    {
	        isAlive = false;
	        foreach (GameObject turretGameObject in BuildingSpawn.buildingsSpawned)
	        {
	            turretGameObject.SendMessage("RemoveEnemy", this);
                Debug.Log("Im dead remove me");
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
