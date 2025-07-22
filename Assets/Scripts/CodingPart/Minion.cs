using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private bool isMoving = false;
    private bool isTurning = false;
    private int moveDistance = 1;

    public IEnumerator Move(string direction)
    {
        if (isMoving)
        yield break;

        Debug.Log("Player moves!");

        Vector2 dir = direction == "forward" ? (transform.up * moveDistance) : (transform.up * -moveDistance);
        yield return StartCoroutine(SmoothMoving(dir));
    }

    public IEnumerator Turn(string direction)
    {
        if (isTurning)
        yield break;

        Debug.Log("Player turns!");

        float angle = direction == "left" ? 90f : -90f;
        yield return StartCoroutine(SmoothRotate(angle));
    }

    public void Attack()
    {
        Debug.Log("Player attacks!");
    }

    public void Collect()
    {
        Debug.Log("Player collects item!");
    }

    private IEnumerator SmoothMoving(Vector2 direction)
    {
        isMoving = true;

        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction;
        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isTurning = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, angle);
        float duration = 0.3f;
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