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


  protected double babyAngle = 0.0; // vinkel på bebis, ny variabel från simon
  protected double angleVel = 0.0; // vinkelhastighet på bebis, ny variabel från simon 
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
  protected double radius = 0.15;
  
  protected double AccX;
  protected double AccY;
  protected double VelX;
  protected double VelY;
  protected Vector2 velocity;


  protected float babyLength = 0.75f; //längd på bebis, ny variabel av simon

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
            InputRight = kick_force;
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
          InputLeft = kick_force;
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


      transform.Translate((float)VelX*0.01f, (float)VelY*0.01f, 0.0f);


        //simon lade till nedan

        //vinkel på bebis
        babyAngle = angleOfBaby(transform.position.x, transform.position.y, babyAngle, angleVel);
        //vinkelhastighet på bebis
        angleVel = angleVel + (h * babyAngle);
        //för att unity ska rotera bebis
        transform.Rotate(0.0f, 0.0f, (float)babyAngle * 0.01f);


    }


//Calculate the accelaration in Y
    double yAcc(double x, double y,double velocity, double uR, double uL){



      double u = inputY(y, uR, uL);

      double a = (1/m) * (u - k * Math.Sin(phiR) * stretchDistR - k * Math.Sin(phiL) * stretchDistL - b*velocity - m * Gravity);

      return a;

    }

//Calculate the accelaration in X
    double xAcc(double x, double y, double velocity, double uR, double uL){


      double u = inputX(y, uR, uL);

      double a = (1/m) * (u - k * Math.Cos(phiR) * stretchDistR - k * Math.Cos(phiL) * stretchDistL - b*velocity);

      return a;

    }


    /*
     Kommande 3 funktioner har Simon lagt till
         */

    // lade till innan jag såg att fille hade gjort en euclidean distance calculator
    double euclDist(Vector2 startpoint, Vector2 endpoint)
    {

        double dist = Math.Sqrt(Math.Pow((endpoint.x - startpoint.x), 2) + Math.Pow((endpoint.y - startpoint.y), 2));

        return dist;
    }


    //hämta vridmomentet
    double getTorque(double force, Vector2 forcePosition, double forceAngle, Vector2 centerOfMass)
    {

        double r = euclDist(centerOfMass, forcePosition);


        double r_angle = Math.Atan2((forcePosition.y - centerOfMass.y), (forcePosition.x - centerOfMass.x));


        double theta = forceAngle - r_angle;
        double torque = force * r * Math.Sin(theta);

        return torque;
    }


    //räkna ut nuvarande vinkel på bebis
    double angleOfBaby(double x, double y, double angle, double w)
    {

        float I = 1.0f / 4.0f * (float)m * (float)radius* (float)radius + 1.0f / 12.0f * (float)m * babyLength* babyLength;

        Vector2 legPosR; // pos of right leg
        legPosR.x = (float)x - (float)radius;
        legPosR.y = (float)y - 0.5f;

        Vector2 legPosL; // pos of left leg
        legPosL.x = (float)x + (float)radius;
        legPosL.y = (float)y - 0.5f;

        Vector2 CoM;
        CoM.x = (float)x;
        CoM.y = (float)y;

        Vector2 bandStartR; //where the elastic band is fixed
        bandStartR.x = -0.3f;
        bandStartR.y = 2.0f;

        Vector2 bandStartL; //where the elastic band is fixed
        bandStartL.x = 0.3f;
        bandStartL.y = 2.0f;

        Vector2 bandEndR; //where band stops without weight
        bandEndR.x = (float)-radius;
        bandEndR.y = 0.0f;

        Vector2 bandEndL; //where band stops without weight
        bandEndL.x = (float)radius;
        bandEndL.y = 0.0f;




        float[] R = new float[4]; // är osäker här
        R[0] = (float)Math.Cos(angle);
        R[1] = (float)-Math.Sin(angle);
        R[2] = (float)Math.Sin(angle);
        R[3] = (float)Math.Cos(angle);

        Vector2 newEndR; // är osäker här
        newEndR.x = R[0]*bandEndR.x - R[1]*bandEndR.y + (float)x;
        newEndR.y = R[2] * bandEndR.x + R[3] * bandEndR.y + (float)y;

        Vector2 newEndL; // är osäker här
        newEndL.x = R[0] * bandEndL.x - R[1] * bandEndL.y + (float)x;
        newEndL.y = R[2] * bandEndL.x + R[3] * bandEndL.y + (float)y;


        double bandLengthR = euclDist(bandStartR, bandEndR);
        double bandLengthL = euclDist(bandStartL, bandEndL);
        double bandStretchR = euclDist(bandStartR, newEndR);
        double bandStretchL = euclDist(bandStartL, newEndL);
        double stretchDistR = bandLengthR - bandStretchR;
        double stretchDistL = bandLengthL - bandStretchL;

        if (stretchDistR > 0)
            stretchDistR = 0;
        
        if (stretchDistL > 0)
            stretchDistL = 0;


        double phiR = Math.Atan2((bandStartR.y - newEndR.y), (bandStartR.x - newEndR.x));

        double phiL = Math.Atan2((bandStartL.y - newEndL.y), (bandStartL.x - newEndL.x));

        double bandForceR = k * stretchDistR;
        double bandForceL = k * stretchDistL;


        double torqueKickR = getTorque(kick_force, legPosR, angleR, CoM);
        double torqueKickL = getTorque(kick_force, legPosL, angleL, CoM);
        double torqueBandR = getTorque(bandForceR, newEndR, phiR, CoM);
        double torqueBandL = getTorque(bandForceL, newEndL, phiL, CoM);

        double alpha = -((torqueKickR + torqueKickL) * kfc + torqueBandR + torqueBandL + 0.1 * w)*20 / I;

        return alpha;
    }

    // här slutar Simons funktioner






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
