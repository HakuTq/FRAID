using System.Collections.Generic;
using System.Linq;
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
        if(Keyboard.current.kKey.wasPressedThisFrame)
        {
            RebindAction(walkRAction, "rightArrow");
            RebindAction(walkLAction, "leftArrow");
            RebindAction(jumpAction, "upArrow");
        }
    }


    private void RebindAction(InputActionReference actionReference, string inputCode)
    {
        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        // Remove previous binding for the action
        actionReference.action.RemoveAllBindingOverrides();

        // Add new binding
        actionReference.action.ApplyBindingOverride("<Keyboard>/" + inputCode);

        playerController.PlayerInput.SwitchCurrentActionMap("Movement");
    }
}
