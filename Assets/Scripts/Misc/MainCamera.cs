using UnityEngine;
using System.Collections;
using System;
using Extension.ExtraTypes;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private float rotateTime = 1f;

    private Coroutine rotateCoroutine;
    private bool rotateFlag = true;

    public void RotateMainCamera(bool isWhiteTurn)
    {
        if (!rotateFlag)
            return;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateCoroutine(isWhiteTurn));
    }

    private IEnumerator RotateCoroutine(bool isWhiteTurn)
    {
        float targetRotation = isWhiteTurn ? 180 : 0;

        float timeFrac = 0f;

        while (timeFrac < rotateTime)
        {
            timeFrac += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), timeFrac / rotateTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ToggleRotate(bool rotateFlag)
    {
        this.rotateFlag = rotateFlag;
    }
}
