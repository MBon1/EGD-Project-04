using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    float defaultFieldOfView;
    Vector3 defaultPosition;

    [SerializeField] RootManager rootManager;


    /*[SerializeField] float minZoom = 1.0f;
    [SerializeField] float maxZoom = 179.0f;

    [SerializeField] float dragSpeed = 2;
    Vector3 dragOrigin;*/

    Vector2 lastHoldtimes = Vector2.zero;
    [SerializeField] float inputDelay = 0.075f;

    private void Awake()
    {
        mainCamera = this.gameObject.GetComponent<Camera>();
        defaultFieldOfView = mainCamera.fieldOfView;
        defaultPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll Wheel to zoom in & out 
        // Zoom in may focus on mouse cursor position
        // Right click & drag to change camera position
        ReadInput();
    }

    private void ReadInput()
    {
        // Reset Values
        /*if (Input.GetKey(KeyCode.Escape))
        {
            mainCamera.fieldOfView = defaultFieldOfView;
            this.gameObject.transform.position = defaultPosition;
            return;
        }*/

        /*// Zoom In/Out
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0)
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll, minZoom, maxZoom);
            return;
        }

        // Drag Camera
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1) || (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            return;
        }

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, this.gameObject.transform.position.z);*/


        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            lastHoldtimes.x = 0;
        }

        if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            lastHoldtimes.y = 0;
        }


        // Bring to lowest root
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            this.gameObject.transform.position = new Vector3(rootManager.lowestRootPosition.x, rootManager.lowestRootPosition.y - 1, this.transform.position.z);
        }

        // Bring to origin root
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            this.gameObject.transform.position = defaultPosition;
        }


        Vector3 move = mainCamera.transform.position;
        if (Time.time - lastHoldtimes.x >= inputDelay)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                move.x -= 1;
                lastHoldtimes.x = Time.time;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                move.x += 1;
                lastHoldtimes.x = Time.time;
            }
        }

        if (Time.time - lastHoldtimes.y >= inputDelay)
        {
            if (Input.GetKey(KeyCode.DownArrow) ||
                Input.mouseScrollDelta.y < 0)
            {
                move.y -= 1;
                lastHoldtimes.y = Time.time;
            }

            if (Input.GetKey(KeyCode.UpArrow) ||
                Input.mouseScrollDelta.y > 0)
            {
                move.y += 1;
                lastHoldtimes.y = Time.time;
            }

            if (move.y > 0)
            {
                move.y = 0;
            }
        }

        this.gameObject.transform.position = move;
    }
}
