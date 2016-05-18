using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit : NetworkBehaviour {
    public Vector2 ScreenPos;
    public uint id;
    private NavMeshAgent _navAgent;
    public bool selectable = false;
    public virtual void Start()
    {
        id = (uint)UnitManager.Instance.Units.Count; //FIXME
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.updateRotation = false;
    }
	
	public virtual void Update () {
        Vector2 screenRev = Camera.main.WorldToScreenPoint(transform.position);
        ScreenPos = new Vector2(screenRev.x,Screen.height- screenRev.y);
    }
    public void SetTarget(Vector3 pos)
    {
        _navAgent.SetDestination(pos);
    }
    public virtual void Select(int player)
    {
        //SHHT
    }
    public virtual void Deselect(int player)
    {
        //SHHT
    }
}
