using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private float timePassed;
    public float spawnRate;
    public int spawnMax = 20;
    private int currrentSpawnAmount;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (currrentSpawnAmount < spawnMax)
	    {
	        timePassed += Time.deltaTime;
	        if (timePassed > spawnRate)
	        {
	            timePassed = 0;
	            Spawn(prefab);
	        }
	    }
	}

    public void Spawn(GameObject prefab)
    {
        //spawnRate = Random.Range(0.5f, 2);
        currrentSpawnAmount++;
        GameObject clone = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
        clone.GetComponent<Stats>().isAlive = true;
        clone.SetActive(true);
        clone.transform.name = "Enemy " +currrentSpawnAmount;
    }
}
