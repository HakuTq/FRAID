using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRebind : MonoBehaviour
{
    [Header("References")]
    // Reference to the PlayerMovementScript
    [SerializeField] private PlayerMovementScript playerController = null;

    [Header("Actions")]
    // References to the InputActions for jumping and walking left/right
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private InputActionReference walkLAction = null;
    [SerializeField] private InputActionReference walkRAction = null;

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            RebindAction(walkRAction, "rightArrow");
            RebindAction(walkLAction, "leftArrow");
            RebindAction(jumpAction, "upArrow");
        }
    }

    /// <summary>
    /// Rebinds an action to a new input.
    /// </summary>
    /// <param name="actionReference">The action to rebind (jump, walkL, walkR).</param>
    /// <param name="inputCode">The new key code to bind the action to (e.g., "W", "space", "upArrow").</param>
    public void RebindAction(InputActionReference actionReference, string inputCode)
    {
        // Switch to the "Menu" action map to prevent any conflicts during rebinding
        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        // Remove previous binding for the specified action
        actionReference.action.RemoveAllBindingOverrides();

        // Add new binding for the specified action
        actionReference.action.ApplyBindingOverride("<Keyboard>/" + inputCode);

        //Saves and changes the Map
        End();
    }
    private void End()
    {
        // Switch back to the "Movement" action map after rebinding
        playerController.PlayerInput.SwitchCurrentActionMap("Movement");

        //Here will be the code for saving the inputs
    }
}
