using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isTurning = false;
    public void Move(string direction)
    {
        if (direction == "forward") transform.position += Vector3.right;
        else if (direction == "backward") transform.position += Vector3.back;
    }

    public void Turn(string direction)
    {
        if (!isTurning)
        {
            float angle = direction == "left" ? 90f : -90f;
            StartCoroutine(SmoothRotate(angle));
        }
    }

    public void Attack()
    {
        Debug.Log("Player attacks!");
    }

    public void Collect()
    {
        Debug.Log("Player collects item!");
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isTurning = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, angle);
        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        isTurning = false;
    }
}
