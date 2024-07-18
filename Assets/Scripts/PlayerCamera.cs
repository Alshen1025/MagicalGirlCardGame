using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // 플레이어를 따라갈 타겟
    public Vector3 offset = new Vector3(0, 5, -10); // 카메라 오프셋

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}