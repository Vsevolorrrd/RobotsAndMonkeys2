using System.Collections;
using UnityEngine;

public class L_MoveObject : MonoBehaviour
{
    private Vector3 initialPosition;
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
