using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    [SerializeField]
    private Transform boid;

    public List<Transform> nearFish;
    public List<Vector3> nearWall;
    public List<Vector3> nearOtherCollider;

    private float speed = 15f;
    private float turnSpeed = 5f;

    private float alignW = 2f;
    private float cohesW = 5f;
    private float avoidW = 6f;

    private float fishAvoid = 4f;
    private float wallAvoid = 25f;
    private float otherAvoid = 8f;

    void Start() {
        InvokeRepeating("UpdateNearObjects", 0f, 0.2f);
    }

    void Update() {

        CalculateMove();
    }

    void UpdateNearObjects() { 
        nearFish = new List<Transform>();
        nearWall = new List<Vector3>();
        nearOtherCollider = new List<Vector3>();

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 8f);

        foreach(Collider i in colliders) {
            if(i.transform != transform) {

                if(i.tag == "boid") {
                    nearFish.Add(i.transform);
                } else if(i.tag == "wall") {
                    nearWall.Add(i.ClosestPoint(transform.position));
                } else {
                    nearOtherCollider.Add(i.ClosestPoint(transform.position));
                    
                    //Debug.DrawLine(transform.position, i.ClosestPoint(transform.position), Color.magenta);

                }
            }
        }
        
    }


    void CalculateMove() {

        Vector3 a = CalculateAlignment() * alignW;
        Vector3 b = CalculateAvoidance() * avoidW;
        Vector3 c = CalculateCohesion() * cohesW;

        Debug.DrawLine(transform.position, transform.position + a, Color.green);
        Debug.DrawLine(transform.position, transform.position + b, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + c, Color.yellow);

        Vector3 move = (a + b + c) / (alignW + avoidW + cohesW);

        if(move.magnitude > 0.1f) {
            move.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * turnSpeed);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }



    Vector3 CalculateAlignment() {
        Vector3 total = Vector3.zero;
        foreach(Transform i in nearFish) { total += i.forward; }
        
        Vector3 output = (total / nearFish.Count);
        output.Normalize();

        return output;
    }

    Vector3 CalculateAvoidance() {

        Vector3 total = Vector3.zero;

        foreach(Vector3 i in nearOtherCollider) {

            Vector3 disp = transform.position - i;
            float dist = Vector3.Magnitude(disp);

            disp.Normalize();
            disp *= otherAvoid;
            disp /= dist;
            total += disp;

        }

        foreach(Vector3 i in nearWall) {

            Vector3 disp = transform.position - i;
            float dist = Vector3.Magnitude(disp);

            disp.Normalize();
            disp *= wallAvoid;
            disp /= dist;
            total += disp;

        }

        foreach(Transform i in nearFish) {
            Vector3 disp = transform.position - i.position;
            float dist = Vector3.Magnitude(disp);

            disp.Normalize();
            disp *= fishAvoid;
            disp /= dist;
            total += disp;
        }
        

        return total;

    }

    Vector3 CalculateCohesion() {

        Vector3 total = Vector3.zero;
        foreach(Transform i in nearFish) { 
            total += i.position - transform.position; 
            //Debug.DrawLine(transform.position, i.position, Color.yellow);
        }
        
        Vector3 output = total / nearFish.Count;
        output.Normalize();

        return output;

    }


}
