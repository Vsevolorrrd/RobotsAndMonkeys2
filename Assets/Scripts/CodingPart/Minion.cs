using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask moveObjectLayer;
    [SerializeField] LayerMask monkeyLayer;
    [SerializeField] GameObject blood;

    [SerializeField] AudioClip roombaShort;
    [SerializeField] AudioClip roombaSuperShort;
    [SerializeField] AudioClip roombaSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip shot;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    private bool isMoving = false;
    private bool isTurning = false;
    private bool isAttacking = false;
    private int moveDistance = 1;
    private Vector3 initialPosition;
    Collectable item;
    Collectable currentItem;

    public IEnumerator Move(string direction)
    {
        if (isMoving)
        yield break;

        Vector2 checkDirection = direction == "backward" ? -transform.up : transform.up;
        Vector2 dir = direction == "backward" ? (transform.up * -moveDistance) : (transform.up * moveDistance);
        if (IsWallInDirection(checkDirection))
        {
            Debug.Log("There is a wall! Noooooo!!!");
            StartCoroutine(BumpIntoTheWall(checkDirection));
            yield break;
        }

        if (IsMoveObjInDirection(checkDirection))
        {
            var objectToMove = GetObject(checkDirection, moveObjectLayer);

            if (IsWallInDirection(checkDirection, objectToMove.transform))
            {
                Debug.Log("There is a wall! Noooooo!!!");
                StartCoroutine(BumpIntoTheWall(checkDirection));
                yield break;
            }

            Debug.Log("I move the object!");
            StartCoroutine(MoveObject(checkDirection, objectToMove));
            yield break;
        }

        Debug.Log("Player moves!");
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, moveDistance, monkeyLayer);
        if (hit.collider == null)
        {
            Debug.Log("There is no monkey! Noooooo!!!");
            AudioManager.Instance.PlaySound(roombaSound, 1f);
            yield break;
        }

        if (IsWallInDirection(checkDirection) || IsMoveObjInDirection(checkDirection))
        {
            Debug.Log("There is a wall! Noooooo!!!");
            StartCoroutine(BumpIntoTheWall(checkDirection));
            yield break;
        }

        Killable monkey = hit.collider.GetComponent<Killable>();
        monkey.AboutToDie();
        Debug.Log("MONKEY!");

        Vector2 dir = transform.up * moveDistance;
        yield return StartCoroutine(AttackAnim(dir, monkey));
    }

    public void Collect()
    {
        if (item != null)
        {
            item.OnCollect(transform, currentItem);
            currentItem = item;
            item = null;
        }
        Debug.Log("Player collects item!");
    }

    public IEnumerator Shoot()
    {
        if (!currentItem?.GetComponent<Pistol>())
        yield break;

        AudioManager.Instance.PlaySound(shot, 0.9f);
        Shaker.Instance.ShakeCamera(3f, 0.4f);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(transform.up);

        currentItem = null; // remove the pistol
        Debug.Log("Shooted");
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
        Shaker.Instance.ShakeCamera(1.4f, 0.4f);
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

    private IEnumerator AttackAnim(Vector2 direction, Killable monkey)
    {
        isAttacking = true;

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
        Vector2 startPos = transform.position;
        Vector2 buildUpPos = startPos - direction / 6f;
        Vector2 endPos = startPos + direction;
        float buildUpDuration = 0.25f;
        float duration = 0.25f;
        float time = 0;

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
    private IEnumerator MoveObject(Vector2 direction, GameObject objectToMove)
    {
        isMoving = true;

        AudioManager.Instance.PlaySound(roombaSuperShort, 0.6f);
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction * 1f;
        float duration = 0.5f;
        float time = 0f;

        Vector2 objStartPos = objectToMove.transform.position;
        Vector2 objEndPos = objStartPos + direction * 1f;
        float objDuration = 0.3f;
        float objTime = 0f;


        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time / 0.42f);
            time += Time.deltaTime;
            if (time > 0.18f)
            {
                //Shaker.Instance.ShakeCamera(0.8f, 0.4f);
                //AudioManager.Instance.PlaySound(hitSound, 0.4f);
                objectToMove.transform.position = Vector2.Lerp(objStartPos, objEndPos, objTime / objDuration);
                objTime += Time.deltaTime;
            }
            yield return null;
        }

        objectToMove.transform.position = objEndPos;
        transform.position = endPos;
        isMoving = false;
    }

    private bool IsWallInDirection(Vector2 dir, Transform targetPos = null)
    {
        Vector2 pos = transform.position;
        if (targetPos != null)
            pos = targetPos.position;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, moveDistance, wallLayer);
        return hit.collider != null;
    }
    private bool IsMoveObjInDirection(Vector2 dir, Transform targetPos = null)
    {
        Vector2 pos = transform.position;
        if (targetPos != null)
            pos = targetPos.position;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, moveDistance, moveObjectLayer);
        return hit.collider != null;
    }
    private GameObject GetObject(Vector2 dir, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, moveDistance, layerMask);
        return hit.collider.gameObject;
    }

    private void HandleReset()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        blood.SetActive(false);

        StopAllCoroutines();
        isMoving = false;
        isTurning = false;
        isAttacking = false;
        currentItem = null;
        item = null;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        item = other.GetComponent<Collectable>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Collectable>() == item)
        {
            item = null;
        }
    }
}