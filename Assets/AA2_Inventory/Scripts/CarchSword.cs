using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CarchSword : XRGrabInteractable
{
    [Header("Ajustes de Vibraci�n")]
    public float intensidad = 0.5f;
    public float duracion = 0.1f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Esto hace que el mando vibre por c�digo al agarrar
        if (args.interactorObject is XRBaseInputInteractor controllerInteractor)
        {
            controllerInteractor.xrController.SendHapticImpulse(intensidad, duracion);
        }

        Debug.Log("Katana agarrada y mando vibrando.");
    }
}
