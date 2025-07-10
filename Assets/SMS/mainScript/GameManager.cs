using Photon.Pun;
using UnityEngine;
using static LobbyManager;

public class GameManager : MonoBehaviourPun
{
    Vector3 spawnPos = new Vector3(0, 1, 0);
    PlayerSaveData savedData;
    void Start()
    {
        string userId = PhotonNetwork.LocalPlayer.UserId;

        // ����� ������ �ε�
        savedData = SaveSystem.LoadPlayerData(userId);

        if (savedData != null)
        {
            spawnPos = savedData.position;
            Debug.Log($"[GameManager] ����� ��ġ�� ����: {spawnPos}");
        }
        else
        {
            Debug.Log("[GameManager] ����� ������ ����. �⺻ ��ġ ���");
        }

        InstantiatePlayer();
    }

    // �÷��̾� ������ ����
    void InstantiatePlayer()
    {
        string userId = PhotonNetwork.LocalPlayer.UserId;

        if (GameObject.Find(userId) != null)
        {
            Debug.Log("[GameManager] �̹� �÷��̾ �����մϴ�!!");
            return;
        }
        // job�� ���� ������ ����
        string job = TempMemory.MySaveData != null ? TempMemory.MySaveData.userJob : "pen"; // �⺻���� pen
        /*if (savedData != null)
        {
            job = savedData.userJob;
        }*/
        string prefabName = "";

        switch (job)
        {
            case "pen":
                prefabName = "PenPlayer"; 
                break;
            case "eraser":
                prefabName = "EraserPlayer"; 
                break;
            default:
                prefabName = "Player"; // ���� ������
                break;
        }

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);
        player.name = userId;
        // ���� ���� ���� job ����
        if (player.GetComponent<PhotonView>().IsMine)
        {
            player.GetComponent<PhotonView>().RPC("SetJob", RpcTarget.AllBuffered, job);
        }
        Debug.Log($"[GameManager] ���� ������ ���� �Ϸ�: {prefabName}");
    }


    // ������ ����
    [PunRPC]
    public void ReceivePlayerData(string json)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
        SaveSystem.SavePlayerData(data);

        Debug.Log($"[GameManager] Player {data.userId} ������ ���� �Ϸ�: pos={data.position}");
    }
}
