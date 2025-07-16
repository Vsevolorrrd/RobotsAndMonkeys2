using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleCommandRunner : MonoBehaviour
{
    public TMP_InputField codeInput;
    public Player player;

    public void RunCode()
    {
        string rawCode = codeInput.text;
        string[] lines = rawCode.Split('\n');
        StartCoroutine(ExecuteCommands(lines));
    }

    IEnumerator ExecuteCommands(string[] lines)
    {
        foreach (string line in lines)
        {
            string command = line.Trim().ToLower();

            if (command.StartsWith("move"))
            {
                string[] parts = command.Split(' ');
                if (parts.Length > 1)
                    player.Move(parts[1]);
            }
            else if (command.StartsWith("turn"))
            {
                string[] parts = command.Split(' ');
                if (parts.Length > 1)
                    player.Turn(parts[1]);
            }
            else if (command == "attack")
            {
                player.Attack();
            }
            else if (command == "collect")
            {
                player.Collect();
            }
            else
            {
                Debug.LogWarning("Uknown command: " + command);
            }

            yield return new WaitForSeconds(0.5f); 
        }
    }
}

