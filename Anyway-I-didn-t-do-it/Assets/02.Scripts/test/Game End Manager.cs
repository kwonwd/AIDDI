//using SourGrape.hongyeop;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameEndManager : MonoBehaviour
//{
//    // �±װ� "Player"�� ������Ʈ���� ������ �ۺ� ����
//    public List<GameObject> Winners; // ���ڵ�
//    public List<GameObject> Ghosts; // ���ɵ�

//    private Camera _camera;

//    void Start()
//    {
//        _camera = Camera.main; // ���� ī�޶� ��������
//    }

//    void Update()
//    {
//        // Ű���� 1�� ������ ���� - �ʿ��� ȣ�� �������� �����ϸ� ��
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            FindWinners(); // ���� ã��
//            WinnersDance(); // ���� ��
//            FindGhosts(); // ���� ã��
//            GhostsBuster(); // ���� ���ֱ�
//            _camera.GetComponent<CameraController_dummy>().enabled = false; // ī�޶� ��Ʈ�ѷ� ����
//            _camera.GetComponent<GameEndCameraManager>().enabled = true; // ī�޶� ��Ʈ�ѷ� ����
//        }
//    }

//    private void FindWinners()
//    {
//        // �±װ� "Player"�� ��� ������Ʈ�� ã�Ƽ� �迭�� ����
//        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
//        Winners = new List<GameObject>();

//        foreach (GameObject player in allPlayers)
//        {
//            PlayerStatusController_dummy status = player.GetComponent<PlayerStatusController_dummy>();
//            if (status != null && !status.IsDead)
//            {
//                Winners.Add(player);
//            }
//        }
//    }

//    private void WinnersDance()
//    {
//        // Winners ����Ʈ�� �ִ� ��� ������Ʈ�� IsDance ������Ƽ�� true�� ����
//        foreach (GameObject player in Winners)
//        {
//            PlayerStatusController_dummy status = player.GetComponent<PlayerStatusController_dummy>();
//            if (status != null)
//            {
//                status.IsDance = true;
//            }
//        }
//    }

//    private void FindGhosts()
//    {
//        // �±װ� "PlayerGhost"�� ��� ������Ʈ�� ã�Ƽ� �迭�� ����
//        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("PlayerGhost");
//        Ghosts = new List<GameObject>();

//        foreach (GameObject ghost in allGhosts)
//        {
//            PlayerGhostController status = ghost.GetComponent<PlayerGhostController>();
//            if (status != null)
//            {
//                Ghosts.Add(ghost);
//            }
//        }
//    }

//    private void GhostsBuster()
//    {
//        // Ghosts ����Ʈ�� �ִ� ��� ������Ʈ�� IsDead ������Ƽ�� true�� ����
//        foreach (GameObject ghost in Ghosts)
//        {
//            PlayerGhostController status = ghost.GetComponent<PlayerGhostController>();
//            if (status != null)
//            {
//                status.IsDead = true;
//            }
//        }
//    }
//}