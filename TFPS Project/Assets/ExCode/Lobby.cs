using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI ���� Ŭ������ ����ϱ� ����
using Photon.Pun;       // ���� ���� Ŭ������ ���
using Photon.Realtime;  // �� ���� ���� Ŭ������ ����ϱ� ����

public class Lobby : MonoBehaviourPunCallbacks  // ���� ���� �ݹ� �Լ��� ����ϱ� ����
{
    public InputField roomNameInput;    // �� �̸� �Է¶�
    public Button createBtn;            // �� ���� ��ư
    public Text connectInfoTxt;         // ���� ��Ȳ �ؽ�Ʈ
    public Transform content;           // �� ����� ����� Scroll View�� Content
    public GameObject roomBtnPref;      // �� ���� ��ư

    // �����ϴ� ��� ���� �����ϴ� ����Ʈ
    List<RoomInfo> allRoomList = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ư�� OnClick() �Լ��� OnClickCreate() �Լ� ����
        createBtn.onClick.AddListener(OnClickCreate);

    }

    // ���� ��ư ������ ȣ��
    void OnClickCreate()
    {
        // �� �̸� �Է¶��� ����ִٸ� �Լ� Ż���Ͽ� �Ʒ� �ڵ� ���� �Ұ�
        if (roomNameInput.text == "") return;

        // �ߺ� Ŭ���� �����ϱ� ���� ��ư ��Ȱ��ȭ
        ButtonOnOff(false);

        // ���ο� �� �ɼ� ����
        RoomOptions roomOptions = new RoomOptions();

        // �ִ� �ο� �� 2������ ����
        roomOptions.MaxPlayers = 2;

        // ����ڰ� �Է��� �� �̸��� �� �ɼ����� �� ���� �õ�
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
        connectInfoTxt.text = "�� ���� ��...";

    }

    // ���� ��ư ������ ȣ��
    void OnClickJoin(string roomName)
    {
        // �ߺ� Ŭ���� �����ϱ� ���� ��ư ��Ȱ��ȭ
        ButtonOnOff(false);

        // ��ư�� �����ִ� �̸��� ������ ���� �õ�
        PhotonNetwork.JoinRoom(roomName);
    }

    // ��ư Ȱ��ȭ/��Ȱ��ȭ ó��
    void ButtonOnOff(bool isOn)
    {
        //�� ���� ��ư
        createBtn.interactable = isOn;

        // �� ��� UI�� �ִ� ��� ��ư
        foreach (var roomBtns in content.GetComponentsInChildren<Button>())
        {
            roomBtns.interactable = isOn;
        }

    }

    // �� ���ӿ� �����ϸ� ȣ��
    public override void OnJoinedRoom()
    {
        // PlayScene�̶�� �̸��� �� �ҷ�����(�� ��ȯ)
        PhotonNetwork.LoadLevel("PlayScene");
        connectInfoTxt.text = "�� ���� ����!";
    }

    // �� ���� ���� ��, ȣ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // �ٽ� ��ư Ŭ���� �� �ֵ��� Ȱ��ȭ
        ButtonOnOff(true);
        connectInfoTxt.text = "�� ���� ����";
    }

    // �κ� �����ϰų� �� ��Ͽ� ��ȭ�� ����� ȣ��
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ��ȭ�� ���� �� ����� ���� ��ü �� ����Ʈ ����
        foreach (var changedRoom in roomList)
        {
            // ��ü �� ����Ʈ�� �̹� �ִ� ���̶�� ����� ���̹Ƿ� ����
            if (allRoomList.Contains(changedRoom))
                allRoomList.Remove(changedRoom);

            // ��ü �� ����Ʈ�� ���� ���̶�� ������ ���̹Ƿ� �߰�
            else
                allRoomList.Add(changedRoom);
            
        }

        // �� ��� UI �ʱ�ȭ�� ���� �ݺ�
        for (int i = 0; i < content.childCount; i++)
        {
            // content�� ��� �ڽ� ����
            Destroy(content.GetChild(i).gameObject);
        }

        // �����ϴ� ��� ���� ����Ʈ��� ��ư ����
        foreach (var room in allRoomList)
        {
            // content�� �ڽ����� ��ư ���� ���� ��, ���纻�� ������ ����
            GameObject roomBtn = Instantiate(roomBtnPref, content);

            // ��ư�� �ڽ� Text ������Ʈ�� ���� �̸� ���
            Text roomNameTxt = roomBtn.GetComponentInChildren<Text>();
            roomNameTxt.text = room.Name;

            // ���� ��ư�� OnClick()�Լ��� OnClickJoin() �Լ� ���� �� ���� �̸� ����
            roomBtn.GetComponent<Button>().onClick.AddListener(() => OnClickJoin(room.Name));

        }
    }

}
