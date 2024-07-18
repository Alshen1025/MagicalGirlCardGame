using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // 플레이어 프리팹
    private List<GameObject> players = new List<GameObject>(); // 생성된 플레이어 리스트
    private CardManager cardManager;
    private PlayerBoardManager playerBoardManager;



    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        playerBoardManager = FindObjectOfType<PlayerBoardManager>();
    }
    public void GameReset()
    {
        if (PhotonNetwork.IsConnected)
        {
            InitializePlayers();
            StartCoroutine(cardManager.ResetCards());
            playerBoardManager.InitializePlayerBoards();
        }
    }

    void InitializePlayers()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 플레이어의 시작 위치를 가져와서 생성
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, GetStartPosition(i), Quaternion.identity);
            players.Add(player);
        }
    }

    Vector3 GetStartPosition(int index)
    {
        // 플레이어의 시작 위치를 결정하는 코드 (예: 원형으로 배치할 경우)
        float angle = index * (360f / PhotonNetwork.PlayerList.Length);
        Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * 5f;
        return position;
    }

    // 플레이어가 방에 입장했을 때 호출되는 콜백 메서드
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, GetStartPosition(players.Count), Quaternion.identity);
        players.Add(player);
    }

    // 플레이어가 방에서 나갔을 때 호출되는 콜백 메서드
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        int index = players.FindIndex(p => p.GetComponent<PhotonView>().Owner.UserId == otherPlayer.UserId);
        if (index >= 0)
        {
            Destroy(players[index]);
            players.RemoveAt(index);
        }
    }
}