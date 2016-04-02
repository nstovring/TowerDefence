using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Aimer : MonoBehaviour {

    public float range = 20f;
    public Stats curTarget;
    public List<Stats> enemies;
    public string oppositionTag;
    protected int layerMask = 0;

    public virtual void Initialize()
    {
        layerMask = LayerMask.NameToLayer(oppositionTag);
        layerMask =  1 << layerMask;
    }

    public virtual void AimAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }

    public virtual void FindEnemies(string opposition, int layerMask)
    {
        if (!SelectEnemy())
        {
            //Create collision sphere
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.tag == opposition)
                {
                    Stats curEnemy = hitCollider.transform.GetComponent<Stats>();
                    //Check if enemy is alive
                    if (!curEnemy.isAlive)
                    {
                        if (enemies.Contains(curEnemy))
                        {
                            enemies.Remove(curEnemy);
                        }
                        //curTarget = null;
                        return;
                    }
                    if (!enemies.Contains(curEnemy))
                    {
                        enemies.Add(curEnemy);
                        return;
                    }
                }
            }
        }
    }

    public virtual bool SelectEnemy()
    {
        if (curTarget)
        {
            //Check if target is within range
            if (Vector3.Distance(curTarget.transform.position, transform.position) > range)
            {
                enemies.RemoveAt(0);
                curTarget = null;
                return false;
            }
            //Check if target is dead
            if (!curTarget.isAlive)
            {
                enemies.RemoveAt(0);
                curTarget = null;
            }
        }
        if (!curTarget && enemies.Count != 0)
        {
            //Always choose the first enemy detected
            curTarget = enemies[0];
            return true;
        }
        return false;
    }

}
