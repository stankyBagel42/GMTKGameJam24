using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 10f;
    public Vector3 offset;
    public Transform minBound;
    public Transform maxBound;
    
    private Vector3 specificVector;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();    
    }

    // Update is called once per frame
    void Update()
    {
        float minX = minBound.transform.position.x;
        float minY = minBound.transform.position.y;
        float maxX = maxBound.transform.position.x;
        float maxY = maxBound.transform.position.y;

        specificVector = new Vector3(
            Mathf.Clamp(target.transform.position.x + offset.x, minX, maxX),
            Mathf.Clamp(target.transform.position.y + offset.y, minY, maxY),
            transform.position.z + offset.z
        );
        transform.position = Vector3.Lerp(transform.position, specificVector, smoothSpeed * Time.deltaTime);
    }
}
