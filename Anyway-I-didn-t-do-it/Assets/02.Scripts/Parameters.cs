using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SourGrape.bbbb
{
    public class Parameters : MonoBehaviour
    {
        // Web
        static public string URL = "http://j11a708.p.ssafy.io/";

        // GameManager
        static public float GameTime = 120.0f;
        static public int MaxPlayer = 4;

        // Arrow Manager Parameter;
        static public int shootLocation = 36;
        static public int randomArrow = 7;
        static public int targetArrow = 2;

        // Arrow Parameter
        static public float ArrowHeight = 1.0f;
        static public float ArrowSpeed = 5.0f;
        static public Vector3 ArrowSize = new Vector3(5, 5, 5);

        // Error Message
        // How to Use
        // GameObject _obj = Instantiate(Resources.Load<GameObject>(Parameters.ErrorPanel));
        // _obj.GetComponent<bbbb.ErrorPanel>().message = "";

        static public string ErrorPanel = "bbbb/ErrorPanel";
        static public string NeedMorePlayers = "4����� ������ �� �ֽ��ϴ�!";
        static public string WrongRoomCode = "�������� �ʴ� ���Դϴ�!";
        static public string MsgCopyCode = "�� �ڵ尡 ����Ǿ����ϴ�!";
        static public string MsgDisconnect = "������ ���������ϴ�!";

    }
}
