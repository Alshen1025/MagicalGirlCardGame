using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardManager : MonoBehaviourPunCallbacks
{
    public GameObject playerBoardPrefab; // 플레이어 보드 프리팹
    public Transform[] boardPositions; // 플레이어 보드 위치 배열 (최대 4개)
    private List<GameObject> playerBoards = new List<GameObject>(); // 생성된 플레이어 보드 리스트

    // void Start()
    // {
    //     // if (PhotonNetwork.IsConnected)
    //     // {
    //     //     InitializePlayerBoards();
    //     // }
    // }

    public void InitializePlayerBoards()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject board = PhotonNetwork.Instantiate(playerBoardPrefab.name, boardPositions[i].position, Quaternion.identity);
            playerBoards.Add(board);

            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                // 로컬 플레이어 보드 설정
                PlayerBoard localBoard = board.GetComponent<PlayerBoard>();
                localBoard.SetAsLocalPlayer();
            }
        }

        // 빈 보드를 생성하여 남은 위치에 추가
        for (int i = PhotonNetwork.PlayerList.Length; i < boardPositions.Length; i++)
        {
            GameObject board = Instantiate(playerBoardPrefab, boardPositions[i].position, Quaternion.identity);
            playerBoards.Add(board);
            board.SetActive(false); // 플레이어가 없는 보드는 비활성화
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 들어오면 보드를 추가
        int playerIndex = PhotonNetwork.PlayerList.Length - 1;
        GameObject board = PhotonNetwork.Instantiate(playerBoardPrefab.name, boardPositions[playerIndex].position, Quaternion.identity);
        playerBoards.Add(board);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 플레이어가 나가면 보드를 제거
        int playerIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, otherPlayer);
        Destroy(playerBoards[playerIndex]);
        playerBoards.RemoveAt(playerIndex);
    }
}