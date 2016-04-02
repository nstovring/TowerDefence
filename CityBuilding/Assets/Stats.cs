using UnityEngine;
using System.Collections;

public abstract class Stats : MonoBehaviour {

    public bool isAlive = false;

    public int health = 100;
    public float range = 20f;
    public float fireRate = 1;
    public int launchForce = 10;
    public int damage = 10;

    public string oppositionTag;

    public Aimer myAimer;
    public Shooter myShooter;

    public void OnDrawGizmosSelected()
    {
        //Display Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
