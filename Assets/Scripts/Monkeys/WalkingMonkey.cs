using UnityEngine;
using System.Collections;

public class WalkingMonkey : Killable
{
    [SerializeField] GameObject sprites;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float waitTime = 1f;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private bool move = false;

    private void Update()
    {
        if (waypoints.Length == 0) return;
        if (isWaiting || !move) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }
    private IEnumerator SmoothRotate(float angle)
    {
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
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        StartCoroutine(SmoothRotate(angle));

        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

    }
    private void ShouldIMove(GameState state)
    {
        if (state == GameState.Executing)
        {
            move = true;
            currentWaypointIndex = 0;
        }
        else
        {
            move = false;
        }
    }
    public override void AboutToDie()
    {
        base.AboutToDie();
        move = false;
    }
    public override void Die()
    {
        base.Die();
        sprites.SetActive(false);
    }
    protected override void HandleReset()
    {
        base.HandleReset();
        sprites.SetActive(true);
    }
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnStateChanged += ShouldIMove;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (GameManager.Instance)
        GameManager.Instance.OnStateChanged -= ShouldIMove;
    }
}