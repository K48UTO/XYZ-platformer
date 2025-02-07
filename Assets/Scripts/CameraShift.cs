using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public Transform[] targets;
    public float transitionSpeed = 1f; 
    public float stayDuration = 2f; 
    private Transform originalFollow;
    private Transform intermediateTarget;

    private void Awake()
    {
        originalFollow = cinemachineCamera.Follow;
        intermediateTarget = new GameObject("IntermediateTarget").transform;
    }

    public void ActivateShift()
    {
        cinemachineCamera.Follow = intermediateTarget;
        StartCoroutine(ShiftCameraSequence());
    }

    private IEnumerator ShiftCameraSequence()
    {
        foreach (Transform target in targets)
        {
            yield return StartCoroutine(MoveToTarget(target));
        }

        yield return StartCoroutine(MoveToTarget(originalFollow));
        cinemachineCamera.Follow = originalFollow;
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        while (Vector3.Distance(intermediateTarget.position, target.position) > 0.4f)
        {
            intermediateTarget.position = Vector3.Lerp(intermediateTarget.position, target.position, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        cinemachineCamera.Follow = target;

        yield return new WaitForSeconds(stayDuration);
     }
    
}
