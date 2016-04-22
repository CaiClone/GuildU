using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WareHouse {
    static readonly Dictionary<string, string> pathMap = new Dictionary<string, string>
    {
        {"UnitPrefab","Units/" }
    };
    private static WareHouse _instance;
    public Dictionary<string, GameObject> UnitPrefabs;
    private WareHouse()
    {
        UnitPrefabs = new Dictionary<string, GameObject>();
        foreach (GameObject gob in Resources.LoadAll<GameObject>(pathMap["UnitPrefab"]))
            UnitPrefabs.Add(gob.name, gob);

    }
    public static WareHouse get()
    {
        if (_instance == null)
            _instance = new WareHouse();
        return _instance;
    }
}
