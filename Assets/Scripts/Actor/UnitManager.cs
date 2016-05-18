using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnitManager : NetworkBehaviour {
    public Dictionary<uint,Unit> Units;
    public List<Unit> SelectedUnits;
    public Player Local;
    public static UnitManager Instance;

	void Start () {
        Units = new Dictionary<uint,Unit>();
        SelectedUnits = new List<Unit>();
        Instance = this;
    }
    public void SpawnUnits()
    {
        Units.Clear();
        GameObject gob = Instantiate(WareHouse.get().UnitPrefabs["Unit"]);
        NetworkServer.Spawn(gob);
        GameObject gob2 = Instantiate(WareHouse.get().UnitPrefabs["Unit"]);
        NetworkServer.Spawn(gob2);
    }
	void Update () {
	
	}
    uint[] getIdSelected()
    {
        uint[] sel = new uint[SelectedUnits.Count];
        for(int i = 0;i<SelectedUnits.Count;i++)
        {
            sel[i] = SelectedUnits[i].id;
        }
        return sel;
    }
    public void Move(Vector3 pos)
    {
        Local.CmdSetMoveTarget(pos,getIdSelected());
    }
    public void RpcSetTarget(Vector3 pos,uint[] units)
    {
        foreach (uint u in units)
            Units[u].SetTarget(pos);
    }
    public void AddSelectable(Unit u)
    {
        u.id = (uint)Units.Count;
        Units.Add(u.id, u);
    }
    public void SelectRect(Rect rect)
    {
        DeselectAll();
        foreach (Unit u in Units.Values)
            if (u.selectable && rect.Contains(u.ScreenPos))
                SelectUnit(u);
    }
    public void SelectUnit(uint id,int playerid)
    {
        Units[id].Select(playerid);
    }
    public void DeselectUnit(uint id, int playerid)
    {
        Units[id].Deselect(playerid);
    }
    void SelectUnit(Unit u)
    {
        Local.CmdSelectUnit(u.id,Local.PlayerID);
        u.Select(Local.PlayerID);
        SelectedUnits.Add(u);
    }
    void DeselectAll()
    {
        foreach (Unit u in SelectedUnits)
        {
            u.Deselect(Local.PlayerID);
            Local.CmdDeselectUnit(u.id,Local.PlayerID);
        }
        SelectedUnits.Clear();
    }
}
