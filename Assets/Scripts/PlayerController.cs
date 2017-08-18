using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode throwBall;

    int oneTouch;

    private Rigidbody2D theRB;

    public Transform groundCheckPoint;
    public Transform wallCheckPoint;

    public float groundCheckRadius;
    public float wallCheckRadius;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public bool isGrounded;
    public bool touchingWall;

    private Animator anim;

    public GameObject snowBall;
    public Transform throwPoint;

	// Use this for initialization
	void Start () {
        oneTouch = 1;
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        touchingWall = Physics2D.OverlapCircle(wallCheckPoint.position, wallCheckRadius, whatIsWall);

        if (Input.GetKey(left))
        {
            theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
        }
        else if (Input.GetKey(right))
        {
            theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(0, theRB.velocity.y);
        }
        //add cases to switch direction again later
        //get key down looks for one press, not held
        if(Input.GetKeyDown(jump) && isGrounded)
        {
            Debug.Log("first jump " + oneTouch);
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            oneTouch = 1;

        }

        if (Input.GetKeyDown(jump) && oneTouch == 1 && touchingWall && !isGrounded)
        {
            //Wall Jump
            Debug.Log("inside " + oneTouch);
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            oneTouch = 0;
        }

        if(Input.GetKeyDown(throwBall) )
        {
            GameObject ballClone = (GameObject)Instantiate(snowBall, throwPoint.position, throwPoint.rotation);
            ballClone.transform.localScale = transform.localScale;
            anim.SetTrigger("Throw");
        }

        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(theRB.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        anim.SetFloat("Speed", Mathf.Abs(theRB.velocity.x));
        anim.SetBool("Grounded", isGrounded);
        //anim.SetTrigger()




    }
}
