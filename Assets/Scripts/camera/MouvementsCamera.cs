using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementsCamera : MonoBehaviour
{

    public float sensibility = 0.35f;

    public int maxDezoom = 15;
    public int maxZoom = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Deplacement de la cam
        if (Input.GetMouseButton(2))
        {
            HideAndLockCursor();
            Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
            Vector3 pos = transform.position;
            if (NewPosition.x > 0.0f)
            {
                pos += transform.right*sensibility;
            }
            else if (NewPosition.x < 0.0f)
            {
                pos -= transform.right*sensibility;
            }
            if (NewPosition.z > 0.0f)
            {
                pos += transform.forward*sensibility;
            }
            if (NewPosition.z < 0.0f)
            {
                pos -= transform.forward*sensibility;
            }
            pos.y = transform.position.y;
            transform.position = pos;

        }

        if(Input.GetMouseButtonUp(2))
        {
            ShowAndUnlockCursor();
        }

        //zoom et dezoom
        if(Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Vector3 pos = transform.position;
            pos.y += 2 * -Input.GetAxis("Mouse ScrollWheel");

            if(pos.y > maxDezoom) { pos.y = maxDezoom; }else if(pos.y < maxZoom) { pos.y = maxZoom; }

            transform.position = pos; 
        }

    }

    void ShowAndUnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void mouveCamOnVector(Vector3 pos)
    {
        Vector3 posS = new Vector3(); posS.x = pos.x; posS.y = transform.position.y; posS.z = pos.z-3;
        transform.position = posS;
    }
}
