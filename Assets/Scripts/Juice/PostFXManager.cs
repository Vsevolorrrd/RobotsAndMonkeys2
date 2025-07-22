using UnityEngine;
using UnityEngine.Rendering;

public class PostFXManager : MonoBehaviour
{
    [SerializeField] Volume codeFX;

    void Start()
    {
        codeFX.weight = 1f;
        GameManager.Instance.OnStateChanged += SetFX;
    }

    private void SetFX(GameState state)
    {
        switch (state)
        {
            case GameState.Programming:
                codeFX.weight = 1f;
                break;
            case GameState.Executing:
                codeFX.weight = 0f;
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= SetFX;
    }
}
