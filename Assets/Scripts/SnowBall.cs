using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour {

    public float ballSpeed;
    private Rigidbody2D theRB;

    public GameObject snowBallEffect;

	// Use this for initialization
	void Start () {
        theRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("tester");
        theRB.velocity = new Vector2(ballSpeed * transform.localScale.x, 0);
		
	}


    //problems here: it still exists, it's not colliding with anything.
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("got here");

        Instantiate(snowBallEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
