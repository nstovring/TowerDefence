using UnityEngine;

using System.Collections;
using System.Collections.Generic;


//[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
	
	[ SerializeField ] private Transform _transform;
	
	[ SerializeField ] private Material _material;
	
	[ SerializeField ] private Vector2 _gridSize;
	
	[ SerializeField ] private int _rows;
	
	[ SerializeField ] private int _columns;

    public GameObject cellPrefab;

    public List<Transform> unitsInGrid;

    private Transform[,] GridMatrix;

    public Transform target;
    public float cellSize = 1;

    public string targetTag;


    public void SetDestination(Transform destTransform)
    {
        target = destTransform;
    }

	void Start()
	{
        if(!target)
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        GridMatrix = new Transform[_rows,_columns];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                Vector3 cellposition = new Vector3(i * cellSize, 0, j * cellSize) + (transform.position - transform.localScale * transform.localScale.x);
                Transform newObject = new GameObject("GridPosition :" + i +","+ j).GetComponent<Transform>();
                newObject.position = cellposition;
                newObject.parent = transform;
                GridMatrix[i, j] = newObject;
            }
        }
        UpdateGrid();
	    
	}

    public float speed;

    public void Update()
    {
        UpdateGrid();
        //foreach (var unit in unitsInGrid)
        //{
        //    unit.parent = transform;
        //}
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    public bool gridGenerated;

    public Transform GetClosestPosition(Vector3 targetedPoint)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (Vector3.Distance(GridMatrix[i, j].position, targetedPoint) < cellSize)
                {
                    return GridMatrix[i, j];
                }
            }
        }
        return null;
    }

    public void UpdateGrid()
	{
       

       // _transform.localScale = new Vector3( _gridSize.x, _gridSize.y, 1.0f );
		
		//_material.SetTextureScale( "_MainTex", new Vector2( _columns, _rows ) );
	}

    //void OnDrawGizmosSelected()
    //{
    //    UpdateGrid();
    //    for (int i = 0; i < _rows; i++)
    //    {
    //        for (int j = 0; j < _columns; j++)
    //        {
    //            Gizmos.DrawCube(GridMatrix[i, j], Vector3.one);
    //        }
    //    }
    //}
}
