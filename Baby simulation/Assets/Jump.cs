using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

  protected float Position;

    // Start is called before the first frame update
    void Start()
    {
        Position = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
    /*  if (Input.GetKey("right"))
        {
            InputRight = 1.0 * kick_force;
        }
      else{
            InputRight = 0.0;
        }

      if (Input.GetKey("left"))
      {
          InputLeft = 1.0 * kick_force;
      }
      else{
          InputLeft = 0.0;
      }*/
    }
}
