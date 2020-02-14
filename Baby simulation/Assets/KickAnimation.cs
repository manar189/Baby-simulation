using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KickAnimation : MonoBehaviour
{
  Image myImageComponent; // Image component attached to this gameobject

  public Sprite originalSprite;
  public Sprite pressedSprite;
  public string PressedKey = "right";


   void Start() //Lets start by getting a reference to our image component.
   {
   myImageComponent = GetComponent<Image>();
   }

  void Update()
    {
      if(Input.GetKey(KeyCode.D))
      {
        myImageComponent.sprite  = pressedSprite;
      }
      else if(Input.GetKeyUp(KeyCode.D))
      {
        myImageComponent.sprite = originalSprite;
      }
  }

}
