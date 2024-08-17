using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude = 1, float timeBetween = 0.05f){
        print("Shaking camera...");
        Vector3 originalPos = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration){
            float xOffset = Random.Range(-0.5f , 0.5f);
            float yOffset = Random.Range(-0.5f , 0.5f);
            print("Shaking offset (" + xOffset + "," + yOffset + ")");
            transform.localPosition += new Vector3(xOffset * magnitude, yOffset * magnitude, 0);
            
            elapsedTime += timeBetween;
            yield return new WaitForSeconds(timeBetween);
        }
        transform.localPosition = originalPos;
    }
}