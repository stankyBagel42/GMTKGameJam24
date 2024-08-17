using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float baseMoveForce = 4f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float sizeChangeSpeed = 0.1f;
    [SerializeField] private Animator animator;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite growingSprite;
    [SerializeField] private Sprite shrinkingSprite;




    static readonly int IsRunning = Animator.StringToHash("IsRunning");
    static readonly int IsJumping = Animator.StringToHash("IsJumping");
    static readonly int IsGrounded = Animator.StringToHash("IsGrounded");


    // -1 for shrinking, 0 for no change, 1 for growing
    private float _growDirection = 0;
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

    // Changes the object's scale, and re-calculates the speed based on the difference.
    private void ChangeScale(float changeSpeed){

        var spriteRenderer=transform.GetComponentInChildren<SpriteRenderer>();
        // change the speed based on the square of the changing scale (since mass would be quadratic when compared to scale changing)
        var velocityChange = changeSpeed * changeSpeed;
        _rigidbody.velocity /= velocityChange;

        if (changeSpeed == 1){
            spriteRenderer.sprite = normalSprite;

        }else{
            transform.localScale *= changeSpeed;
            if(changeSpeed > 1){
                spriteRenderer.sprite = growingSprite;
            }else{
                spriteRenderer.sprite = shrinkingSprite;
            }
        }
    }

    private void Update()
    {
        float changeSpeed = 1 + (_growDirection * sizeChangeSpeed * Time.deltaTime);
        ChangeScale(changeSpeed);
        // _rigidbody.velocity = _currentVelocity;
        _currentVelocity = Vector2.ClampMagnitude(_currentVelocity, maxSpeed);

        // the y scale is always positive, if the scale is larger the force applied should be larger, at a minimum it is 1x
        _currentVelocity *= Mathf.Min(1, transform.localScale.y);
        _rigidbody.AddForce(_currentVelocity);
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
        _currentVelocity.x = ctx.ReadValue<float>() * baseMoveForce;
        if (animator)
        {
            Vector3 localScale = transform.localScale;
            if (_currentVelocity.x  > 0)
            {
                localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            } else if (_currentVelocity.x  < 0)
            {
                localScale = new Vector3(-1*Mathf.Abs(localScale.x), localScale.y, localScale.z);
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
        if (Mathf.Abs(_currentVelocity.y) < 0.01f)
            {
                animator.SetBool(IsJumping, true);
                animator.SetBool(IsGrounded, false);
            }
            else
            {
                animator.SetBool(IsJumping, false);
            }
    }

    public void OnSizeChange(InputAction.CallbackContext ctx){
        float direction = ctx.ReadValue<float>();

        if(direction != 0){
            direction /= Mathf.Abs(direction);
        }

        _growDirection = direction;
    }
    
 
}