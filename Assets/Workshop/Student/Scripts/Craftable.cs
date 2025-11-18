using UnityEngine;

public class Craftable : MonoBehaviour
{
    public GameObject uiPanel;   // ลาก UI Panel มาใส่ใน Inspector

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isOpen = !isOpen;                // สลับสถานะ เปิด/ปิด
            uiPanel.SetActive(isOpen);       // เปิดหรือปิด UI
        }
    }
}
