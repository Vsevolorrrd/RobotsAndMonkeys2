using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AdvancedCodeWindow : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] Button runButton;
    [SerializeField] bool SetActiveOnStart = true;

    private void Start()
    {
        if (SetActiveOnStart)
        UI.SetActive(true);

        GameManager.Instance.OnStateChanged += SetUI;

        runButton.onClick.AddListener(() =>
        {
            string[] lines = codeInputField.text.Split('\n');
            CodeManager.Instance.LoadProgram(lines);
            GameManager.Instance.SetState(GameState.Executing);
        });
    }
    private void SetUI(GameState state)
    {
        switch (state)
        {
            case GameState.Programming:
                UI.SetActive(true);
                break;
            case GameState.Executing:
                UI.SetActive(false);
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= SetUI;
    }
}
[System.Serializable]
public class ComandLine
{
    public string firstLetter;
    public string fullLine;
    public string firstLetterSecondWord;
}