using UnityEngine;

public class L_Door : MonoBehaviour
{
    [SerializeField] GameObject doorPart;
    [SerializeField] Animator animator;
    private Vector3 initialScale;
    public void OpenDoor(bool state)
    {
        GetComponent<Collider2D>().enabled = !state;
        animator.SetBool("OpenDoor", state);
    }
    private void HandleReset()
    {
        doorPart.transform.localScale = initialScale;
        StopAllCoroutines();
    }

    private void Start()
    {
        initialScale = transform.localScale;
        GameManager.Instance.OnGameReset += HandleReset;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        GameManager.Instance.OnGameReset -= HandleReset;
    }
}
