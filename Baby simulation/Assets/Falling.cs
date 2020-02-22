using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Hej

public class Falling : MonoBehaviour
{

  protected double Gravity = 9.82;
  public double m = 9.0;
  public double k = 145.5;
  public double b = 8.0;
  public double h = 0.01;
  //Spec for inital band
  //Right band
  protected double bandStartRX = -0.2;
  protected double bandStartRY = 2.0;
  protected double bandEndRX = -0.1;
  protected double bandEndRY = 0.0;
  //Left band
  protected double bandStartLX = 0.2;
  protected double bandStartLY = 2.0;
  protected double bandEndLX = 0.1;
  protected double bandEndLY = 0.0;
  //Measure both bands
  protected double bandLengthR;
  protected double bandLengthL;
  protected double bandStretchR;
  protected double bandStretchL;
  protected double stretchDistR;
  protected double stretchDistL;
  //Band Specs
  protected double compRange = 0.5;
  protected double compLimit = 0.1;
  protected double phiR;
  protected double phiL;

  //Specs for baby
  protected double InputRight = 0.0;
  protected double InputLeft = 0.0;
  protected double AccX;
  protected double AccY;
  protected double VelX;
  protected double VelY;
  protected Vector2 velocity;

  //Legs
  protected double angleR = 3*Math.PI/8;
  protected double angleL = 5*Math.PI/8;
  public double kick_force = 100.0;
  public double kfc = 15.0;

  //Controller
  protected bool isLclicked = false;
  protected bool isRclicked = false;



    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {



      bandLengthR = totDistance(bandStartRX, bandEndRX, bandStartRY, bandEndRY);
      bandLengthL = totDistance(bandStartLX, bandEndLX, bandStartLY, bandEndLY);
      bandStretchR = totDistance(bandStartRX, transform.position.x, bandStartRY, transform.position.y);
      bandStretchL = totDistance(bandStartLX, transform.position.x, bandStartLY, transform.position.y);


      stretchDistL = bandLengthL - bandStretchL;
      stretchDistR = bandLengthR - bandStretchR;

      //Is he too high?
      if(stretchDistR > 0){
        stretchDistR = minE(compLimit*Math.Exp(stretchDistR - compRange), compLimit);
      }

      if(stretchDistL > 0){
        stretchDistL = minE(compLimit*Math.Exp(stretchDistL - compRange), compLimit);
      }

      phiR = Math.Atan2((bandStartRY-(transform.position.y-bandEndRY)),(bandStartRX-(transform.position.x-bandEndRX)));
      phiL = Math.Atan2((bandStartLY-(transform.position.y-bandEndLY)),(bandStartLX-(transform.position.x-bandEndLX)));

      //Get the right input
      if (Input.GetKey("right"))
        {
          //Make the click last for one frame
          if(!isRclicked){
            InputRight = 1.0 * kick_force;
            isRclicked = true;
          }
          else{
              InputRight = 0.0;
          }

        }
      else{
            isRclicked = false;
            InputRight = 0.0;
        }

      if (Input.GetKey("left"))
      {
        //Make the click last for one frame
        if(!isLclicked){
          InputLeft = 1.0 * kick_force;
          isLclicked = true;
        }
        else{
            InputLeft = 0.0;
        }

      }
    else{
          isLclicked = false;
          InputLeft = 0.0;
      }



      AccX = xAcc(transform.position.x, transform.position.y, VelX, InputRight, InputLeft);
      AccY = yAcc(transform.position.x, transform.position.y, VelY, InputRight, InputLeft);



      VelX = VelX + (h * AccX);
      VelY = VelY + (h * AccY);

      //För att han ska studsa och ha friktion på marken
      //Krävs negativ acceleration på X
      /*if( transform.position.y <= -2){

        VelY = -1 * 0.5 * VelY; //Bäst att va på den säkra sidan

        VelX = 0.3 * VelX;

      }*/

      transform.Translate((float)VelX*0.01f, (float)VelY*0.01f, 0.0f);


    }


//Calculate the accelaration in Y
    double yAcc(double x, double y,double velocity, double uR, double uL){



      double u = inputY(y, uR, uL);

      double a = (1/m) * (u - k * Math.Sin(phiR) * stretchDistR - k * Math.Sin(phiL) * stretchDistL - b*velocity - m * Gravity);

      return a;

    }

//Calculate the accelaration in X
    double xAcc(double x, double y,double velocity, double uR, double uL){


      double u = inputX(y, uR, uL);

      double a = (1/m) * (u - k * Math.Cos(phiR) * stretchDistR - k * Math.Cos(phiL) * stretchDistL - b*velocity);

      return a;

    }


//Calculates the total distance between two points
    double totDistance(double x1, double x2, double y1, double y2){
      double dist = Math.Sqrt(Math.Pow((y2 - y1), 2) + Math.Pow((x2 - x1), 2));

      return dist;
    }
//Returns the smallest of two values
    double minE(double x, double y){
      if( x > y){
        return y;
      }
      else {
        return x;
      }
    }
//The function for y
    double inputY(double y, double inputR, double inputL){
      double u = kfc*inputR*Math.Sin(angleR)*(1-Math.Exp(minE(y, 0))) + kfc*inputL*Math.Sin(angleL)*(1-Math.Exp(minE(y, 0)));

      return u;
    }

//The function for x
      double inputX(double y, double inputR, double inputL){
        double u = kfc*inputR*Math.Cos(angleR)*(1-Math.Exp(minE(y, 0))) + kfc*inputL*Math.Cos(angleL)*(1-Math.Exp(minE(y, 0)));

        return u;
      }

}
