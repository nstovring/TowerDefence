using UnityEngine;

using System.Collections;
using System.Collections.Generic;


//[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public Transform[] destinations = new Transform[4];
	[ SerializeField ] private Transform _transform;
	
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
                GameObject newObject = Instantiate(cellPrefab, cellposition, Quaternion.identity) as GameObject;
                //Transform newObject = new GameObject("GridPosition :" + i +","+ j).GetComponent<Transform>();
                newObject.transform.position = cellposition;
                newObject.transform.parent = transform;
                newObject.transform.localScale = Vector3.one/cellSize;
                GridMatrix[i, j] = newObject.transform;
            }
        }
        UpdateGrid();
	    
	}

    public float speed;
    private int destinationIncrement = 0;
    public void Update()
    {
        UpdateGrid();
        //foreach (var unit in unitsInGrid)
        //{
        //    unit.parent = transform;
        //}
        float step = speed * Time.deltaTime;
        transform.LookAt(target);
        Vector3 newVector3 = new Vector3(target.position.x,transform.position.y, target.position.z);
        if (Vector3.Distance(transform.position, newVector3) > 5)
        {
            Vector3 newDirection = newVector3 - transform.position; 
            transform.position += newDirection.normalized * step;
        }
        else if(destinations[0] != null)
        {
            SetDestination(destinations[destinationIncrement]);
            destinationIncrement++;
            destinationIncrement = destinationIncrement%4;
        }
    }

    public bool gridGenerated;
    public float navmeshRange = 0.3f;
    public Transform GetNearestCellOnNavmesh(Transform currentTransformTarget)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(currentTransformTarget.position, out hit, navmeshRange, NavMesh.AllAreas))
        {
            return currentTransformTarget;
        }
        return GetClosestPosition(currentTransformTarget);
    }

    public Transform GetClosestPosition(Transform targetedPoint)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (Vector3.Distance(GridMatrix[i, j].position, targetedPoint.position) < cellSize)
                {
                    //NavMeshHit hit;
                    //if (NavMesh.SamplePosition(GridMatrix[i, j].position, out hit, navmeshRange, NavMesh.AllAreas))
                    //{
                        return GridMatrix[i, j + 5];
                    //}
                }
            }
        }
        return targetedPoint;
    }

    public Transform GetClosestPosition(Vector3 targetedPoint)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (Vector3.Distance(GridMatrix[i, j].position, targetedPoint) < cellSize)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(GridMatrix[i, j].position, out hit, navmeshRange, NavMesh.AllAreas))
                    {
                        return GridMatrix[i, j ];
                    }
                }
            }
        }
        return null;
    }

    public void UpdateGrid()
	{
        //for (int i = 0; i < _rows; i++)
        //{
        //    for (int j = 0; j < _columns; j++)
        //    {
        //        Vector3 cellposition = new Vector3(i * cellSize, 0, j * cellSize) + (transform.position - transform.localScale * transform.localScale.x);
        //        //Transform newObject = new GameObject("GridPosition :" + i +","+ j).GetComponent<Transform>();
        //        GridMatrix[i, j].position = cellposition;
        //    }
        //}

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
