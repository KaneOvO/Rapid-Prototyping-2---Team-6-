using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(SphereCollider))]

public class SC_RigidbodyWalker : MonoBehaviour
{
    private Animator animator;
    public float speed = 15.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public Camera playerCamera;
    public float rotationSpeed = 2.0f;
    bool grounded = false;
    bool isJumping = false;
    Rigidbody r;
    Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    void Awake()
    {
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;
        r.useGravity = false;
        r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rotation.y = transform.eulerAngles.y;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(animator == null)
        {
            return;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S )) &&!isJumping)
        {
            animator.SetFloat("Speed", r.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }


    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isThirdPesronView)
        {
            Quaternion localRotation = Quaternion.Euler(0f, Input.GetAxis("Horizontal") * lookSpeed, 0f);
            transform.rotation = transform.rotation * localRotation;

            if (grounded)
            {
                //Direction of movement
                Vector3 forwardDir = Vector3.Cross(transform.up, -transform.right).normalized;

                Vector3 targetVelocity = forwardDir * Input.GetAxis("Vertical") * speed;

                Vector3 velocity = transform.InverseTransformDirection(r.velocity);
                velocity.y = 0;
                velocity = transform.TransformDirection(velocity);

                Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                velocityChange = transform.TransformDirection(velocityChange);

                r.AddForce(velocityChange, ForceMode.VelocityChange);
                //Debug.Log(velocityChange);

                if (Input.GetButton("Jump") && canJump)
                {
                    r.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
                    isJumping = true;
                    animator.SetBool("IsJump", true);
                }
            }

            grounded = false;
        }
    }

    void OnCollisionStay(Collision other)
    {
        grounded = true;

        if(other.gameObject.tag == "Planet" || other.gameObject.tag == "Water")
        {
            isJumping = false;
            animator.SetBool("IsJump", false);
        }

    }

}
