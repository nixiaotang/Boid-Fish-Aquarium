using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoid : MonoBehaviour
{

    [SerializeField]
    private GameObject boid;

    [SerializeField]
    private Transform boidParent;


    void Start()
    {
        for(int i = 0; i < 5; i++) {
            
            //generate new koi fish
            GameObject newBoid = Instantiate(boid, 
                new Vector3(Random.Range(-42f, -10f), Random.Range(4f, 30f), Random.Range(-16f, 16f)), 
                Quaternion.Euler(Random.Range(-30f, 30), Random.Range(0f, 360f), 0f),
                boidParent
            );
            

            //start animation at random frame
            Animator anim = newBoid.GetComponentInChildren<Animator>();
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(state.fullPathHash, -1, Random.Range(-0, 1f));
        }


    }

}
