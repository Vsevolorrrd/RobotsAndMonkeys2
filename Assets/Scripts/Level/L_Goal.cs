using UnityEngine;

public class L_Goal : MonoBehaviour
{
    [SerializeField] Killable[] monkeysToKill;
    [SerializeField] string sceneToLoad;
    private int monkeyKilled;

    private void Awake()
    {
        foreach (var monkey in monkeysToKill)
        {
            monkey.SetAsTarget(this);
        }
    }

    public void MonkeyKilled()
    {
        monkeyKilled++;
        if (monkeyKilled >= monkeysToKill.Length)
        {
            GameManager.Instance.PlayerWon();
            Invoke("Victory", 1f);
        }
    }

    private void Victory()
    {
        if (sceneToLoad == null) return;
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    private void HandleReset()
    {
        monkeyKilled = 0;
    }

    private void Start()
    {
        GameManager.Instance.OnGameReset += HandleReset;
    }
    private void OnDestroy()
    {
        if (GameManager.Instance)
        GameManager.Instance.OnGameReset -= HandleReset;
    }
}