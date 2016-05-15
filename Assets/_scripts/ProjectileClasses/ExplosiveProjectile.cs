using UnityEngine;
using System.Collections;

public class ExplosiveProjectile : Projectile
{
    public float explosionRange = 5;

    // Use this for initialization
    void Start ()
    {
    }

	// Update is called once per frame
	void Update () {
        if (HitTarget())
        {
            HitEffect();
            //target.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }

    public override void ProjectilePath()
    {
        base.ProjectilePath();
        // Do something different here
        //float curdistance = Vector3.Distance(transform.position, target.transform.position);
        //float curTimeInPath = (startDistance / curdistance);
        //Vector3 pathVector3 = new Vector3(target.transform.position.x, target.transform.position.y + path.Evaluate(curTimeInPath), target.transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, pathVector3, Time.deltaTime * baseSpeed);
    }

    public override void HitEffect()
    {
        base.HitEffect();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange, layerMask);
        foreach (var enemy in hitColliders)
        {
            enemy.GetComponent<Stats>().ReceiveDamage(damage);
        }
    }
}
