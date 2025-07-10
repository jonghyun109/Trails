using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomCodeInput;
    [SerializeField] TMP_Text codeText;
    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] Button gameStartBtn;
    [SerializeField] bool isReadyToJoin = false;

    IEnumerator Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        statusText.text = " ���� ������ ";
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);
        statusText.text = " ���� ���� �Ϸ� ";
        playerCount.text = "1";
        gameStartBtn.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "������ ���� ���� �Ϸ�";
    }

    public override void OnJoinedLobby()
    {
        statusText.text = "����ȭ �Ϸ�!";
        isReadyToJoin = true;
    }
    public void OnCreateRoomButton()
    {
        string roomName = Random.Range(10000, 99999).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
        codeText.text = $"�� ���� ��";
    }

    public void OnJoinRoomButton()
    {
        if (!isReadyToJoin)
        {
            statusText.text = "����ȭ �� �Դϴ� �ٽ� �õ����ּ���";
            return;
        }

        PhotonNetwork.JoinRoom(roomCodeInput.text);
        statusText.text = $"�� ���� �õ�: {roomCodeInput.text}";
    }



    public override void OnJoinedRoom()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        codeText.text = "�� ���� �Ϸ� : " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("������ �� �̸�: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);


        if (!PhotonNetwork.IsMasterClient)
        {
            statusText.text = "�� ���� �Ϸ�!";
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();

        Debug.Log($"[����] {newPlayer.NickName}, �� �ο�: {count}");

        if (PhotonNetwork.IsMasterClient && count == 2)
        {
            gameStartBtn.gameObject.SetActive(true);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();

        Debug.Log($"[����] {otherPlayer.NickName}, �� �ο�: {count}");

        if (PhotonNetwork.IsMasterClient && count < 2)
        {
            gameStartBtn.gameObject.SetActive(false);
        }
    }
    public override void OnLeftRoom()
    {
        playerCount.text = "1";
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartBtn.gameObject.SetActive(false);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "���� ����: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "���� ����: " + message;
    }

    public void SceneChange()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
