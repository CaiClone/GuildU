using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public GameObject cursor;
    public UnitManager um;

    private Texture2D _rectTex;
    private GUIStyle _rectStyle;
    public Rect _rect;
    private Vector3 _startSelectPoint;
    [SyncVar(hook = "OnselectChange")]
    public bool _selecting;
    //TODO: SET THIS
    public int PlayerID;
	void Start ()
    {
        Color col;
        um = UnitManager.Instance;
        if (isLocalPlayer)
        {
            um.Local = this;
            cursor.GetComponent<SpriteRenderer>().enabled = false;
            col = new Color(0, 230, 50, 0.5f);
            if (isServer)
            {
                um.SpawnUnits();
                PlayerID = 0;
            }
            else
            {
                PlayerID = 1; //FIXME for more than 2 players
            }
        }
        else
        {
            if (isServer)
                PlayerID = 1;
            else PlayerID = 0;
            col = new Color(250, 50, 0, 0.5f);
            cursor.transform.rotation = Camera.main.transform.rotation;        
        }

        _rectTex = new Texture2D(1, 1);
        _rectStyle = new GUIStyle();
        _rectTex.SetPixel(0, 0, col);
        _rectTex.Apply();
        _rectStyle.normal.background = _rectTex;
        _selecting = false;
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
        if(playerID==PlayerID)
            um.SelectUnit(id, playerID);
    }
    [Command]
    public void CmdDeselectUnit(uint id, int playerID)
    {
        RpcDeselectUnit(id, playerID);
    }
    [ClientRpc]
    void RpcDeselectUnit(uint id, int playerID)
    {
        if (playerID == PlayerID)
            um.DeselectUnit(id, playerID);
    }
    // Update is called once per frame
    void Update () {
        if (isLocalPlayer)
        {
            Vector2 mousepos = Input.mousePosition;
            Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, 1));
            cursor.transform.position = v;
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
            if (Input.GetMouseButtonDown(1))
            {
                Ray r = Camera.main.ScreenPointToRay(mousepos);
                RaycastHit hit;
                if(Physics.Raycast(r,out hit)) //TODO LAYERS
                    um.Move(hit.point);
            }
        }else if (_selecting)
        {
            AdjustRect(Camera.main.WorldToScreenPoint(cursor.transform.position));
        }
	}
    void AdjustRect(Vector2 mousepos)
    {
        Vector3 wordMouse = Camera.main.WorldToScreenPoint(_startSelectPoint);
        float reverseY = Screen.height - mousepos.y;
        float reverseMouseY = Screen.height - wordMouse.y;
        //OPTIMIZE
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

    [Command]
    public void CmdSetMoveTarget(Vector3 pos,uint[] units)
    {
        RpcSetTarget(pos,units);
    }
    [ClientRpc]
    public void RpcSetTarget(Vector3 pos, uint[] units)
    {
        um.RpcSetTarget(pos, units);
    }
    void OnGUI()
    {
        if(_selecting)
            GUI.Box(_rect, GUIContent.none, _rectStyle);
    }
}
