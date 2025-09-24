using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;       // Player
  public float smoothSpeed = 5f; // tốc độ mượt
  public Vector3 offset;         // lệch so với player

  void LateUpdate()
  {
    if (target == null) return;

    Vector3 desiredPosition = target.position + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    transform.position = smoothedPosition;
  }
}
