using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 10f;  // technically acceleration
    public float maxSpeed = 10f;
    //public float runningSpeed = 20f;
    public float jumpImpulse = 50f;
    public float jumpBoost = 3f;
    public bool isGrounded;
    public bool isHitting;

    public delegate void BrickHit();
    public static event BrickHit OnBrickHit;
    
    public delegate void QuestionHit();
    public static event QuestionHit OnQuestionHit;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = -Input.GetAxis("Horizontal");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity += Vector3.right * horizontalMovement * Time.deltaTime * speed;
        
        Collider col = GetComponent<Collider>();
        float halfHeight = col.bounds.extents.y + 0.03f; // add so it goes a little past the edge of the capsule
        
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + Vector3.down * halfHeight;
        
        isGrounded = Physics.Raycast(startPoint, Vector3.down, halfHeight);
        Color lineColor = (isGrounded) ? Color.red : Color.blue;
        Debug.DrawLine(startPoint, endPoint, lineColor, 0f, false);

        
        // jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        }
        else if (!isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.up * jumpBoost, ForceMode.Force);
            }
        }

        // running
        float runSpeed = maxSpeed * 1.5f;
        float normSpeed = maxSpeed;
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("running");
            maxSpeed = runSpeed;
        }
        else
        {
            maxSpeed = normSpeed;
        }

        // cap velocity
        if (Math.Abs(rb.velocity.x) > maxSpeed)
        {
            Vector3 newVel = rb.velocity;
            newVel.x = Math.Clamp(newVel.x, -maxSpeed, maxSpeed);
            rb.velocity = newVel;
        }

        if (isGrounded && Math.Abs(horizontalMovement) < 0.5f)
        {
            Vector3 newVel = rb.velocity;
            newVel.x *= 1 - Time.deltaTime;
            rb.velocity = newVel;
        }

        // so mario faces the right way
        float yaw = (rb.velocity.x > 0) ? 90 : -90;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        float speedA = rb.velocity.x;
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("Speed", Math.Abs(speedA));
        anim.SetBool("In Air", !isGrounded);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("Level Cleared!");
        }
        else if (other.tag == "water")
        {
            Debug.Log("You Died");
            transform.position = new Vector3(-9.3f, 2.0f, 0.0f);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        Collider col = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float hatHeight = col.bounds.extents.y * 1.93f; // slightly above top of collider (needed so ray isn't infinite)
        Vector3 collisionPoint = startPoint + Vector3.up * hatHeight;

        //RaycastHit hitInfo;
        isHitting = Physics.Raycast(startPoint, Vector3.up, out RaycastHit hitInfo, hatHeight);
        Color lineColor2 = (isHitting) ? Color.red : Color.blue;
        Debug.DrawLine(startPoint, collisionPoint, lineColor2, 0f, false);
        
        // hitting blocks
        if (isHitting)
        {
            // Debug.Log("is hitting block");
            GameObject hitObject = hitInfo.collider.gameObject;
            AudioSource src = hitObject.GetComponent<AudioSource>();
            src.Play();
            // Debug.Log("Hit object: " + hitObject.tag);
            if (hitObject.tag == "brick")
            {
                OnBrickHit.Invoke();
                Destroy(hitObject);
            }

            if (hitObject.tag == "question")
            {
                OnQuestionHit.Invoke();
            }
        }
    }
}
