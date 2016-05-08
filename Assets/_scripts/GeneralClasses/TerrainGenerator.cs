using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{

    public Vector3 spawnPoint;
    public GameObject groundPrefab;
    public GameObject[] enviromentPrefabs;
    public int prefabAmount;
    public float scaleMax;

    [Range(0,10)]
    public int treePercentage;
    [Range(0, 10)]
    public int rockPercentage;
    [Range(0, 10)]
    public int dunePercentage;
    [Range(0, 10)]
    public int cactusPercentage;
    [Range(0, 10)]
    public int collumnPercentage;
    [Range(0, 10)]
    public int concretePercentage;

    private GameObject terrainGameObject;

    public int groundScale;

    public void GenerateTerrain()
    {
        if (terrainGameObject != null)
        {
            DestroyImmediate(terrainGameObject);
        }
        spawnPoint = transform.position;
        float groundSize = groundPrefab.transform.localScale.z * groundScale;
        terrainGameObject = Instantiate(groundPrefab, spawnPoint, Quaternion.identity) as GameObject;
        terrainGameObject.transform.localScale *= transform.localScale.z;
        terrainGameObject.transform.parent = transform;
        terrainGameObject.isStatic = true;
        List<GameObject> enviromentList = WeightedList();
        for (int i = 0; i < prefabAmount; i++)
        {
            Quaternion randomQuaternion = Quaternion.Euler(new Vector3(0, Random.Range(0, 355), 0));
            GameObject prefab = Instantiate(enviromentList[Random.Range(0, enviromentList.Count)],
                new Vector3(Random.Range(spawnPoint.x - groundSize, spawnPoint.x + groundSize),
                    spawnPoint.y,
                    Random.Range(spawnPoint.z - groundSize, spawnPoint.z + groundSize)), randomQuaternion) as GameObject;
            prefab.transform.localScale *= Random.Range(1f, scaleMax);
            prefab.transform.parent = terrainGameObject.transform;
            prefab.isStatic = true;
        }
    }

    public List<GameObject> WeightedList()
    {
        List<GameObject> tempList = new List<GameObject>();

        int[] weightInts = { treePercentage, treePercentage, rockPercentage, rockPercentage ,
            rockPercentage , dunePercentage, dunePercentage, cactusPercentage, collumnPercentage,
            collumnPercentage, concretePercentage};

        for (int i = 0; i < weightInts.Length; i++)
        {
            for (int j = 0; j < weightInts[i]; j++)
            {
                tempList.Add(enviromentPrefabs[i]);
            }
        }

        return tempList;
    }
}
