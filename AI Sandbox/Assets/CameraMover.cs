using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.x -= Input.GetAxis("Vertical");
        pos.z += Input.GetAxis("Horizontal");
        pos.y += Input.GetAxis("Mouse ScrollWheel") * 10.0f;

        if(pos.y > 60)
        {
            pos.y = 60;
        }
        else if (pos.y < 10)
        {
            pos.y = 10;
        }

        gameObject.transform.position = pos;
    }
}
