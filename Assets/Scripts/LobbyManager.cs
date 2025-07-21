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
        //�Ϸ� �Ǳ� ������ ��ư ����
        checkBtn.interactable = false;
        makeCodeBtn.interactable = false;
        roomCodeInput.interactable = false;


        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

        statusText.text = " ���� ������ ";
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        playerCount.text = "1";
        gameStartBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "������ ���� ���� �Ϸ�";
    }

    public override void OnJoinedLobby()
    {
        statusText.text = "����ȭ �Ϸ�";
        checkBtn.interactable = true;
        makeCodeBtn.interactable = true;
        roomCodeInput.interactable = true;
    }
    public void OnCreateRoomButton()
    {
        string roomName = Random.Range(10000, 99999).ToString();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
        codeText.text = $"�� ���� ��";
    }

    public void OnJoinRoomButton()
    {

        PhotonNetwork.JoinRoom(roomCodeInput.text);
        statusText.text = $"�� ���� �õ�: {roomCodeInput.text}";
    }



    public override void OnJoinedRoom()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        codeText.text = "�� ���� �Ϸ� : " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("������ �� �̸�: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        //�� ���� ��ư ����
        checkBtn.interactable = false;
        makeCodeBtn.interactable = false;
        roomCodeInput.interactable = false;

        exitBtn.gameObject.SetActive(true);
        statusText.text = "�÷��̾� ��ٸ��� ��..";
        StartCoroutine(MoveTitleUp(150f, 0.5f));
        if (!PhotonNetwork.IsMasterClient)
        {
            statusText.text = "�� ���� �Ϸ�";
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
        statusText.text = "���� ����: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "���� ����: " + message;
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

        rect.anchoredPosition = targetPos; // ���� ����
    }










    public void SceneChange()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
