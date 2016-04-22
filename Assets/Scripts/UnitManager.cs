using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UnitManager : NetworkBehaviour {
    public List<Unit> SelectableUnits;
    public List<Unit> SelectedUnits;
    public Player Local;
    //TODO GET THIS
	void Start () {
        SelectableUnits = new List<Unit>();
        SelectedUnits = new List<Unit>();
    }
    public void SpawnUnits()
    {
        GameObject gob = Instantiate(WareHouse.get().UnitPrefabs["Guardian"]);
        NetworkServer.Spawn(gob);
        SelectableUnits.Add(gob.GetComponent<Unit>());
        for (uint i = 0; i < SelectableUnits.Count; i++)
            SelectableUnits[(int)i].id = i;
    }
	void Update () {
	
	}
    public void SelectRect(Rect rect)
    {
        DeselectAll();
        foreach (Unit u in SelectableUnits)
            if (rect.Contains(u.ScreenPos))
                SelectUnit(u);
    }
    public void SelectUnit(uint id,int playerid)
    {
        foreach (Unit u in SelectableUnits)
            if (u.id == id)
                u.Select(playerid);
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
            u.Deselect(Local.PlayerID);
        SelectedUnits.Clear();
    }
}
