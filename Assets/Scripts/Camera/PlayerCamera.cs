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

    }
    private void ChangeCameraMode(GameState state)
    {
        switch (state)
        {
            case GameState.Programming:
                activeCamera = false;
                cinemaCam.Follow = transform;
                cinemaCam.LookAt = transform;
                break;
            case GameState.Executing:
                cinemaCam.Follow = targetToFollow;
                cinemaCam.LookAt = targetToFollow;
                activeCamera = true;
                break;
        }
    }
    void Start()
    {
        GameManager.Instance.OnStateChanged += ChangeCameraMode;
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        GameManager.Instance.OnStateChanged -= ChangeCameraMode;
    }
}