using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public int damage;
    public Transform targetPos;
    private float baseSpeed = 20f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (targetPos != null)
	    {
	        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime*baseSpeed);
	    }
	    else
	    {
	        //Destroy(gameObject);
	    }
	}

    void OnCollisionEnter(Collision other)
    {
        //Destroy(gameObject);
    }
}
