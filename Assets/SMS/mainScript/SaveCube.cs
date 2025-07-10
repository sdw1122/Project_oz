using UnityEngine;
using Photon.Pun;

public class SaveCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PhotonView pv = other.GetComponent<PhotonView>();
        if (pv != null && pv.IsMine && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("SaveCube �浹 - ���� ��û ����");

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView targetPV = player.GetComponent<PhotonView>();
                if (targetPV != null)
                {
                    // �� �÷��̾�� ��ġ ��û ������
                    targetPV.RPC("SendMyDataToHost", targetPV.Owner);
                }
            }
        }
    }
}
