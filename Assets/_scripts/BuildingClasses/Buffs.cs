using UnityEngine;
using System.Collections;

public class Buffs : MonoBehaviour {
    public float range;
    public float prevRange;
    public delegate void changedState();
    public delegate void changedRange(GameObject target, float range);
    public delegate void increasedRange(GameObject target, float range);
    public delegate void decreasedRange(GameObject target, float range);
    public event changedState ChangedState;
    public event changedRange ChangedRange;
    public event increasedRange IncreasedRange;
    public event decreasedRange DecreasedRange;
    // Use this for initialization
    void Start () {
        range = 20;
        prevRange = range;
        Builder.OnSpawn += checkForDistance;
    }
	
	// Update is called once per frame
	void Update () {
        if (ChangedRange != null || IncreasedRange != null || DecreasedRange != null)
        {
            if (!prevRange.Equals(range))
            {
                Debug.Log("range changed");
                
                if(range < prevRange)
                {
                    if(DecreasedRange != null)
                    DecreasedRange(gameObject, range);
                }
                if (range > prevRange)
                {
                    if(IncreasedRange != null)
                    IncreasedRange(gameObject, range);
                }
                else
                {
                    if(ChangedRange != null)
                    ChangedRange(gameObject, range);
                }
                prevRange = range;
            }
        }
	}
    public void checkForDistance(GameObject target)
    {
        if (target.tag != "Buff")
        {
            float dist = (target.transform.position - gameObject.transform.position).magnitude;
            target.GetComponent<BuildingSpawn>().AddHandlers(this, range, dist);
        }
    }

}
