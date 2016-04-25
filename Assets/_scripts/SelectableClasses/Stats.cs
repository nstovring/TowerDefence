using UnityEngine;
using System.Collections;

public abstract class Stats : MonoBehaviour {

    public bool isAlive = false;
    public bool isSelected = false;

    public int health = 100;
    public float range = 20f;
    public float fireRate = 1;
    public int launchForce = 10;
    public int damage = 10;

    public string oppositionTag;

    public Aimer myAimer;
    public Shooter myShooter;
    public Mover myMover;

    public Texture myHealthBarSprite;
    private GameObject healthBarHolder;

    public virtual void Initialize()
    {
        isAlive = true;
        healthBarHolder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        healthBarHolder.transform.parent = gameObject.transform;
        healthBarHolder.transform.localScale = new Vector3(1,0.2f,0.2f);
        healthBarHolder.transform.localPosition = Vector3.zero + Vector3.up * 2;
    }

    public void ResizeHealthBar()
    {
        //int maxWidth = 100;
        healthBarHolder.transform.localScale = new Vector3(1 * (health/100f), 0.2f, 0.2f);
        //myHealthBarSprite.rect.size = (0.1f*maxWidth)*health;
    }

    public virtual void ReceiveDamage(int _damage)
    {
        health = health> 0 ? health -= _damage : health ;
        health = health < 0 ? health = 0 : health;
    }

    public void OnDrawGizmosSelected()
    {
        //Display Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
