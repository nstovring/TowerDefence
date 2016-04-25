using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private float timePassed;
    public float spawnRate;
    public float roundRate = 10;
    public int spawnMax = 20;
    public int maxRounds = 2;
    private int currentSpawnAmount;
    private int currentRound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (currentSpawnAmount < spawnMax)
	    {
	        timePassed += Time.deltaTime;
	        if (timePassed > spawnRate)
	        {
	            timePassed = 0;
	            Spawn(prefab);
	        }
	    }
	    else if(currentRound < maxRounds)
	    {
	        timePassed += Time.deltaTime;
	        if (timePassed > roundRate)
	        {
	            timePassed = 0;
	            currentSpawnAmount = 0;
	            currentRound++;
	        }
	    }
	}

    public float spawnRadius = 5;

    public void Spawn(GameObject prefab)
    {
        //spawnRate = Random.Range(0.5f, 2);

        float rngXpos = Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);
        float rngZpos = Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius);

        Vector3 rnSpawnPosVector3 = new Vector3(rngXpos,transform.position.y,rngZpos);
        currentSpawnAmount++;
        GameObject clone = Instantiate(prefab, rnSpawnPosVector3, Quaternion.identity) as GameObject;
        clone.SetActive(true);
        clone.transform.name = "Enemy " + currentRound + ","+ currentSpawnAmount;
    }
}
