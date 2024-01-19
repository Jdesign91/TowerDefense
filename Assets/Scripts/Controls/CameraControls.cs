using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public int cameraLimitTop;
    public int cameraLimitBottom;
    public int cameraLimitLeft;
    public int cameraLimitRight;
    public float speed = 1f;
    private void Start()
    {
        cameraLimitTop = -8;
        cameraLimitBottom = -46;
        cameraLimitLeft = -25;
        cameraLimitRight = 25;
    }

    // Simple camera navigation.
    void Update()
    {
        Vector3 newPost = transform.localPosition;
        if (Input.mousePosition.x <= 0 && transform.localPosition.x > cameraLimitLeft)
        {
            newPost.x -= speed * Time.deltaTime;
        }
        else if (Input.mousePosition.x >= Screen.width && transform.localPosition.x < cameraLimitRight)
        {
            newPost.x += speed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= 0 && transform.localPosition.z > cameraLimitBottom)
        {
            newPost.z -= speed * Time.deltaTime; 
        }
        else if (Input.mousePosition.y >= Screen.height && transform.localPosition.z < cameraLimitTop)
        {
            newPost.z += speed * Time.deltaTime; 
        }

        transform.localPosition = newPost;
    }
}

