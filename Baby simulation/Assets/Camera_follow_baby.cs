using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_follow_baby : MonoBehaviour
{

  Vector3 desiredCameraStart = new Vector3(0.8f,0.7f,0f);
    // Start is called before the first frame update
    private Func<Vector3> GetcameraFollowPositionFunc;
    public void Setup(Func<Vector3> GetcameraFollowPositionFunc){
      this.GetcameraFollowPositionFunc = GetcameraFollowPositionFunc;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = GetcameraFollowPositionFunc()*0.1f + desiredCameraStart;
        cameraFollowPosition.z = transform.position.z;
        transform.position = cameraFollowPosition;
    }
}
