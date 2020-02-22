using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect_baby_with_camera : MonoBehaviour
{

    public Camera_follow_baby cameraFollow;
    public Transform playerTransfrom;
    // Start is called before the first frame update
    void Start()
    {
        cameraFollow.Setup(() => playerTransfrom.position);
    }

    // Update is called once per frame

}
