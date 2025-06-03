using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public CinemachineVirtualCamera topView;
    public CinemachineVirtualCamera platformView;
    public Transform playerTF;

    public bool changeView = false;
    private bool isRotating = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isRotating)
        {
            changeView = !changeView;

            if (changeView)
            {
                topView.Priority = 11;
                platformView.Priority = 10;
                StartCoroutine(SmoothRotate(playerTF, Quaternion.Euler(30f, 0f, 0f), 1f));
            }
            else
            {
                topView.Priority = 10;
                platformView.Priority = 11;
                StartCoroutine(SmoothRotate(playerTF, Quaternion.Euler(0f, 0f, 0f), 1f));
            }
        }
    }

    IEnumerator SmoothRotate(Transform t, Quaternion targetRot, float duration)
    {
        isRotating = true;

        Quaternion startRot = t.rotation;
        float time = 0;
        while (time < duration)
        {
            t.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        t.rotation = targetRot;
        isRotating = false;
    }

}
