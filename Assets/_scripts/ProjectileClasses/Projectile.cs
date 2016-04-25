using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public int damage;
    public Stats target;
    protected float baseSpeed = 20f;
    public GameObject hitEffect;
    protected int layerMask;

    public AnimationCurve path;
    // Update is called once per frame
    void Update ()
	{
	    if (HitTarget())
	    {
            target.ReceiveDamage(damage);
	        Destroy(gameObject);
	    }
	}

    public void Initialize(string oppositionTag)
    {
        layerMask = LayerMask.NameToLayer(oppositionTag);
        layerMask = 1 << layerMask;
    }

    public void Initialize(string oppositionTag, Stats curTarget, int damage)
    {
        layerMask = LayerMask.NameToLayer(oppositionTag);
        layerMask = 1 << layerMask;
        target = curTarget;
        this.damage = damage;
    }

    public bool HitTarget()
    {
        if (target == null)
        {
            return false;
        }
        //Make several different projectile movement patterns
        ProjectilePath();

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            return true;
        }
        return false;
    }

    public virtual void ProjectilePath()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * baseSpeed);
    }

    public virtual void HitEffect()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);

        //Do something special when target is hit
    }

    void OnCollisionEnter(Collision other)
    {
        //Destroy(gameObject);
    }

}
