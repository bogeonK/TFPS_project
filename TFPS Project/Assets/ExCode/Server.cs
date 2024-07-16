using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI ���� Ŭ������ ����ϱ� ����
using Photon.Pun;       // ���� ���� Ŭ������ ���
using Photon.Realtime;  // OnDisconnected �Լ� �����ϸ� �ڵ����� ����

public class Server : MonoBehaviourPunCallbacks // ���� ���� �ݹ� �Լ��� ��ӹޱ� ����
{

    public Button startBtn;     // ���� ��ư
    public GameObject lobby;    // �κ� ȭ��
    public Text connectInfoTxt; // ���� ��Ȳ �ؽ�Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ư�� OnClick() �Լ��� onClickStart() �Լ� ����
        startBtn.onClick.AddListener(OnClickStart);

        // ���� ��ư ��Ȱ��ȭ ���·� ����
        startBtn.interactable = false;

        // ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        connectInfoTxt.text = "���� ���� ��...";

    }

    // ���� ��ư ������ ȣ��
    void OnClickStart()
    {
        PhotonNetwork.JoinLobby();  // �κ� ���� �õ�
        connectInfoTxt.text = "�κ� ���� ��...";
    }

    // ���� ���ӿ� �����ϸ� ȣ��
    public override void OnConnectedToMaster()
    {
        // ���� ��ư Ȱ��ȭ
        startBtn.interactable = true;
        connectInfoTxt.text = "���� ���� ����!";
    }

    // �κ� ���ӿ� �����ϸ� ȣ��
    public override void OnJoinedLobby()
    {
        // �κ� ȭ������ ��ȯ
        gameObject.SetActive(false);
        lobby.SetActive(true);
        connectInfoTxt.text = "�κ� ���� ����!";

    }

    // ���� �����ϸ� ȣ��
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ���� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
        connectInfoTxt.text = "���� ����, ���� ������ ��...";

    }
}
