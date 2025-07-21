using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool codeIsRunnig = false;
    public bool CodeIsRunning() {  return codeIsRunnig; }

    public void SwitchState(bool state)
    {

    }
}