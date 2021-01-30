﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Refs")]
    public new Rigidbody rigidbody;
    public Animator animator;
    public PlayerView view;

    [Header("Movement")]
    public Vector3 horizontalAxis, verticalAxis;

    public float maxSpeed = 5;
    public float accel = 1;
    [Range(0,1)]
    public float floorDrag = 0.05f;
    [Range(0,1)]
    public float airDrag = 0.05f;

    private bool canMove = true;

    [Header("Floor Detection")]
    public Transform floorDetectorTransform;
    public float detectionRadius = 0.1f;
    public LayerMask floorLayer;

    [Header("Ledge Detection")]
    public Transform ledgeDetectorTransform;
    public Transform climbEndTransform;
    public float detectionLength = 1;
    public LayerMask ledgeLayer;

    [Header("Jump")]
    public float jumpForce = 10f;
    private bool isJumping;

    private Vector3 lastDir;
    
    [Header("Debug")]
    public bool isOnFloor;
    public bool ledgeDetected;

    private Collider[] results;
    private RaycastHit[] hitResult;

    private Vector3 ledgeStartPosition = Vector3.zero;
    private Vector3 ledgeEndPosition = Vector3.zero;

    private void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        if (animator == null)
           animator = GetComponent<Animator>();

        results = new Collider[1];
        hitResult = new RaycastHit[1];
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        if (canMove && (hor != 0 || vert != 0))
        {
            if (rigidbody.velocity.magnitude < maxSpeed)
            {
                rigidbody.velocity += horizontalAxis.normalized * hor * accel;
                rigidbody.velocity += verticalAxis.normalized * vert * accel;
            }
            lastDir = (horizontalAxis.normalized * hor + verticalAxis.normalized * vert).normalized;
            transform.forward = lastDir;

            animator.SetFloat("speed", rigidbody.velocity.magnitude);
        }
        else
            animator.SetFloat("speed", 0f);

        animator.SetFloat("velocityY", rigidbody.velocity.y);

        CheckFloor();
        CheckLedge();
        ApplyDrag();
    }

    public void OnJumpStart()
    {
        rigidbody.velocity += Vector3.up * jumpForce;
        isJumping = true;

        animator.ResetTrigger("jump");
        Debug.Log("Jump Start");
    }

    public void OnJumpEnd()
    {
        canMove = true;
        isJumping = false;


        animator.ResetTrigger("jump");
        Debug.Log("Jump End");
    }

    public void OnClimbEnd()
    {
        transform.position = climbEndTransform.position;
        rigidbody.isKinematic = false;
        canMove = true;
        Debug.Log("Climb End");
    }

    private void CheckLedge()
    {
        ledgeDetected = Physics.OverlapSphereNonAlloc(ledgeDetectorTransform.position,  detectionLength, results, ledgeLayer) > 0;
        if(ledgeDetected && isJumping)
        {
            StartClimb();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
            canMove = false;
        }

    }

    private void StartClimb()
    {
        rigidbody.isKinematic = true;
        animator.SetTrigger("climb");

        isJumping = false;
    }

    private void CheckFloor()
    {
        isOnFloor = Physics.OverlapSphereNonAlloc(floorDetectorTransform.position, detectionRadius, results, floorLayer) > 0;
        animator.SetBool("isOnFloor", isOnFloor);
    }

    private void ApplyDrag()
    {
        var prevVel = rigidbody.velocity;
        prevVel *= 1 - floorDrag;
        prevVel.y = rigidbody.velocity.y;
        rigidbody.velocity = prevVel;
    }

    private void OnDrawGizmos()
    {
        if (floorDetectorTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(floorDetectorTransform.position, detectionRadius);
            if (isOnFloor)
                Gizmos.DrawWireCube(floorDetectorTransform.position, new Vector3(1, 0.2f, 1));
        }

        if(ledgeDetectorTransform != null)
        {
            Gizmos.color = ledgeDetected ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(ledgeDetectorTransform.position, detectionLength);
        }
    }
}
