using UnityEngine;

public class L_Switch : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] bool VictoryCondition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AI"))
        {
            if (VictoryCondition)
            {
                GameManager.Instance.PlayerWon();
                Invoke("Victory", 1f);
            }
        }
    }
    private void Victory()
    {
        if (sceneToLoad == null) return;
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }
}