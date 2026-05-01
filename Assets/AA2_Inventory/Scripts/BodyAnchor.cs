using UnityEngine;

public class BodyAnchor : MonoBehaviour
{
    [SerializeField] private Transform headTransform;

    [SerializeField] private float hipOffset = -0.55f;
    [SerializeField] private float forwardLean = 0.05f;
    [SerializeField] private float smoothSpeed = 12f;

    void LateUpdate()
    {
        if (headTransform == null) return;

        Vector3 targetPos = new Vector3(
            headTransform.position.x,
            headTransform.position.y + hipOffset,
            headTransform.position.z
        );

        float headYaw = headTransform.eulerAngles.y;
        Quaternion targetRot = Quaternion.Euler(0f, headYaw, 0f);
        targetPos += targetRot * new Vector3(0f, 0f, forwardLean);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}
