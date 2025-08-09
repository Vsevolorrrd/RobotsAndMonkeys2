using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] Button runButton;

    private void Start()
    {
        if (runButton == null) return;

        runButton.onClick.AddListener(() =>
        {
            string[] lines = codeInputField.text.Split('\n');
            if (lines.Length > 0)
            {
                LoadProgram(lines[0]);
            }
        });
    }

    public void LoadProgram(string firstLine)
    {
        string trimmed = firstLine.Trim().ToLower();

        if (!string.IsNullOrEmpty(trimmed))
        {
            StartCoroutine(ExecuteCommand(trimmed));
        }
    }

    private IEnumerator ExecuteCommand(string commandLine)
    {
        switch (commandLine) // I was lasy, okay?
        {
            case "new game":
                SceneLoader.Instance.LoadScene("Intro");
                break;

            case "exit game":
                Application.Quit();
                break;

            case "load level(1)":
                SceneLoader.Instance.LoadScene("Level1");
                break;

            case "load level(2)":
                SceneLoader.Instance.LoadScene("Level2");
                break;

            case "load level(3)":
                SceneLoader.Instance.LoadScene("Level3");
                break;

            case "load level(4)":
                SceneLoader.Instance.LoadScene("Level4");
                break;

            case "load level(5)":
                SceneLoader.Instance.LoadScene("Level5");
                break;

            default:
                Debug.LogWarning($"Unknown command: {commandLine}");
                break;
        }

        yield return null;
    }
    public void loadLevel()
    {
        SceneLoader.Instance.LoadScene("Level1");
    }
}