using UnityEngine;
using UnityEngine.InputSystem;

public class InteractiveController : MonoBehaviour
{
    public Camera playerCamera;         // 플레이어 카메라
    public float interactRange = 3f;    // 상호작용 거리
    public LayerMask interactLayer;     // 상호작용 오브젝트 레이어
    public GameObject interactionUI;    // "상호작용 E" UI 오브젝트

    //public Material targetMaterial;
    //public float opaqueValue = 1f; //투명 0, 불투명 1
    //public float transparentValue = 0f; // 투명 0, 불투명 1

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작 시 투명 + 윤곽선 미표시로 초기화
        //SetOpaque(false);
        //SetOutline(false);
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

    // 투명/불투명 상태 전환 함수
    //public void SetOpaque(bool isOpaque)
    //{
    //    targetMaterial.SetFloat("_IsOpaque", isOpaque ? opaqueValue : transparentValue);
    //}

    //// 윤곽선 표시/숨김 함수
    //public void SetOutline(bool show)
    //{
    //    targetMaterial.SetFloat("_ShowOutline", show ? 1f : 0f);
    //}

}
