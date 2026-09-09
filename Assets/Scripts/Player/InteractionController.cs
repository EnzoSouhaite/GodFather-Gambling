using Unity.VisualScripting;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    private IInteractController interactController;

    private void OnEnable()
    {
        interactController = interactController ?? GetComponent<IInteractController>();

        if (interactController == null) return;
        
        interactController.onInteract += OnInteract;
    }

    private void OnDisable()
    {
        if (interactController == null) return;

        interactController.onInteract -= OnInteract;
    }

    private void OnInteract()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, _camera.transform.forward, out hit);
        if (hit.collider == null) return;

        ITouchableOnDown touchable = hit.collider.GetComponent<ITouchableOnDown>();
        if (touchable == null) return;

        touchable.OnTouchedDown();
    }
}