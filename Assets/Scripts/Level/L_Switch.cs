using UnityEngine;

public class L_Switch : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AI"))
        {
            if (sceneToLoad == null) return;
            SceneLoader.Instance.LoadScene(sceneToLoad);
        }
    }
}