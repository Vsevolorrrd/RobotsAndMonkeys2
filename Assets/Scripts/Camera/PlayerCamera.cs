using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] CinemachineCamera cinemaCam;
    private bool activeCamera = false;
    void Update()
    {
        if (!activeCamera) return;
        UpdateCamPos();
        CamZoom();
    }
    private void UpdateCamPos()
    {

    }
    private void CamZoom()
    {

    }
    private void ChangeCameraMode(GameState state)
    {
        switch (state)
        {
            case GameState.Programming:
                activeCamera = false;
                break;
            case GameState.Executing:
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
        GameManager.Instance.OnStateChanged -= ChangeCameraMode;
    }
}