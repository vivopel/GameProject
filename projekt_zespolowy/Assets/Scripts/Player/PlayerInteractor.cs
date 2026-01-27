using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1.2f;
    [SerializeField] private LayerMask interactableMask;

    private InputAction interactAction;

    private void Awake()
    {
        var input = GetComponent<PlayerInput>();
        interactAction = input.actions["Interact"];
    }

    private void OnEnable()
    {
        interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        var hit = Physics2D.OverlapCircle(transform.position, interactRadius, interactableMask);
        if (hit == null) return;

        var i = hit.GetComponent<IInteractable>();
        if (i != null) i.Interact(this);
    }
}
