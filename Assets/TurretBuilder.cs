using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBuilder : Builder
{

    public static List<GameObject> builtTurrets;

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

    public override void BuildObject()
    {
        base.BuildObject();

    }
}
