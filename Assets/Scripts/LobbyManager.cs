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
        statusText.text = " 서버 연결중 ";
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);
        statusText.text = " 서버 연결 완료 ";
        playerCount.text = "1";
        gameStartBtn.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "마스터 서버 연결 완료";
    }

    public override void OnJoinedLobby()
    {
        statusText.text = "동기화 완료!";
        isReadyToJoin = true;
    }
    public void OnCreateRoomButton()
    {
        string roomName = Random.Range(10000, 99999).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
        codeText.text = $"방 생성 중";
    }

    public void OnJoinRoomButton()
    {
        if (!isReadyToJoin)
        {
            statusText.text = "동기화 중 입니다 다시 시도해주세요";
            return;
        }

        PhotonNetwork.JoinRoom(roomCodeInput.text);
        statusText.text = $"방 입장 시도: {roomCodeInput.text}";
    }



    public override void OnJoinedRoom()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        codeText.text = "방 입장 완료 : " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("입장한 방 이름: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);


        if (!PhotonNetwork.IsMasterClient)
        {
            statusText.text = "방 입장 완료!";
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();

        Debug.Log($"[입장] {newPlayer.NickName}, 총 인원: {count}");

        if (PhotonNetwork.IsMasterClient && count == 2)
        {
            gameStartBtn.gameObject.SetActive(true);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();

        Debug.Log($"[퇴장] {otherPlayer.NickName}, 총 인원: {count}");

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
        statusText.text = "입장 실패: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "생성 실패: " + message;
    }

    public void SceneChange()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
