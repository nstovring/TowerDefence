using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Builder : MonoBehaviour {

    protected int objectSelectedNum = 0;
    protected bool isBuilding;
    protected GameObject currentPrefab;
    protected GameObject playerControlledUnit;
    protected List<GameObject> selectedObjects; 

    protected readonly KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!isBuilding && objectSelectedNum > 0)
        //{
        //    DisplaySelectedObject();
        //}
        //CheckForInput();
    }
    public delegate void onSpawn(GameObject target);
    public static event onSpawn OnSpawn;
    public void DisplaySelectedObject(List<GameObject> objects)
    {
        if (objectSelectedNum <= objects.Count)
        {
            currentPrefab = objects[objectSelectedNum - 1];
            currentPrefab.transform.position = UnitMovement.mouseToWorldPosition;
            currentPrefab.transform.rotation = UnitMovement.playerRotation;
        }
        if (Input.GetMouseButtonUp(0))
        {
            BuildObject();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetBuilding();
        }
    }

    public void BuildObject()
    {
        GameObject newObject = Instantiate(currentPrefab, currentPrefab.transform.position, currentPrefab.transform.rotation) as GameObject;
        newObject.layer = LayerMask.NameToLayer("m_Object");

        if (newObject.GetComponent<Stats>())
        newObject.GetComponent<Stats>().isAlive = true;
        OnSpawn(newObject);
        ResetBuilding();
    }

    public void ResetBuilding()
    {
        currentPrefab.transform.position = ObjectManager.instance.spawnYardPosition;
        currentPrefab.transform.rotation = Quaternion.identity;
        objectSelectedNum = 0;
    }

    public void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isBuilding = true;
            selectedObjects = ObjectManager.instance.turretPrefabs;
            Debug.Log("Offensive Building Mode On");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isBuilding = true;
            selectedObjects = ObjectManager.instance.obstaclePrefabs;
            Debug.Log("Defensive Building Mode On");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isBuilding = true;
            selectedObjects = ObjectManager.instance.buffPrefabs;
            Debug.Log("Buff Building Mode On");
        }
        if (isBuilding)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    objectSelectedNum = i + 1;
                    isBuilding = false;
                }
            }
        }
    }
}
