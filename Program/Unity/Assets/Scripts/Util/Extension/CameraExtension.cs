using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtension
{
    private static Coroutine _currentCoroutine = null;
    private static Vector3 _originPosition = Vector3.zero;

    // 카메라 x축으로 흔듬
    public static void ShakeX(this Camera cam, float duration, float amount = 0.3f, float delay = 0f)
    {
        var mono = cam.GetComponent<MonoBehaviour>( );

        if (_currentCoroutine != null)
        {
            mono.StopCoroutine(_currentCoroutine);
            cam.transform.localPosition = _originPosition;
        }

        _currentCoroutine = mono.StartCoroutine(_Shake(cam, delay, duration, amount, new Vector3(1f, 0f, 0f)));
    }

    // 카메라 y축으로 흔듬
    public static void ShakeY(this Camera cam, float duration, float amount = 0.3f, float delay = 0f)
    {
        var mono = cam.GetComponent<MonoBehaviour>( );

        if (_currentCoroutine != null)
        {
            mono.StopCoroutine(_currentCoroutine);
            cam.transform.localPosition = _originPosition;
        }

        _currentCoroutine = mono.StartCoroutine(_Shake(cam, delay, duration, amount, new Vector3(0f, 1f, 0f)));
    }

    // 카메라 흔듬
    public static void Shake(this Camera cam, float duration, float amount = 0.3f, float delay = 0f)
    {
        var mono = cam.GetComponent<MonoBehaviour>( );

        if (_currentCoroutine != null)
        {
            mono.StopCoroutine(_currentCoroutine);
            cam.transform.localPosition = _originPosition;
        }

        _currentCoroutine = mono.StartCoroutine(_Shake(cam, delay, duration, amount, new Vector3(1f, 1f, 1f)));
    }

    // 카메라 흔듬
    private static IEnumerator _Shake(Camera cam, float delay, float duration, float amount, Vector3 shakePos)
    {
        Transform camTrans = cam.transform;

        _originPosition = camTrans.localPosition;

        while (duration > 0)
        {
            Vector3 rand = UnityEngine.Random.insideUnitSphere;
            rand = new Vector3(rand.x * shakePos.x, rand.y * shakePos.y, rand.z * shakePos.z);

            camTrans.localPosition = _originPosition + rand * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        camTrans.localPosition = _originPosition;

        _currentCoroutine = null;
    }

    // 카메라 흔듬 중지
    public static void StopShake(this Camera cam)
    {
        var mono = cam.GetComponent<MonoBehaviour>( );

        if (_currentCoroutine != null)
        {
            mono.StopCoroutine(_currentCoroutine);
            cam.transform.localPosition = _originPosition;

            _currentCoroutine = null;
        }
    }
}