using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public GameObject cursor;
    public UnitManager um;

    private Texture2D _rectTex;
    private GUIStyle _rectStyle;
    public Rect _rect;
    private Vector2 _startSelectPoint;
    [SyncVar(hook = "OnselectChange")]
    public bool _selecting;
    //TODO: SET THIS
    public int PlayerID;
	void Start () {
        if (isLocalPlayer)
        {
            cursor.GetComponent<SpriteRenderer>().enabled = false;
        }
        _rectTex = new Texture2D(1, 1);
        _rectStyle = new GUIStyle();
        _rectTex.SetPixel(0, 0, new Color(0,230,50,0.5f));
        _rectTex.Apply();
        _rectStyle.normal.background = _rectTex;
        _selecting = false;
        um = FindObjectOfType<UnitManager>();
        um.Local = this;
        PlayerID = 1;
        if (isServer)
        {
            PlayerID = 0;
            um.SpawnUnits();
        }
    }
	void OnselectChange(bool val)
    {
        if (!isLocalPlayer)
        {
            if (val)
            {
                _startSelectPoint = cursor.transform.position;
                AdjustRect(Camera.main.WorldToScreenPoint(cursor.transform.position));
            }
            _selecting = val;

        }
    }
    [Command]
    public void CmdSelectUnit(uint id,int playerID)
    {
        RpcSelectUnit(id, playerID);
    }
    [ClientRpc]
    void RpcSelectUnit(uint id,int playerID)
    {
        if (PlayerID != playerID)
            um.SelectUnit(id, playerID);
    }
    // Update is called once per frame
    void Update () {
        if (isLocalPlayer)
        {
            Vector2 mousepos = Input.mousePosition;
            Vector3 v = Camera.main.ScreenToWorldPoint(mousepos);
            cursor.transform.position = new Vector2(v.x, v.y);
            if (_selecting)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _selecting = false;
                    CmdEndSelect();
                    um.SelectRect(_rect);
                }
                else
                {
                    AdjustRect(mousepos);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                _startSelectPoint = v;
                _rect = new Rect(new Vector2(mousepos.x,Screen.height-mousepos.y), Vector2.one*10);
                _selecting = true;
                CmdStartSelect();
            }
        }else if (_selecting)
        {
            AdjustRect(Camera.main.WorldToScreenPoint(cursor.transform.position));
        }
	}
    void AdjustRect(Vector2 mousepos)
    {
        Vector2 wordMouse = Camera.main.WorldToScreenPoint(_startSelectPoint);
        float reverseY = Screen.height - mousepos.y;
        float reverseMouseY = Screen.height - wordMouse.y;
        _rect.x = Mathf.Min(wordMouse.x, mousepos.x);
        _rect.y = Mathf.Min(reverseMouseY, reverseY);
        _rect.xMax = Mathf.Max(wordMouse.x, mousepos.x);
        _rect.yMax = Mathf.Max(reverseMouseY, reverseY);
    }
    [Command]
    void CmdStartSelect()
    {
        _selecting = true;
    }
    [Command]
    void CmdEndSelect()
    {
        _selecting = false;
    }
    void OnGUI()
    {
        if(_selecting)
            GUI.Box(_rect, GUIContent.none, _rectStyle);
    }
}
