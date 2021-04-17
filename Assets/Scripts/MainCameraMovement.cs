using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{

    public float panSpeed = 20f;
    public float panBorderThickness = 20f;
    public float scrollSpeed = 100f;
    public Vector2 minmaxY = new Vector2(10, 100);

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        float deltaTime = Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            position.z += panSpeed * deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            position.z -= panSpeed * deltaTime;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= panSpeed * deltaTime;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            position.x += panSpeed * deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        position.y += -scroll * scrollSpeed * deltaTime;
        transform.position = position;
    }
}
