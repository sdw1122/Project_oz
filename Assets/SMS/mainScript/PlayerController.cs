using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    

    Rigidbody rigid;
    PhotonView pv;

    public float JumpPower = 5.0f;
    public float moveSpeed = 5.0f;
    bool isJump;

    public float mouseSensitivity = 100f; // 마우스 감도
    private float xRotation = 0f; // 상하 시야 각도

    public string job;
    [PunRPC]
    public void SetJob(string _job)
    {
        job = _job;
        Debug.Log($"[PlayerController] Job 설정됨: {job}");
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        pv= GetComponent<PhotonView>();
        
    }
    private void Start()
    {
        if (!pv.IsMine) return;
        if(playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void FixedUpdate()
    {
        if (pv.IsMine) 
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = transform.right * h + transform.forward * v;
            moveDirection.Normalize();

            // Rigidbody.velocity를 직접 제어
            // y축 속도는 중력에 의해 변경되므로 현재 속도를 유지
            rigid.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rigid.linearVelocity.y, moveDirection.z * moveSpeed);



            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            // 상하 시야 회전 (카메라만)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 시야 각도 제한 (고개를 너무 뒤로 젖히거나 숙이지 않도록)
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // 좌우 시야 회전 (플레이어 몸체와 함께)
            transform.Rotate(Vector3.up * mouseX);
        }
       
    }

    private void Update()
    {   if(pv.IsMine) 
        {
            if (Input.GetButtonDown("Jump") && !isJump)
            {
                isJump = true;
                rigid.AddForce(new Vector3(0, JumpPower, 0), ForceMode.Impulse);
            }
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pv.IsMine) 
        {
            if (collision.gameObject.name == "Plane")
                isJump = false;
        }
       
    }
    [PunRPC]
    void SendMyDataToHost()
    {
        if (!pv.IsMine) return;
        
        PlayerSaveData myData = new PlayerSaveData
        {
            userId = PhotonNetwork.LocalPlayer.UserId,
            userJob=job,
            position = transform.position,
            // 나중에 저장할 데이터 추가
        };
        GameObject gm = GameObject.Find("GameManager");
        PhotonView gmView = gm.GetComponent<PhotonView>();

        //gm에게 데이터 전달
        string json = JsonUtility.ToJson(myData);
        gmView.RPC("ReceivePlayerData", RpcTarget.MasterClient, json);
    }
    


}
