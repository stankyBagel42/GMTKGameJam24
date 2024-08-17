using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    // Weapon cooldown, in seconds
    [SerializeField] public float cooldown = 0.5f;

    // Weapon mode, shrink if true else grow
    [SerializeField] public bool shrinkMode = false;

    // Amount the scale changes per second when firing at an object as a percentage of the object's scale.
    [SerializeField] public float scaleChangeSpeed = 0.1f;

    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public Transform weaponOrigin;

    private Transform curTarget = null;

    private float lastFire = -1f;

    public void Shoot(InputAction.CallbackContext ctx)
    {
        var shootDirection = ctx.ReadValue<Vector2>();
        float curTime = Time.time;
        float timeSinceFire = curTime - lastFire;
        Vector2 weaponStart = new Vector2(weaponOrigin.position.x, weaponOrigin.position.y);
        // if the gun is being shot in a direction, not the key being released
        if (shootDirection.magnitude > 0.0f)
        {
            // Vector2 weaponOrigin = transform.position + (transform.localScale / 2);
            if (timeSinceFire > cooldown)
            {
                weaponOrigin.parent.GetComponent<BoxCollider2D>().enabled = false;
                var hit_info = Physics2D.Raycast(weaponOrigin.position, shootDirection);
                weaponOrigin.parent.GetComponent<BoxCollider2D>().enabled = true;
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, weaponOrigin.position);

                if (hit_info)
                {
                    Debug.Log("hit: " + hit_info.transform.name);
                    if (hit_info.transform.tag == "SizeChange")
                    {
                        curTarget = hit_info.transform;
                    }

                    lineRenderer.SetPosition(1, hit_info.point);
                }
                else
                {
                    lineRenderer.SetPosition(1, weaponOrigin.position + 100 * Vector3.right);
                }
                Debug.Log(String.Format("Shot in direction: ({0}, {1})", shootDirection.x, shootDirection.y));

            }

        }
        else
        {
            lineRenderer.enabled = false;
            curTarget = null;
            // start the cooldown
            lastFire = curTime;
        }
    }

    public void ChangeMode(InputAction.CallbackContext ctx)
    {
        shrinkMode = !shrinkMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (curTarget != null)
        {
            if (shrinkMode)
            {
                curTarget.localScale *= 1 - (scaleChangeSpeed * Time.deltaTime);
            }
            else
            {
                curTarget.localScale *= 1 + (scaleChangeSpeed * Time.deltaTime);
            }
        }
    }
}
