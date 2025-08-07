using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeWindow : MonoBehaviour
{
    [SerializeField] GameObject programmingUI;
    [SerializeField] GameObject runUI;
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] Button runButton;
    [SerializeField] bool SetActiveOnStart = true;

    private void Start()
    {
        if (SetActiveOnStart)
        programmingUI.SetActive(true);

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
                programmingUI.SetActive(true);
                runUI.SetActive(false);
                break;
            case GameState.Executing:
                programmingUI.SetActive(false);
                runUI.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= SetUI;
    }
}