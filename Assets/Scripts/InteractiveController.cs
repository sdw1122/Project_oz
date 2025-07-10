using UnityEngine;
using UnityEngine.InputSystem;

public class InteractiveController : MonoBehaviour
{
    public Camera playerCamera;         // 플레이어 카메라
    public float interactRange = 3f;    // 상호작용 거리
    public LayerMask interactLayer;     // 상호작용 오브젝트 레이어
    public GameObject interactionUI;    // "상호작용 E" UI 오브젝트

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            interactionUI.SetActive(true);

            // E키를 누르면 상호작용 실행
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("작동");
                
            }
        }
        else
        {
            interactionUI.SetActive(false);
        }
    }
}
