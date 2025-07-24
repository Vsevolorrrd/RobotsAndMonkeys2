using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask monkeyLayer;
    [SerializeField] GameObject blood;
    [SerializeField] AudioClip roombaShort;
    [SerializeField] AudioClip roombaSuperShort;
    [SerializeField] AudioClip roombaSound;
    [SerializeField] AudioClip hitSound;

    private bool isMoving = false;
    private bool isTurning = false;
    private bool isAttacking = false;
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

    public IEnumerator Attack()
    {
        if (isAttacking)
        yield break;

        Vector2 checkDirection = transform.up;

        if (IsWallInDirection(checkDirection))
        {
            Debug.Log("There is a wall! Noooooo!!!");
            StartCoroutine(BumpIntoTheWall(checkDirection));
            yield break;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, moveDistance, monkeyLayer);

        if (hit.collider == null)
        {
            Debug.Log("There is no monkey! Noooooo!!!");
            StartCoroutine(BumpIntoTheWall(checkDirection));
            yield break;
        }

        Killable monkey = hit.collider.GetComponent<Killable>();
        Debug.Log("MONKEY!");

        Vector2 dir = transform.up * moveDistance;
        yield return StartCoroutine(AttackAnim(dir, monkey));
    }

    public void Collect()
    {
        Debug.Log("Player collects item!");
    }

    private IEnumerator SmoothMoving(Vector2 direction)
    {
        isMoving = true;

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
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

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
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

        AudioManager.Instance.PlaySound(hitSound, 0.6f);
        Shaker.Instance.ShakeCamera(1.2f, 0.4f);
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

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
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

    private IEnumerator AttackAnim(Vector2 direction, Killable monkey)
    {
        isAttacking = true;

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
        Vector2 startPos = transform.position;
        Vector2 buildUpPos = startPos - direction/6f;
        Vector2 endPos = startPos + direction;
        float buildUpDuration = 0.25f;
        float duration = 0.25f;
        float time = 0f;

        while (time < buildUpDuration)
        {
            transform.position = Vector2.Lerp(startPos, buildUpPos, time / buildUpDuration);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(buildUpPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        monkey.Die();
        blood.SetActive(true);
        Shaker.Instance.ShakeCamera(2.5f, 0.8f);
        transform.position = endPos;
        isAttacking = false;
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
        blood.SetActive(false);
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