using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRebind : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovementScript playerController = null;
    [Header("Actions")]
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private InputActionReference walkLAction = null;
    [SerializeField] private InputActionReference walkRAction = null;

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            RebindJump("upArrow");
            RebindWalkL("leftArrow");
            RebindWalkR("rightArrow");
        }
    }

    public void RebindJump(string inputCode)
    {
        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        // Remove previous binding for jump action
        jumpAction.action.RemoveAllBindingOverrides();

        // Add new binding
        jumpAction.action.ApplyBindingOverride("<Keyboard>/" + inputCode);

        playerController.PlayerInput.SwitchCurrentActionMap("Movement");
    }
    public void RebindWalkL(string inputCode)
    {
        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        // Remove previous binding for jump action
        walkLAction.action.RemoveAllBindingOverrides();

        // Add new binding
        walkLAction.action.ApplyBindingOverride("<Keyboard>/" + inputCode);

        playerController.PlayerInput.SwitchCurrentActionMap("Movement");
    }
    public void RebindWalkR(string inputCode)
    {
        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        // Remove previous binding for jump action
        walkRAction.action.RemoveAllBindingOverrides();

        // Add new binding
        walkRAction.action.ApplyBindingOverride("<Keyboard>/" + inputCode);

        playerController.PlayerInput.SwitchCurrentActionMap("Movement");
    }
}
