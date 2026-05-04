using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace AA2Inventory_G4
{
    public class CatchWeapon : XRGrabInteractable
    {
        [Header("Vibración")]
        [SerializeField] private float intensidad = 0.5f;
        [SerializeField] private float duracion = 0.1f;

        [Header("Holster")]
        [SerializeField] private float holsterSnapRadius = 0.15f;
        [SerializeField] private LayerMask holsterLayer;

        [SerializeField] private Vector3 holsterRotationOffset = Vector3.zero;

        private Rigidbody rb;
        private HolsterSlot currentSlot;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (currentSlot != null)
            {
                currentSlot.SetHolsterBeingUsed(false);
                currentSlot = null;
                transform.SetParent(null);
            }

            base.OnSelectEntered(args);

            if (args.interactorObject is XRBaseInteractor interactor)
            {
                Transform interactorAttach = interactor.GetAttachTransform(this);
                Vector3 posOffset = transform.position - attachTransform.position;
                Quaternion rotOffset = Quaternion.Inverse(attachTransform.rotation) * transform.rotation;
                transform.rotation = interactorAttach.rotation * rotOffset;
                transform.position = interactorAttach.position + transform.rotation * (Quaternion.Inverse(rotOffset) * posOffset);
            }

            if (args.interactorObject is XRBaseInputInteractor ctrl)
                ctrl.xrController.SendHapticImpulse(intensidad, duracion);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            HolsterSlot nearest = FindNearestHolster();

            if (nearest != null)
                SnapToHolster(nearest);
            else
                FreezeInAir();
        }

        private HolsterSlot FindNearestHolster()
        {
            Vector3 origin = attachTransform != null ? attachTransform.position : transform.position;
            Collider[] hits = Physics.OverlapSphere(origin, holsterSnapRadius, holsterLayer);

            HolsterSlot nearest = null;
            float minDist = float.MaxValue;

            foreach (var hit in hits)
            {
                var slot = hit.GetComponent<HolsterSlot>();
                if (slot == null || slot.GetHolsterIsBeingUsed())
                    continue;

                float d = Vector3.Distance(origin, slot.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = slot;
                }
            }
            return nearest;
        }

        private void SnapToHolster(HolsterSlot slot)
        {
            rb.isKinematic = true;
            rb.useGravity = false;

            transform.SetParent(null);

            Vector3 swordUp = slot.GetWeapomUpAxis();
            Vector3 swordRight = slot.GetWeapomRightAxis();
            Vector3 swordForward = Vector3.Cross(swordRight, swordUp).normalized;
            Quaternion baseRot = Quaternion.LookRotation(swordForward, swordUp);
            Quaternion offset = Quaternion.Euler(holsterRotationOffset);
            transform.rotation = baseRot * offset;

            if (attachTransform != null)
            {
                transform.position = slot.transform.position - (attachTransform.position - transform.position);
            }
            else
            {
                transform.position = slot.transform.position;
            }

            transform.SetParent(slot.transform);

            currentSlot = slot;
            slot.SetHolsterBeingUsed(true);
        }

        private void FreezeInAir()
        {
            transform.SetParent(null);
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void ReleaseFromHolster()
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            currentSlot = null;
        }
    }
}
