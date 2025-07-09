using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomCodeInput;
    [SerializeField] TMP_Text codeText;
    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] Button gameStartBtn;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        playerCount.text = "1";
        gameStartBtn.gameObject.SetActive(false);
    }

    public void OnCreateRoomButton()
    {
        string roomCode = Random.Range(100000, 999999).ToString();
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(roomCode, options);
        codeText.text = $"�� ���� ��: {roomCode}";
    }

    public void OnJoinRoomButton()
    {
        string roomCode = roomCodeInput.text;
        if (!string.IsNullOrEmpty(roomCode))
        {
            PhotonNetwork.JoinRoom(roomCode);
            statusText.text = $"�� ���� �õ�: {roomCode}";
        }
    }

    

    public override void OnJoinedRoom()
    {
        playerCount.text = "2";
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartBtn.gameObject.SetActive(true);            
        }
        else
        {
            statusText.text = $"�� ���� �Ϸ�!";
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
