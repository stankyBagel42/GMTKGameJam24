using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private Animator animator;
    static readonly int IsRunning = Animator.StringToHash("IsRunning");
    static readonly int IsJumping = Animator.StringToHash("IsJumping");
    static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

    private Vector2 _currentVelocity;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private Collider2D[] _colliders;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliders = new Collider2D[1];
        _colliders[0] = _collider;
        _currentVelocity = _rigidbody.velocity;
    }

    private void Update()
    {
        _rigidbody.velocity = _currentVelocity;
        var contacts = _rigidbody.GetContacts(_colliders);
        if (contacts > 0){
                animator.SetBool(IsGrounded, true);
        }else{
            animator.SetBool(IsGrounded, false);
        }
    }
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // set new horizontal velocity
        _currentVelocity.x = ctx.ReadValue<float>() * speed;
        if (animator)
        {
            Vector3 localScale = transform.localScale;
            if (_currentVelocity.x  > 0)
            {
                localScale = new Vector3(1, localScale.y, localScale.z);
            } else if (_currentVelocity.x  < 0)
            {
                localScale = new Vector3(-1, localScale.y, localScale.z);
            }
        
            transform.localScale = localScale;
            if (Mathf.Abs(_currentVelocity.x) > 0.01f)
            {
                animator.SetBool(IsRunning, true);
            }
            else
            {
                animator.SetBool(IsRunning, false);
            }
        }    
    }

    public void OnJump(InputAction.CallbackContext ctx){
        _currentVelocity.y = ctx.ReadValue<float>() * jumpSpeed;
        if (Mathf.Abs(_currentVelocity.y) > 0.01f)
            {
                animator.SetBool(IsJumping, true);
                animator.SetBool(IsGrounded, false);
            }
            else
            {
                animator.SetBool(IsJumping, false);
            }
    }
 
}