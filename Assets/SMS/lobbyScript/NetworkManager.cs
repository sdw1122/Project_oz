using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings();

    }

    /*public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� ����");
        RoomOptions rm = new RoomOptions();
        rm.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("OZ", rm, TypedLobby.Default);
    }*/


    /*public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    public override void OnJoinedRoom()
    {
        GameObject PI = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }*/
}
