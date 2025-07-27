using UnityEngine;

public class L_FloorButton : MonoBehaviour
{
    [SerializeField] L_Door[] doors;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var door in doors)
        {
            door.OpenDoor(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (var door in doors)
        {
            door.OpenDoor(false);
        }
    }
}