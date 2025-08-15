using TMPro;
using UnityEngine;

public class GhostTextFollower : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text ghost;          

    RectTransform srcRT; 
    RectTransform ghostRT;

    void Awake()
    {
        if (!inputField || !ghost)
        {
            enabled = false;
            return;
        }

        srcRT = inputField.textComponent.rectTransform;
        ghostRT = ghost.rectTransform;
    }

    void LateUpdate()
    {
        ghostRT.anchoredPosition = srcRT.anchoredPosition;
    }
}
