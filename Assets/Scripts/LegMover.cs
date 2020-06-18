using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMover : MonoBehaviour
{
    public GameObject Leg;
    public LayerMask layerMask;
    public LayerMask groundMask;
    public bool canStep = true;
    public bool isStepping = false;
    public bool newTarget = false;
    public Vector3 targetLocation;

    void Start()
    {
    }

    void LateUpdate()
    {
        //int groundMask = 1 << 8;
        var startPosition = transform.position;
        var legPosition = Leg.transform.position;
        var rayLeg = Physics2D.Raycast(startPosition, legPosition - startPosition, 10f, layerMask);
        var rayHeight = Physics2D.Raycast(startPosition+ new Vector3(0,3f,0), Vector2.down, 10f, groundMask);
        transform.position = new Vector2(startPosition.x, rayHeight.point.y);
        Debug.DrawRay(startPosition, legPosition - startPosition, Color.white);
        Debug.DrawRay(startPosition + new Vector3(0, 3f, 0), Vector2.down *3f, Color.red);
        if (rayLeg.distance >= 1.5f)
        {
            StartCoroutine(nameof(MoveLeg));
        }
    }

    private IEnumerator MoveLeg()
    {
        if (canStep == false)
        {
            yield break;
        }
        
        if (newTarget == false)
        {
            targetLocation = transform.position;
            newTarget = true;
        }

        isStepping = true;
        Leg.transform.position = Vector3.Lerp(Leg.transform.position, targetLocation, Time.deltaTime * 5);

        if (RoughApproximate(Leg.transform.position.x, targetLocation.x, 2.005f) &&
            RoughApproximate(Leg.transform.position.y, targetLocation.y, 2.005f))
        {
            newTarget = false;
            StopCoroutine(nameof(MoveLeg));
            canStep = false;
            isStepping = false;
        }
        else
        {
            yield return null;

            StartCoroutine(nameof(MoveLeg));
        }
    }

    private static bool RoughApproximate(float val1, float val2, float threshold)
    {
        if (threshold > 0f)
        {
            return Mathf.Abs(val1 - val2) <= 0.029; // optimised threshold value for our spider
        }

        return Mathf.Approximately(val1, val2);
    }
}