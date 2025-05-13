using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

public class CameraMove : MonoBehaviour
{
    public GameObject viewField;

    private bool isPressed;
    private Vector3 prevMousePosition;
    private Camera cam;
    private static float minCamSize = 3f;
    private static float maxCamSize = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevMousePosition = Input.mousePosition;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        { 
            isPressed = false;
            return;
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        float scrollSpeed = 3f;
        if ((mouseScroll < 0) && (cam.orthographicSize-mouseScroll*scrollSpeed < maxCamSize))
        {
            cam.orthographicSize -= mouseScroll * scrollSpeed;
        }
        if ((mouseScroll > 0) && (cam.orthographicSize-mouseScroll*scrollSpeed > minCamSize))
        {
            cam.orthographicSize -= mouseScroll * scrollSpeed;

            float cameraHeiht = 2f * cam.orthographicSize;
            float cameraWidth = cameraHeiht * cam.aspect;

            Vector3 delta = new Vector3((Input.mousePosition.x - Screen.width/2) / Screen.width * cameraWidth,
                                        (Input.mousePosition.y - Screen.height/2) / Screen.height * cameraHeiht,
                                        0);
            Vector3 newPosition = transform.position + delta / ((cam.orthographicSize - minCamSize) / mouseScroll);
            if (isInViewField(newPosition))
            {
                transform.position = newPosition;
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isPressed = false;
        }

        if (isPressed)
        {
            float cameraHeiht = 2f * cam.orthographicSize;
            float cameraWidth = cameraHeiht * cam.aspect;

            Vector3 delta = Input.mousePosition - prevMousePosition;
            Vector3 newPosition = transform.position - new Vector3(delta.x * cameraWidth / Screen.width,
                                                                   delta.y * cameraHeiht / Screen.height,
                                                                   0);
            if (isInViewField(newPosition))
            {
                transform.position = newPosition;
            }
        }

        prevMousePosition = Input.mousePosition;

    }


    bool isInViewField(Vector3 v)
    {
        if ( v.x < (viewField.transform.position.x - viewField.transform.localScale.x / 2) ||
             v.x > (viewField.transform.position.x + viewField.transform.localScale.x / 2) ||
             v.y < (viewField.transform.position.y - viewField.transform.localScale.y / 2) ||
             v.y > (viewField.transform.position.y + viewField.transform.localScale.y / 2) )
        {
            return false;
        }
        return true;
    }

}
