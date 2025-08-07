using System;
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
    private bool autoCompleted;

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
            autoCompleted = true;
        }
    }

    private void LateUpdate()
    {
        if (autoCompleted)
        {
            autoCompleted = false;

            int tabPos = inputField.text.IndexOf('\t');
            if (tabPos >= 0)
            {
                string text = inputField.text;
                text = text.Remove(tabPos, 1);
                inputField.text = text;

                inputField.caretPosition = tabPos;
            }
        }
    }

    private void OnInputChanged(string fullText)
    {
        if (suppressInput)
        return;

        int caretPos = inputField.caretPosition;
        string text = inputField.text;

        // find current word
        int wordStart = text.LastIndexOfAny(new[] { ' ', '\n', '\t' }, caretPos - 1) + 1;
        int wordLength = caretPos - wordStart;

        if (wordStart < 0 || wordLength < 0 || wordStart + wordLength > text.Length)
        {
            suggestionText.text = "";
            currentSuggestion = "";
            return;
        }

        string currentWord = text.Substring(wordStart, wordLength);

        if (string.IsNullOrWhiteSpace(currentWord))
        {
            suggestionText.text = "";
            currentSuggestion = "";
            return;
        }

        var match = AutoCompleteDatabase.Keywords.FirstOrDefault(k =>
        k.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase) &&
        k.Length > currentWord.Length);

        if (match != null)
        {
            string completedPart = match.Substring(currentWord.Length);
            currentSuggestion = completedPart;

            string beforeWord = text.Substring(0, wordStart);
            string afterWord = text.Substring(caretPos);
            string ghostWord = currentWord + $"<color=#888888>{completedPart}</color>";
            suggestionText.text = beforeWord + ghostWord + afterWord;
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

        int caretPos = inputField.caretPosition;
        string originalText = inputField.text;

        int wordStart = originalText.LastIndexOfAny(new[] { ' ', '\n', '\t' }, caretPos - 1) + 1;
        int wordLength = caretPos - wordStart;

        if (wordStart < 0 || wordStart + wordLength > originalText.Length)
        {
            suppressInput = false;
            return;
        }

        string before = originalText.Substring(0, wordStart);
        string after = originalText.Substring(caretPos);
        string completedWord = originalText.Substring(wordStart, wordLength) + currentSuggestion;
        string newText = before + completedWord + after;

        inputField.text = newText;
        inputField.caretPosition = before.Length + completedWord.Length;
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