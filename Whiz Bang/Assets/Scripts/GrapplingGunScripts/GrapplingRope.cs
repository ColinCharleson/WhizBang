using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public GrapplingGun grapplingGun;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    private float delta;

    public GameObject plungerHead;
    bool grappled = false;
    Quaternion curRot;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!grapplingGun.IsGrappling())
        {
            grappled = false;
            plungerHead.transform.position = grapplingGun.gunTip.position;
            plungerHead.transform.rotation = grapplingGun.gunTip.rotation;

            currentGrapplePosition = grapplingGun.gunTip.position;

            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }
        else
        { 
            if(grappled == false)
            {
                grappled = true;
                curRot = plungerHead.transform.rotation;
            }
            else
            {
                plungerHead.transform.rotation = curRot;
            }

            plungerHead.transform.position = currentGrapplePosition;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = grapplingGun.GetGrapplePoint();
        var gunTipPosition = grapplingGun.gunTip.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var right = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.right;
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta) +
                         right * waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
