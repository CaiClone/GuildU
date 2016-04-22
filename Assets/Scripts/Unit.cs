using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit : NetworkBehaviour {

    public bool Selectable;
    public int _selected;
    public Vector2 ScreenPos;
    public uint id;

    void Start ()
    {
        Selectable = true;
        _selected = 0;
    }
	
	void Update () {
        Vector2 screenRev = Camera.main.WorldToScreenPoint(transform.position);
        ScreenPos = new Vector2(screenRev.x,Screen.height- screenRev.y);
    }
    public void Select(int player)
    {
        _selected |= 1 >> player;
    }
    public void Deselect(int player)
    {
        _selected &= ~(1 << player); 
    }
    void OnGUI()
    {
        if (_selected!=0)
        {
            GUI.Box(new Rect(ScreenPos.x-20, ScreenPos.y - 20, 40, 40), "faggot");
        }
    }
}
