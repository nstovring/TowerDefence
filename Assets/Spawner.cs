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

    private float spawnRadius = 5;

    public void Spawn(GameObject prefab)
    {
        //spawnRate = Random.Range(0.5f, 2);

        float rngXpos = Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);
        float rngZpos = Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius);

        Vector3 rnSpawnPosVector3 = new Vector3(rngXpos,transform.position.y,rngZpos);
        currrentSpawnAmount++;
        GameObject clone = Instantiate(prefab, rnSpawnPosVector3, Quaternion.identity) as GameObject;
        clone.GetComponent<Stats>().isAlive = true;
        clone.SetActive(true);
        clone.transform.name = "Enemy " +currrentSpawnAmount;
    }
}
