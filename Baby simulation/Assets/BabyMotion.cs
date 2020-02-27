using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Hej

public class BabyMotion : MonoBehaviour
{
    //things that are constant
    protected const double Gravity = 9.82;
    protected const double h = 0.01;
    protected const double kfc = 15.0;


    //modifieable form gui
    public float babyLength = 0.75f; //längd på bebis, ny variabel av simon

    public double m = 9.0;
    public double k = 145.5;
    public double b = 8.0;
    public double bw = 0.8;
    public double babyRadius = 0.15;
    public double babyAngle = 0.0; // vinkel på bebis, ny variabel från simon
    public double angleVel = 0.0; // vinkelhastighet på bebis, ny variabel från simon 
    public double kickForce = 100.0;
    public double kickAngleR = 3 * Math.PI / 8;
    public double kickAngleL = 5 * Math.PI / 8;


    public Vector2 bandStartR = new Vector2(-0.3f, 2.0f);
    public Vector2 bandStartL = new Vector2(0.3f, 2.0f);
    public Vector2 bandEndR;
    public Vector2 bandEndL;
    public Vector2 newBandEndR;
    public Vector2 newBandEndL;
    //public Vector2 babyPosition;

    public Vector2 legPosR;
    public Vector2 legPosL;
    public Vector2 newLegPosR;
    public Vector2 newLegPosL;


    //Band Specs
    protected double bandAngleR;
    protected double bandAngleL;

    //Measure both bands
    protected double bandLengthR;
    protected double bandLengthL;
    protected double bandStretchR;
    protected double bandStretchL;
    protected double stretchDistR;
    protected double stretchDistL;


    //Specs for baby
    protected double InputRight = 0.0;
    protected double InputLeft = 0.0;

  
    protected double accX;
    protected double accY;
    protected double velX;
    protected double velY;


    //Controller
    protected bool isLclicked = false;
    protected bool isRclicked = false;



    void Start()
    {
        bandEndR = new Vector2((float)-babyRadius, 0.0f);
        bandEndL = new Vector2((float)babyRadius, 0.0f);
        legPosR = new Vector2((float)-babyRadius, -babyLength/2);
        legPosL = new Vector2((float)babyRadius, -babyLength/2);

    }


    // Update is called once per frame
    void FixedUpdate()
    {

            kickAngleR = 3 * Math.PI / 8 + babyAngle;
            kickAngleL = 5 * Math.PI / 8 + babyAngle;

        //babyPosition.x = transform.position.x;
        //babyPosition.y = transform.position.y;

        newBandEndR = rotatePoint(bandEndR, babyAngle); //removed transform.position / babyposition
        newBandEndL = rotatePoint(bandEndL, babyAngle);//removed transform.position / babyposition
        newBandEndR.x += transform.position.x;
        newBandEndR.y += transform.position.y;
        newBandEndL.x += transform.position.x;
        newBandEndL.y += transform.position.y;


        bandLengthR = Vector2.Distance(bandStartR, bandEndR);
        bandLengthL = Vector2.Distance(bandStartR, bandEndL);
        bandStretchR = Vector2.Distance(bandStartR, newBandEndR);
        bandStretchL = Vector2.Distance(bandStartL, newBandEndL);
        stretchDistL = getStretchDistance(bandLengthL, bandStretchL);
        stretchDistR = getStretchDistance(bandLengthR, bandStretchR);


        bandAngleR = Math.Atan2((bandStartR.y - newBandEndR.y), (bandStartR.x - newBandEndR.x));
        bandAngleL = Math.Atan2((bandStartL.y - newBandEndL.y), (bandStartL.x - newBandEndL.x));


        newLegPosR = rotatePoint(legPosR, babyAngle); //removed transform.position / babyposition
        newLegPosL = rotatePoint(legPosL, babyAngle);//removed transform.position / babyposition
        newLegPosR.x += transform.position.x;
        newLegPosR.y += transform.position.y;
        newLegPosL.x += transform.position.x;
        newLegPosL.y += transform.position.y;

        //Get the right input
        if (Input.GetKey("right"))
        {
          //Make the click last for one frame
          if(!isRclicked){
            InputRight = kickForce;
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
          InputLeft = kickForce;
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



      accX = xAcc(transform.position.x, transform.position.y, velX, InputRight, InputLeft);
      accY = yAcc(transform.position.x, transform.position.y, velY, InputRight, InputLeft);



      velX = velX + (h * accX);
      velY = velY + (h * accY);


      transform.Translate((float)velX*0.01f, (float)velY*0.01f, 0.0f);


        //simon lade till nedan

        //vinkel på bebis
        double alpha = angleAcceleration();
        //vinkelhastighet på bebis
        angleVel = angleVel + (h * alpha);
        //för att unity ska rotera bebis
        transform.Rotate(0.0f, 0.0f, (float)angleVel * 0.01f);


    }


//Calculate the accelaration in Y
    double yAcc(double x, double y,double velocity, double uR, double uL){



      double u = inputY(y, uR, uL);

      double a = (1/m) * (u - k * Math.Sin(bandAngleR) * stretchDistR - k * Math.Sin(bandAngleL) * stretchDistL - b*velocity - m * Gravity);

      return a;

    }

//Calculate the accelaration in X
    double xAcc(double x, double y, double velocity, double uR, double uL){


      double u = inputX(y, uR, uL);

      double a = (1/m) * (u - k * Math.Cos(bandAngleR) * stretchDistR - k * Math.Cos(bandAngleL) * stretchDistL - b*velocity);

      return a;

    }


    /*
     Kommande 3 funktioner har Simon lagt till
         */

    //hämta vridmomentet
    double getTorque(double force, Vector2 forcePosition, double forceAngle)
    {
        Vector2 centerOfMass = new Vector2(transform.position.x, transform.position.y);
        double r = Vector2.Distance(centerOfMass, forcePosition);
        double rAngle = Math.Atan2((forcePosition.y - centerOfMass.y), (forcePosition.x - centerOfMass.x));
        double theta = forceAngle - rAngle;
        double torque = force * r * Math.Sin(theta);

        return torque;
    }

    Vector2 rotatePoint(Vector2 vec, double angle)
    {
        Vector2 result;

        float[] R = new float[4]; // är osäker här
        R[0] = (float)Math.Cos(angle);
        R[1] = (float)-Math.Sin(angle);
        R[2] = (float)Math.Sin(angle);
        R[3] = (float)Math.Cos(angle);

        result.x = R[0] * vec.x - R[1] * vec.y + vec.x;
        result.y = R[2] * vec.x + R[3] * vec.y + vec.y;

        return result;
    }

    Vector2 calculateBandEnd(double angle, Vector2 bandEnd)
    {


        //float[] R = new float[4]; // är osäker här
        //R[0] = (float)Math.Cos(angle);
        //R[1] = (float)-Math.Sin(angle);
        //R[2] = (float)Math.Sin(angle);
        //R[3] = (float)Math.Cos(angle);

        Vector2 newEnd = rotatePoint(bandEnd, angle);

        //all
        //newEnd.x = R[0] * bandEnd.x - R[1] * bandEnd.y + transform.position.x;
        //newEnd.y = R[2] * bandEnd.x + R[3] * bandEnd.y + transform.position.y;


        return newEnd;
    }



    //räkna ut nuvarande vinkel på bebis
    double angleAcceleration()
    {

        float I = (1.0f / 4.0f) * (float)m * (float)Math.Pow(babyRadius, 2) + (1.0f / 12.0f) * (float)m * (float)Math.Pow(babyLength, 2);

        //double phiR = Math.Atan2((bandStartR.y - newBandEndR.y), (bandStartR.x - newBandEndR.x));

        //double phiL = Math.Atan2((bandStartL.y - newBandEndL.y), (bandStartL.x - newBandEndL.x));

        double bandForceR = k * stretchDistR;
        double bandForceL = k * stretchDistL;


        double torqueKickR = getTorque(kickForce, newLegPosR, kickAngleR);
        double torqueKickL = getTorque(kickForce, newLegPosL, kickAngleL);
        double torqueBandR = getTorque(bandForceR, newBandEndR, bandAngleR);
        double torqueBandL = getTorque(bandForceL, newBandEndL, bandAngleL);

        double alpha = -((torqueKickR + torqueKickL) * kfc + torqueBandR + torqueBandL + bw * angleVel) *kfc / I;

        return alpha;
    }

    // här slutar Simons funktioner

    double getStretchDistance(double bandLength, double bandStretch)
    {
        double stretchDist = bandLength - bandStretch;

        //Is he too high?
        if (stretchDist > 0)
        {
            stretchDistR = 0;//minE(compLimit*Math.Exp(stretchDistR - compRange), compLimit);
        }
        return stretchDist;
    }


//The function for y
    double inputY(double y, double inputR, double inputL){
      double u = kfc*inputR*Math.Sin(kickAngleR) *(1-Math.Exp(Math.Min(y, 0))) + kfc*inputL*Math.Sin(kickAngleL) *(1-Math.Exp(Math.Min(y, 0)));

      return u;
    }

//The function for x
      double inputX(double y, double inputR, double inputL){
        double u = kfc*inputR*Math.Cos(kickAngleR) *(1-Math.Exp(Math.Min(y, 0))) + kfc*inputL*Math.Cos(kickAngleL) *(1-Math.Exp(Math.Min(y, 0)));

        return u;
      }


}
