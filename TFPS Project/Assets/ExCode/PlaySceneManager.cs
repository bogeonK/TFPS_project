using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaySceneManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;   // �÷��̾� ���� ��ġ

    // Start is called before the first frame update
    void Start()
    {
        // ���� �濡 ������ �÷��̾� �ο�
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // �÷��̾� �ο��� ���� �ٸ� ���� ��ġ�� �÷��̾� ���� (1���̸� 0��, 2���̸� 1��)
        PhotonNetwork.Instantiate("Player", playerSpawnPoints[playerCount - 1].position, Quaternion.identity);
    }

}
