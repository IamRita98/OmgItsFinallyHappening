using UnityEngine;
using UnityEngine.InputSystem;

public class RebindActions : MonoBehaviour
{
  PlayerInput playerInput;
  public void RemapActionClicked(InputAction actionToRebind)
  {
    actionToRebind.Disable();
    var rebindOperation = actionToRebind.PerformInteractiveRebinding().WithControlsExcluding("Mouse")
      .OnMatchWaitForAnother(0.1f).OnComplete(operation =>
      {
        Debug.Log($"Rebound '{actionToRebind.name}' to {operation.selectedControl}\n Rebind operation completed!");
        operation.Dispose();
        actionToRebind.Enable();
        UIManager.Instance.ClearUI();
        GameStateManager.Instance.state = State.Combat;
        Messinground();
      });
    rebindOperation.Start();
    
  }

  private void Messinground()
  {
    playerInput = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<PlayerInput>();
    var arrActions= playerInput.currentActionMap.actions;
    int x = 0;
    foreach (var action in arrActions)
    {
      Debug.Log($"Action {x++}:{action.name}");
    }
  }
}
