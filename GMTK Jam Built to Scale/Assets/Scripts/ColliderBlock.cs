using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;

public class ColliderBlock : MonoBehaviour
{
    // The block's collider
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] PolygonCollider2D brokenCollider;
    // Speed to kill this collider block
    [SerializeField] float breakForce;

    [SerializeField] CameraShake cameraShake;
    [SerializeField] SpriteResolver spriteResolver;
    private float lastCollisionTime = 0.0f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (Time.realtimeSinceStartup - lastCollisionTime < 0.05f) {
            return;
        }

        lastCollisionTime = Time.realtimeSinceStartup;


        string tag = other.gameObject.tag;

        if (tag == "Player")
        {
            Rigidbody2D rigidbody = other.transform.GetComponent<Rigidbody2D>();
            float force = rigidbody.velocity.magnitude * rigidbody.mass;
            Debug.Log("I've been hit by" + other.gameObject.name);
            Debug.Log("Hit with force " + force);
            if (force > breakForce)
            {
                string category = spriteResolver.GetCategory();
                string label = spriteResolver.GetLabel();

                if (label == "Whole")
                {
                    spriteResolver.SetCategoryAndLabel(category, "Cracked");
                }
                else if (label == "Cracked")
                {
                    spriteResolver.SetCategoryAndLabel(category, "Broken");
                    boxCollider.enabled = false;
                    brokenCollider.enabled = true;
                }
                else
                {
                    Destroy(transform.gameObject);
                }
            }
        }
    }

}