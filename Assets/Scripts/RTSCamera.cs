using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
    private float _treshold;
    private float _speed;
    //FIXME
    private Rect _bounds = new Rect(-20, -20, 40, 40);
    public Options options;
	void Start () {
        _treshold = options.CameraTreshold;
        _speed = options.CameraSpeed;
    }
	
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        Vector2 movement = Vector2.zero;

        Rect screenR = new Rect(1, 1, Screen.width-1, Screen.height-1);
        if (!screenR.Contains(mousePos))
            return;
        if (mousePos.x < _treshold) movement.x = -1;
        else if (mousePos.x > screenR.xMax - _treshold) movement.x = 1;
        if (mousePos.y < _treshold) movement.y = -1;
        else if (mousePos.y > screenR.yMax - _treshold) movement.y = 1;

        if (transform.position.x < _bounds.xMin && movement.x == -1) movement.x = 0;
        else if (transform.position.x > _bounds.xMax && movement.x == 1) movement.x = 0;
        if (transform.position.y < _bounds.yMin && movement.y == -1) movement.y = 0;
        else if (transform.position.y > _bounds.yMax && movement.y == 1) movement.y = 0;
        movement = Camera.main.transform.TransformDirection(movement);
        Move(movement.normalized * _speed* Time.deltaTime);
    }
    void Move(Vector2 vec)
    {
        transform.Translate(transform.right* vec.x + Vector3.forward * vec.y,Space.World);
    }
}
