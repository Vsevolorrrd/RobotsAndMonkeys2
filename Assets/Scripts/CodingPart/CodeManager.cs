using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : Singleton<CodeManager>
{
    public Minion minion;
    private Queue<string> commands = new();
    private bool playerWon = false;
    public void PlayerWon() => playerWon = true;

    public void LoadProgram(string[] lines)
    {
        commands.Clear();

        foreach (string line in lines)
        {
            string trimmed = line.Trim().ToLower();
            if (!string.IsNullOrEmpty(trimmed))
            commands.Enqueue(trimmed);
        }

        StartCoroutine(ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        while (commands.Count > 0)
        {
            string commandLine = commands.Dequeue();
            yield return ExecuteCommand(commandLine);
            yield return new WaitForSeconds(0.5f); // Delay between commands
        }
        yield return new WaitForSeconds(1f);
        if (!playerWon)
        GameManager.Instance.ResetLevel();
    }

    private IEnumerator ExecuteCommand(string commandLine)
    {
        string raw = commandLine.Trim().ToLower();

        #region Looping Actions

        int repeatCount = 1;
        // basically find the brackets and use them to get what's in between
        if (raw.Contains('(') && raw.EndsWith(")"))
        {
            int open = raw.LastIndexOf('(');
            int close = raw.LastIndexOf(')');

            string repeatPart = raw.Substring(open + 1, close - open - 1);
            if (int.TryParse(repeatPart, out int parsed))
            repeatCount = Mathf.Max(1, parsed);

            raw = raw.Substring(0, open).Trim();
        }

        #endregion

        string[] parts = raw.Split(' ');
        string command = parts[0];

        for (int i = 0; i < repeatCount; i++)
        {
            switch (command)
            {
                case "move":
                    if (parts.Length > 1)
                    yield return StartCoroutine(minion.Move(parts[1]));
                    else
                    Debug.LogWarning("move command missing direction");
                    break;

                case "turn":
                    if (parts.Length > 1)
                    yield return StartCoroutine(minion.Turn(parts[1]));
                    else
                    Debug.LogWarning("turn command missing directio");
                    break;

                case "attack":
                    yield return StartCoroutine(minion.Attack());
                    break;

                case "collect":
                    minion.Collect();
                    break;

                case "wait":
                    yield return new WaitForSeconds(0.5f);
                    break;

                default:
                    Debug.LogWarning($"Unknown command: {commandLine}");
                    break;
            }

            if (repeatCount > 1)
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}