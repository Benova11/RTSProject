using UnityEngine;
using UnityEngine.InputSystem;

public class UnitOperator : MonoBehaviour
{
  private Camera mainCamera;

  [SerializeField] UnitsSelectionManager unitsSelectionManager = null;
  [SerializeField] UnitSelectionHandler selectionHandler = null;
  [SerializeField] LayerMask layerMask = new LayerMask();

  private void Start()
  {
    mainCamera = Camera.main;
  }

  private void Update()
  {
    if(!Mouse.current.rightButton.wasPressedThisFrame) { return; }

    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

    if(hit.collider.TryGetComponent(out Targetable targetable))
    {
      if (targetable.hasAuthority)
      {
        TryMove(hit.point);
        return;
      }

      TryTarget(targetable);
      return;
    }

    TryMove(hit.point);
  }

  private void TryMove(Vector3 movePoint)
  {
    foreach(Unit unit in selectionHandler.SelectedUnits)
    {
      unit.UnitMovements.CmdMove(movePoint);  
    }
  }

  private void TryTarget(Targetable targetable)
  {
    foreach (Unit unit in selectionHandler.SelectedUnits)
    {
      unit.Targeter.CmdSetTarget(targetable.gameObject);
    }
  }

}
