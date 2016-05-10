using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Aimer : MonoBehaviour {

    public float range = 20f;
    public Stats curTarget;
    public List<Stats> enemies;
    public string oppositionTag;
    protected int layerMask = 0;
    public LayerMask myAimableLayerMask;

    public virtual void Initialize()
    {
        layerMask = LayerMask.NameToLayer("isSelectable");
        layerMask = myAimableLayerMask;
        //layerMask =  1 << layerMask;
    }

    public virtual void AimAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }

    public virtual IEnumerator FindEnemies(string opposition, int layerMask)
    {
        while (true)
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
                        //Check if enemy detected is alive should update enemy with removal of collider btw
                        if (!curEnemy.isAlive)
                        {
                            if (enemies.Contains(curEnemy))
                            {
                                enemies.Remove(curEnemy);
                            }
                            yield return new WaitForFixedUpdate();
                        }
                        if (!enemies.Contains(curEnemy))
                        {
                            enemies.Add(curEnemy);
                            yield return new WaitForFixedUpdate();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(2);
        }
    }

    public virtual bool SelectEnemy()
    {
        if (curTarget)
        {
            //Check if target is within range
            if (Vector3.Distance(curTarget.transform.position, transform.position) > range)
            {
                RemoveEnemy(curTarget);
                return false;
            }
            //Check if target is dead
            if (!curTarget.isAlive)
            {
                RemoveEnemy(curTarget);
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

    public void RemoveEnemy(Stats deadEnemy)
    {
        if (curTarget == deadEnemy)
        {
            curTarget = null;
        }
        if (enemies.Contains(deadEnemy))
        {
            enemies.Remove(deadEnemy);
        }
    }

}
