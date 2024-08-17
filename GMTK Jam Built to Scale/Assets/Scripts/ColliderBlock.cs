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
        Debug.Log("I've been hit by" + other.gameObject.name);
        Debug.Log("Hit with velocity "+other.relativeVelocity.magnitude);
        string tag = other.gameObject.tag;
        if (tag == "Player"){
            if(other.relativeVelocity.magnitude > speedToKill){                
                Destroy(transform.gameObject);
            }
        }
    }

}