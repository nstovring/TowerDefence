using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

public abstract class Shooter : MonoBehaviour
{
    public Stats myStats;

    protected int damage;
    public float fireRate;
    public int launchForce;

    protected float timePassed;

    public Rigidbody projectilePrefab;

    public virtual void Initialize()
    {
        fireRate = myStats.fireRate;
        launchForce = myStats.launchForce;
        damage = myStats.damage;
    }

    public virtual void TakeAim()
    {
        timePassed += Time.deltaTime;
        if (timePassed > fireRate)
        {
            timePassed = 0;
            Fire(projectilePrefab);
        }
    }

    public virtual void Fire(Rigidbody projectile)
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        GameObject shellInstance = Instantiate(projectile.gameObject, transform.position, transform.rotation) as GameObject;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.GetComponent<Rigidbody>().velocity = launchForce * transform.forward;
        shellInstance.GetComponent<Projectile>().damage = damage;
        Destroy(shellInstance, 2);
    }
}
