using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float followRange;

    private CinemachineVirtualCamera cineCam;

    private void Start()
    {
        cineCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        if (target != null && IsPlayerInRange())
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        }
    }

    private bool IsPlayerInRange()
    {
        if (cineCam != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= followRange;
        }
        return false;
    }
}
