using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;

    private bool isMoving = false;
    private bool isTurning = false;
    private int moveDistance = 1;
    private Vector3 initialPosition;

    public IEnumerator Move(string direction)
    {
        if (isMoving)
        yield break;

        Vector2 checkDirection = direction == "forward" ? transform.up : -transform.up;
        if (IsWallInDirection(checkDirection))
        {
            Debug.Log("There is a wall! Noooooo!!!");
            StartCoroutine(BumpIntoTheWall(checkDirection));
            yield break;
        }

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

    private IEnumerator BumpIntoTheWall(Vector2 checkDirection)
    {
        isMoving = true;

        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + checkDirection * 0.28f;
        float duration = 0.25f;
        float time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Shaker.Instance.ShakeCamera(0.6f, 0.3f);
        time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(endPos, startPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
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

    private bool IsWallInDirection(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, moveDistance, wallLayer);
        return hit.collider != null;
    }

    private void HandleReset()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }

    private void Start()
    {
        initialPosition = transform.position;
        GameManager.Instance.OnGameReset += HandleReset;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
       GameManager.Instance.OnGameReset -= HandleReset;
    }
}