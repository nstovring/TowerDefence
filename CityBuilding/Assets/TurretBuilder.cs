using UnityEngine;
using System.Collections;

public class TurretBuilder : Builder
{
   
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (!isBuilding && objectSelectedNum > 0)
        {
            DisplaySelectedObject(selectedObjects);
        }
        CheckForInput();
    }

    
}
