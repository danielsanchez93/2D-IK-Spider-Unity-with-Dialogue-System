using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public LayerMask lm;
    public List<GameObject> Legs = new List<GameObject>();
    public List<GameObject> LegMovers = new List<GameObject>();
    public GameObject spiderBody;
    public Movement movement;
    public LayerMask layerMask;


    public float _leg0;
    public float _leg1;
    public float _leg2;
    public float _leg3;

    public float distance = 2f;
    public float increament = 0.2f;

    [Header("PingPong")]
    public float pingPongSpeed = 1f;
    public float delta = 1f;  //delta is the difference between min y to max y.
    [System.Serializable]
    public class Leg
    {
        public Transform furthestPoint;
        public Transform nearestPoint;
        public float offsetY;



    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        SetSpiderBodyPos();
        //BodyRotator();
        CanStep();
    }

    private void FixedUpdate()
    {
        RaycastGround();
        RayCastLegs();
    }

    private void RayCastLegs()
    {
        var startPosition = transform.position;
        int Leg0Layer = 1 << 9;
        int Leg1Layer = 1 << 10;
        int Leg2Layer = 1 << 11;
        int Leg3Layer = 1 << 12;
        var rayLeg0 = Physics2D.Raycast(startPosition, Legs[0].transform.position - startPosition, 10f, Leg2Layer);
        var rayLeg1 = Physics2D.Raycast(startPosition, Legs[1].transform.position - startPosition, 10f, Leg3Layer);
        var rayLeg2 = Physics2D.Raycast(startPosition, Legs[2].transform.position - startPosition, 10f, Leg0Layer);
        var rayLeg3 = Physics2D.Raycast(startPosition, Legs[3].transform.position - startPosition, 10f, Leg1Layer);

        Debug.DrawRay(startPosition, Legs[0].transform.position - startPosition, Color.blue);
        Debug.DrawRay(startPosition, Legs[1].transform.position - startPosition, Color.cyan);
        Debug.DrawRay(startPosition, Legs[2].transform.position - startPosition, Color.magenta);
        Debug.DrawRay(startPosition, Legs[3].transform.position - startPosition, Color.red);
        _leg0 = rayLeg0.distance;
        _leg1 = rayLeg1.distance;
        _leg2 = rayLeg2.distance;
        _leg3 = rayLeg3.distance;

        if (movement.xAxis > 0)
        {
            LegMovers[0].transform.position = transform.position - new Vector3(distance - (increament / 2), 0, 0); 
            LegMovers[1].transform.position = transform.position - new Vector3(distance - (increament / 2), 0, 0);
            LegMovers[2].transform.position = transform.position + new Vector3(distance + increament, 0, 0);
            LegMovers[3].transform.position = transform.position + new Vector3(distance + increament, 0, 0);
        }

        else if ( movement.xAxis < 0)
        {
            LegMovers[0].transform.position = transform.position - new Vector3(distance + increament, 0, 0); ;
            LegMovers[1].transform.position = transform.position - new Vector3(distance + increament, 0, 0);
            LegMovers[2].transform.position = transform.position + new Vector3(distance - (increament/2), 0, 0);
            LegMovers[3].transform.position = transform.position + new Vector3(distance - (increament / 2), 0, 0);
        }

        else
        {
            LegMovers[0].transform.position = transform.position - new Vector3(distance, 0, 0); ;
            LegMovers[1].transform.position = transform.position - new Vector3(distance, 0, 0);
            LegMovers[2].transform.position = transform.position + new Vector3(distance, 0, 0);
            LegMovers[3].transform.position = transform.position + new Vector3(distance, 0, 0);
        }


    }

    private void RaycastGround()
    {
        // int earthLayer = 1 << 8;
        var startPosition = transform.position;
        var groundRaycast = Physics2D.Raycast(startPosition, Vector3.down, 15f, lm);
        Debug.DrawRay(startPosition, Vector3.down * 15f, Color.red);
        transform.position = new Vector3(startPosition.x, groundRaycast.point.y + .1f, 0);
        ;
        // Debug.Log(groundRaycast.distance);
    }

    private void SetSpiderBodyPos()
    {
        Vector3[] positionArray =
        {
            Legs[0].transform.position, Legs[1].transform.position,
            Legs[2].transform.position, Legs[3].transform.position
        };
        var spiderPos = MeanVector(positionArray);
        // var offset = new Vector3(-.15f, 0, 0);
        // spiderBody.transform.position = spiderPos + offset;

        // only height
        spiderBody.transform.position = new Vector2(spiderBody.transform.position.x, spiderPos.y + 1.5f);
        PingPong();
    }

    private void PingPong()
    {

        float y = spiderBody.transform.position.y + Mathf.PingPong(pingPongSpeed * Time.time, delta);
        Vector3 pos = new Vector3(spiderBody.transform.position.x, y, spiderBody.transform.position.z);
        spiderBody.transform.position = pos;
    }

    private Vector3 MeanVector(Vector3[] vecs)
    {
        if (vecs.Length == 0)
            return Vector3.zero;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3 pos in vecs)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }

        return new Vector3(x / vecs.Length, y / vecs.Length, z / vecs.Length);
    }
    
    void BodyRotator()
    {
        var frontLegsMeanPos = GetFrontLegsMeanPos();
        var backLegsMeanPos = GetBackLegsMeanPos();
        var meanDiff = frontLegsMeanPos - backLegsMeanPos;
        var meanHeightDiff = meanDiff.y;
        //Debug.Log("mean height diff: " + meanHeightDiff);
        spiderBody.transform.rotation =  Quaternion.AngleAxis(meanHeightDiff*-20, Vector3.forward);
    }
    
    private Vector3 GetFrontLegsMeanPos()
    {
        Vector3[] frontLegs =
        {
            Legs[2].transform.position, Legs[3].transform.position
        };

        return MeanVector(frontLegs);
    }
    
    private Vector3 GetBackLegsMeanPos()
    {
        Vector3[] backLegs =
        {
            Legs[0].transform.position, Legs[1].transform.position
        };
        
        return MeanVector(backLegs);
    }

    // decide which leg can take the step
    private void CanStep()
    {
        if (LegMovers[1].GetComponent<LegMover>().isStepping &&
            LegMovers[3].GetComponent<LegMover>().isStepping)
        {
            LegMovers[0].GetComponent<LegMover>().canStep = false;
            LegMovers[2].GetComponent<LegMover>().canStep = false;
        }
        else
        {
            LegMovers[0].GetComponent<LegMover>().canStep = true;
            LegMovers[2].GetComponent<LegMover>().canStep = true;
        }

        if (LegMovers[0].GetComponent<LegMover>().isStepping &&
            LegMovers[2].GetComponent<LegMover>().isStepping)
        {
            LegMovers[1].GetComponent<LegMover>().canStep = false;
            LegMovers[3].GetComponent<LegMover>().canStep = false;
        }
        else
        {
            LegMovers[1].GetComponent<LegMover>().canStep = true;
            LegMovers[3].GetComponent<LegMover>().canStep = true;
        }
        
        // all reset?
        if (!LegMovers[0].GetComponent<LegMover>().canStep &&
            !LegMovers[1].GetComponent<LegMover>().canStep &&
            !LegMovers[2].GetComponent<LegMover>().canStep &&
            !LegMovers[3].GetComponent<LegMover>().canStep)
        {
            LegMovers[0].GetComponent<LegMover>().canStep = true;
            LegMovers[2].GetComponent<LegMover>().canStep = true;
        }
    }
}