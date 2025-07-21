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
    [SerializeField] Button checkBtn;
    [SerializeField] Button makeCodeBtn;
    [SerializeField] Button exitBtn;
    

    [SerializeField] Image trailsTitle;
 

    IEnumerator Start()
    {
        //완료 되기 전까지 버튼 막음
        checkBtn.interactable = false;
        makeCodeBtn.interactable = false;
        roomCodeInput.interactable = false;


        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

        statusText.text = " 서버 연결중 ";
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        playerCount.text = "1";
        gameStartBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "마스터 서버 연결 완료";
    }

    public override void OnJoinedLobby()
    {
        statusText.text = "동기화 완료";
        checkBtn.interactable = true;
        makeCodeBtn.interactable = true;
        roomCodeInput.interactable = true;
    }
    public void OnCreateRoomButton()
    {
        string roomName = Random.Range(10000, 99999).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
        codeText.text = $"방 생성 중";
    }

    public void OnJoinRoomButton()
    {

        PhotonNetwork.JoinRoom(roomCodeInput.text);
        statusText.text = $"방 입장 시도: {roomCodeInput.text}";
    }



    public override void OnJoinedRoom()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        codeText.text = "방 입장 완료 : " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("입장한 방 이름: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        //방 들어가면 버튼 막음
        checkBtn.interactable = false;
        makeCodeBtn.interactable = false;
        roomCodeInput.interactable = false;

        exitBtn.gameObject.SetActive(true);
        statusText.text = "플레이어 기다리는 중..";
        StartCoroutine(MoveTitleUp(150f, 0.5f));
        if (!PhotonNetwork.IsMasterClient)
        {
            statusText.text = "방 입장 완료";
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();


        if (PhotonNetwork.IsMasterClient && count == 2)
        {
            gameStartBtn.gameObject.SetActive(true);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        playerCount.text = count.ToString();



        checkBtn.interactable = true;
        makeCodeBtn.interactable = true;
        roomCodeInput.interactable = true;

        

        if (PhotonNetwork.IsMasterClient && count < 2)
        {
            gameStartBtn.gameObject.SetActive(false);
        }
    }
    public override void OnLeftRoom()
    {
        exitBtn.gameObject.SetActive(false);

        StartCoroutine(MoveTitleUp(-150f, 0.5f));

        codeText.text = "-----";
        playerCount.text = "1";
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartBtn.gameObject.SetActive(false);
            
        }
    }
    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "입장 실패: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "생성 실패: " + message;
    }

    IEnumerator MoveTitleUp(float offsetY, float duration)
    {
        RectTransform rect = trailsTitle.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0f, offsetY);

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        rect.anchoredPosition = targetPos; // 최종 보정
    }










    public void SceneChange()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
