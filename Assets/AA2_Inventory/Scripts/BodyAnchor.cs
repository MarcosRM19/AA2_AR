using UnityEngine;

namespace AA2Inventory_G4
{
    public class BodyAnchor : MonoBehaviour
    {
        [SerializeField] private Transform headTransform;

        [SerializeField] private float hipOffset = -0.55f;
        [SerializeField] private float forwardLean = 0.05f;

        void LateUpdate()
        {
            if (headTransform == null)
                return;

            Quaternion targetRot = Quaternion.Euler(0f, headTransform.eulerAngles.y, 0f);
            transform.rotation = targetRot;

            Vector3 targetPos = new Vector3(
                headTransform.position.x,
                headTransform.position.y + hipOffset,
                headTransform.position.z
            );
            targetPos += targetRot * new Vector3(0f, 0f, forwardLean);

            transform.position = targetPos;
        }
    }
}
