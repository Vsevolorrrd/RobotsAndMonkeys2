using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoComplete : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text suggestionText;

    private string currentSuggestion = "";
    private bool suppressInput = false;
    private bool didit;

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
        suggestionText.text = "";
    }

    private void Update()
    {
        // Accept suggestion on Tab or RightArrow
        if (!string.IsNullOrEmpty(currentSuggestion) && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ApplySuggestion();
            didit = true;
        }
    }

    private  void LateUpdate()
    {
        if(didit)
        {
            didit = false;
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            inputField.caretPosition = inputField.text.Length;
        }
    }

    private void OnInputChanged(string fullText)
    {
        if (suppressInput)
        return;

        string currentWord = GetCurrentWord();
        if (string.IsNullOrWhiteSpace(currentWord))
        {
            suggestionText.text = "";
            currentSuggestion = "";
            return;
        }

        var match = AutoCompleteDatabase.Keywords.FirstOrDefault(k => k.StartsWith(currentWord) && k.Length > currentWord.Length);

        if (match != null)
        {
            string completedPart = match.Substring(currentWord.Length);
            currentSuggestion = completedPart;

            // Compose ghost text: typed input + gray suggestion
            suggestionText.text = fullText + $"<color=#888888>{completedPart}</color>";
        }
        else
        {
            suggestionText.text = fullText;
            currentSuggestion = "";
        }
    }

    private void ApplySuggestion()
    {
        suppressInput = true;
        string text = inputField.text;
        string completedText = text + currentSuggestion;
        inputField.text = completedText;
        inputField.caretPosition = completedText.Length;
        suggestionText.text = "";
        currentSuggestion = "";
        suppressInput = false;
    }

    private string GetCurrentWord()
    {
        int caretPos = inputField.caretPosition;
        string text = inputField.text;
        int start = text.LastIndexOfAny(new[] { ' ', '\n', '\t' }, caretPos - 1) + 1;
        int length = caretPos - start;

        if (start < 0 || caretPos > text.Length || length < 0 || start + length > text.Length)
            return "";

        return text.Substring(start, length);
    }
}
public static class AutoCompleteDatabase
{
    public static readonly List<string> Keywords = new List<string>()
    {
        "move forward",
        "move backward",
        "turn left",
        "turn right",
        "attack",
        "collect",
        "wait",
        "shoot",
        "main menu"
    };
}