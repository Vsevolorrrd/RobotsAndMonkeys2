using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : Singleton<CodeManager>
{
    public Minion minion;
    private Queue<string> commands = new();

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
    }

    private IEnumerator ExecuteCommand(string commandLine)
    {
        string[] parts = commandLine.Split(' ');
        string command = parts[0];

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
                minion.Attack();
                break;

            case "collect":
                minion.Collect();
                break;

            case "wait":
                yield return new WaitForSeconds(1f);
                break;

            default:
                Debug.LogWarning($"Unknown command: {commandLine}");
                break;
        }

        yield return null;
    }
}
