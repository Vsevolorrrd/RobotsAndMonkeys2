using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeWindow : MonoBehaviour
{
    public TMP_InputField codeInputField;
    public Button runButton;
    private CodeManager interpreter;

    private void Awake()
    {
        interpreter = CodeManager.Instance;
    }
    private void Start()
    {
        runButton.onClick.AddListener(() =>
        {
            string[] lines = codeInputField.text.Split('\n');
            interpreter.LoadProgram(lines);
        });
    }
}