using UnityEngine;

public class RoomCreationHandler : MonoBehaviour
{
    // RoomCreationPanel GameObject'i buraya atanacak
    public GameObject roomCreationPanel;

    public void ShowRoomCreationPanel()
    {
        // Paneli aktif hale getirir
        if (roomCreationPanel != null)
        {
            roomCreationPanel.SetActive(true);
        }
    }
}
