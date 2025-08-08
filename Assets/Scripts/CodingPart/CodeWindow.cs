using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeWindow : Singleton<CodeWindow>
{
    [SerializeField] GameObject programmingUI;
    [SerializeField] GameObject runUI;
    [SerializeField] TextMeshProUGUI runText;
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] Button runButton;
    [SerializeField] bool SetActiveOnStart = true;

    private string[] codeLines;

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
            codeLines = lines;
            UpdateCodeHighlight(0);
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
    public void UpdateCodeHighlight(int currentIndex)
    {
        if (codeLines == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < codeLines.Length; i++)
        {
            if (i == currentIndex)
            sb.AppendLine($"<color=red>{codeLines[i]}</color>");
            else
            sb.AppendLine(codeLines[i]);
        }

        runText.text = sb.ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= SetUI;
    }
}