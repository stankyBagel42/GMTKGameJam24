using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Movement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float baseMoveForce = 4f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float sizeChangeSpeed = 0.1f;

    [SerializeField] private float coyoteTimeDist = 0.5f;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite growingSprite;
    [SerializeField] private Sprite shrinkingSprite;

    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 5f;

    [SerializeField]
    private bool isGrounded = false;
    [SerializeField] private Transform spawnPoint;

    static readonly int IsGrounded = Animator.StringToHash("IsGrounded");


    // -1 for shrinking, 0 for no change, 1 for growing
    private float _growDirection = 0;
    private Vector2 _currentForce;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private Collider2D[] _colliders;

    private ContactPoint2D[] contacts;
    private Vector2 _lastGroundPoint;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliders = new Collider2D[1];
        // 4 contact points for the feet contacts
        contacts = new ContactPoint2D[4];
        _colliders[0] = _collider;
        _currentForce = new Vector2(0,0);
    }

    // Changes the object's scale, and re-calculates the speed based on the difference.
    private void ChangeScale(float changeSpeed)
    {

        var spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        // change the speed based on the square of the changing scale (since mass would be quadratic when compared to scale changing)
        if (changeSpeed == 1)
        {
            spriteRenderer.sprite = normalSprite;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale *= changeSpeed;

            // clamp y and z scales as they are always positive
            
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);


            // only update velocity if the scale is actually changing
            if ((transform.localScale - newScale).magnitude > 0f){                
                var velocityChange = changeSpeed * changeSpeed;
                _rigidbody.velocity /= velocityChange;

            }
            transform.localScale = newScale;
            if (changeSpeed > 1)
            {
                spriteRenderer.sprite = growingSprite;
            }
            else
            {
                spriteRenderer.sprite = shrinkingSprite;
            }
        }
    }

    private void Update()
    {
        float changeSpeed = 1 + (_growDirection * sizeChangeSpeed * Time.deltaTime);
        ChangeScale(changeSpeed); 

        // the y scale is always positive, if the scale is larger the force applied should be larger, at a minimum it is 1x
        _rigidbody.AddForce(_currentForce);


        _currentForce.y = 0;      
        // get the contacts
        int num_contact = _rigidbody.GetContacts(contacts);

        bool contactBelow = false;
        for(int i=0; i< num_contact; i++){
            _lastGroundPoint = contacts[i].point;
            // contact below
            if (_lastGroundPoint.y < transform.position.y){
                contactBelow=true;
            }
        }
        // add coyote time
        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        float distToLastGround =  (curPos- _lastGroundPoint).magnitude;
        if (contactBelow || distToLastGround < coyoteTimeDist)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        // set new horizontal velocity
        _currentForce.x = ctx.ReadValue<float>() * baseMoveForce * transform.localScale.magnitude;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        var jumpVal = ctx.ReadValue<float>();
        print("Got: " + jumpVal);
        if (jumpVal > 0f && isGrounded)
        {
            float scaleMagnitude = Mathf.Pow(transform.localScale.x, 1.5f);
            float jumpScale = Mathf.Min(scaleMagnitude, 1.5f);
            _currentForce.y =  jumpVal * jumpSpeed * jumpScale;
            
            print("Jumping! Cur y vel: " + _currentForce.y + " | isGrounded: " + isGrounded);
            isGrounded = false;
        }
    }

    public void OnSizeChange(InputAction.CallbackContext ctx)
    {
        float direction = ctx.ReadValue<float>();

        if (direction != 0)
        {
            direction /= Mathf.Abs(direction);
        }

        _growDirection = direction;
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Respawn") {
            //TODO things on death
            transform.localScale = Vector3.one;
            _currentForce = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            gameObject.transform.position = spawnPoint.position;
        }
   }

}