using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public Transform[] targets;
    public float transitionSpeed = 1f; // Скорость перехода к цели
    public float stayDuration = 2f; // Время пребывания на цели
    private Transform originalFollow;
    private Transform intermediateTarget;

    private void Awake()
    {
        originalFollow = cinemachineCamera.Follow;
        // Создаем промежуточный объект для плавного перехода, но не назначаем его сразу
        intermediateTarget = new GameObject("IntermediateTarget").transform;
    }

    public void ActivateShift()
    {
        // Переключаем объект следования камеры на промежуточный только при активации перехода
        cinemachineCamera.Follow = intermediateTarget;
        StartCoroutine(ShiftCameraSequence());
    }

    private IEnumerator ShiftCameraSequence()
    {
        foreach (Transform target in targets)
        {
            yield return StartCoroutine(MoveToTarget(target));

            // Ждем на цели
            
        }

        // Возвращаем камеру к исходной цели
        yield return StartCoroutine(MoveToTarget(originalFollow));
        cinemachineCamera.Follow = originalFollow; // Восстанавливаем первоначальное следование за игроком
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        // Перемещаем промежуточный объект к текущему положению цели 
        while (Vector3.Distance(intermediateTarget.position, target.position) > 0.4f)
        {
            intermediateTarget.position = Vector3.Lerp(intermediateTarget.position, target.position, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        // Как только промежуточный объект достигает цели, начинаем следование за целью
        cinemachineCamera.Follow = target;

        // Даем камере время на следование за целью
        yield return new WaitForSeconds(stayDuration);
     }
    
}
