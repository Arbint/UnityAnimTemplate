using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Transform groundCheckTransform;
    public bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask);
    }
}
