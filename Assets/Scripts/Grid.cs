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

    public Party myParty;
    public Transform target;
    public float cellSize = 1;

    public string targetTag;
    [HideInInspector]
    public Rigidbody rb;

    public void SetDestination(Transform destTransform)
    {
        target = destTransform;
    }

    //private VehicleMover leaderMover;

	void Start()
	{
	    rb = GetComponent<Rigidbody>();
	    //leaderMover = myParty.partyLeader.GetComponent<VehicleMover>();
        if(!target)
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        GridMatrix = new Transform[_rows,_columns];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                Vector3 cellposition = new Vector3(i * cellSize, 0, j * cellSize) + (transform.position - transform.localScale * transform.localScale.x);
                GameObject newObject = Instantiate(cellPrefab, cellposition, Quaternion.identity) as GameObject;
                newObject.transform.position = cellposition;
                newObject.transform.parent = transform;
                newObject.transform.localScale = Vector3.one/cellSize;
                GridMatrix[i, j] = newObject.transform;
            }
        }
	}

    public float speed;
    private int destinationIncrement = 0;
    public void FixedUpdate()
    {
        //speed = (leaderMover.currentVelocity/speed)*speed;
        float step = speed * Time.deltaTime;

        Vector3 newVector3 = new Vector3(target.position.x,transform.position.y, target.position.z);

        //rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 1 * Time.deltaTime));

        if (Vector3.Distance(transform.position, newVector3) > 5)
        {
            //Vector3 newDirection = newVector3 - transform.position; 
            //rb.MovePosition(transform.position + newDirection.normalized * step);
        }
        else if(destinations[0] != null)
        {
            SetDestination(destinations[destinationIncrement]);
            destinationIncrement++;
            destinationIncrement = destinationIncrement%destinations.Length;
        }
    }

    public float navmeshRange = 0.3f;
    public Transform GetNearestCellOnNavmesh(Transform currentTransformTarget)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(currentTransformTarget.position, out hit, navmeshRange, NavMesh.AllAreas))
        {
            StartCoroutine(flashCell(currentTransformTarget));
            return currentTransformTarget;
        }
        return GetClosestPosition(currentTransformTarget);
    }

    IEnumerator flashCell(Transform cell)
    {
        float timeFlash = 3f;
        while (timeFlash > 0)
        {
            float colorValue = Mathf.Sin(Time.time * 5);//Mathf.PingPong(Time.time, 255);
            Color lerpColor = new Color(0, 0, 0, colorValue);

            cell.GetComponent<MeshRenderer>().material.color = lerpColor;
            //lerpColor = Color.
            timeFlash -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cell.GetComponent<MeshRenderer>().material.color = new Color(0,0,0,0);
        yield return null;
    }

    public Transform GetClosestPosition(Transform targetedPoint)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (Vector3.Distance(GridMatrix[i, j].position, targetedPoint.position) < cellSize)
                {
                    StartCoroutine(flashCell(GridMatrix[i, j]));
                    return GridMatrix[i, j];
                }
            }
        }
        StartCoroutine(flashCell(targetedPoint));
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
                    //NavMeshHit hit;
                    //if (NavMesh.SamplePosition(GridMatrix[i, j].position, out hit, navmeshRange, NavMesh.AllAreas))
                    //{        StartCoroutine(flashCell(targetedPoint));

                    return GridMatrix[i , j ];
                    //}
                }
            }
        }
        return null;
    }

    public Transform GetClosestPosition(Vector3 targetedPoint, int offset)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (Vector3.Distance(GridMatrix[i, j].position, targetedPoint) < cellSize)
                {
                    //NavMeshHit hit;
                    //if (NavMesh.SamplePosition(GridMatrix[i, j].position, out hit, navmeshRange, NavMesh.AllAreas))
                    //{
                    StartCoroutine(flashCell(GridMatrix[i, j]));
                    int offsetCell = Mathf.Clamp(j + offset/2, 0, GridMatrix.GetLength(1));
                    StartCoroutine(flashCell(GridMatrix[i, offsetCell]));

                    return GridMatrix[i, offsetCell];
                    //}
                }
            }
        }
        return null;
    }
}
