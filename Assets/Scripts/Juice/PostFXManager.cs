using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PostFXManager : Singleton<PostFXManager>
{
    [SerializeField] Volume codeFX;
    [SerializeField] Volume victoryFX;
    [SerializeField] float effectDecaySpeed = 1f;

    public void VictoryFX()
    {
        StartCoroutine(PlayVictoryEffect());
    }

    private IEnumerator PlayVictoryEffect()
    {
        if (victoryFX == null)
        yield break;

        victoryFX.weight = 1f;

        while (victoryFX.weight > 0)
        {
            victoryFX.weight -= effectDecaySpeed * Time.deltaTime;
            victoryFX.weight = Mathf.Max(victoryFX.weight, 0);
            yield return null;
        }

        victoryFX.weight = 0f;
    }

    private void SetProgrammingFX(GameState state)
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

    void Start()
    {
        codeFX.weight = 1f;
        GameManager.Instance.OnStateChanged += SetProgrammingFX;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= SetProgrammingFX;
    }
}
