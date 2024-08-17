using UnityEngine;
using System.Collections;

public class ColliderBlock : MonoBehaviour
{
    // The block's collider
    [SerializeField] Collider2D collider;
    // Speed to kill this collider block
    [SerializeField] float speedToKill;

    [SerializeField] CameraShake cameraShake;
    void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D rigidbody = other.transform.GetComponent<Rigidbody2D>();
        Debug.Log("I've been hit by" + other.gameObject.name);
        Debug.Log("Hit with velocity "+rigidbody.velocity.magnitude);
        string tag = other.gameObject.tag;
        if (tag == "Player"){
            if(rigidbody.velocity.magnitude > speedToKill){                
                Destroy(transform.gameObject);
            }
        }
    }

}