using UnityEngine;
using System.Collections;

public class RegularProjectile : Projectile
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (HitTarget())
        {
            HitEffect();
            Destroy(gameObject);
        }
    }

    public override void HitEffect()
    {
        base.HitEffect();
        target.ReceiveDamage(damage);
    }
}
