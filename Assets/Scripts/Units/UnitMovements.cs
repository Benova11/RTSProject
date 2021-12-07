using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovements : NetworkBehaviour
{
  [SerializeField] NavMeshAgent agent = null;

  private Camera mainCamera;

  #region Server

  [Command]
  void CmdMove(Vector3 position)
  {
    if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

    agent.SetDestination(hit.position);
  }

  #endregion

  #region Client

  public override void OnStartAuthority()
  {
    mainCamera = Camera.main;
  }

  [ClientCallback]
  void Update()
  {
    if (!hasAuthority) { return; }

    if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

    CmdMove(hit.point);
  }

  #endregion

}
