using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaySceneManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;   // 플레이어 스폰 위치

    // Start is called before the first frame update
    void Start()
    {
        // 현재 방에 참여한 플레이어 인원
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 플레이어 인원에 따라 다른 스폰 위치에 플레이어 생성 (1명이면 0번, 2명이면 1번)
        PhotonNetwork.Instantiate("Player", playerSpawnPoints[playerCount - 1].position, Quaternion.identity);
    }

}
