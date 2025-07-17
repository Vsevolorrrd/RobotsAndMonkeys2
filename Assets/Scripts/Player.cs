using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    bool isMoving = false;
    bool isTurning = false;
    int moveDistance = 1;
    public void Move(string direction)
    {
        if (!isMoving)
        {
            Vector2 dir = direction == "forward" ? (transform.right * moveDistance) : (transform.right * -moveDistance);
            StartCoroutine(SmoothMoving(dir));
        }
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

    private IEnumerator SmoothMoving(Vector2 direction)
    {
    
        isMoving = true;

        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction;
        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(.1f); ;

        transform.position = endPos;
        isMoving = false;
    }
    private IEnumerator SmoothRotate(float angle)
    {
        isTurning = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, angle);
        float duration = 0.1f;
        float time = 0f;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(.1f); ;

        transform.rotation = endRot;
        isTurning = false;
    }
}
