using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] CinemachineCamera cinemaCam;
    [SerializeField] Transform targetToFollow;
    private bool activeCamera = false;

    void Update()
    {
        if (!activeCamera) return;
        UpdateCamPos();
    }
    private void UpdateCamPos()
    {
        if (cinemaCam.Follow != targetToFollow && targetToFollow != null)
        {
            cinemaCam.Follow = targetToFollow;
            cinemaCam.LookAt = targetToFollow;
        }
    }
    private void ChangeCameraMode(GameState state)
    {
        switch (state)
        {
            case GameState.Programming:
                activeCamera = false;
                cinemaCam.enabled = false;
                break;
            case GameState.Executing:
                activeCamera = true;
                cinemaCam.enabled = true;
                break;
        }
    }
    void Start()
    {
        cinemaCam.enabled = false;
        GameManager.Instance.OnStateChanged += ChangeCameraMode;
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        GameManager.Instance.OnStateChanged -= ChangeCameraMode;
    }
}