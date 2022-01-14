using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class UnitMovements : NetworkBehaviour
{
  [SerializeField] NavMeshAgent agent = null;
  [SerializeField] Targeter targeter;

  #region Server

  [ServerCallback]
  private void Update()
  {
    if (!agent.hasPath) { return; }
    if (agent.remainingDistance > agent.stoppingDistance) { return; }

    agent.ResetPath();
  }

  [Command]
  public void CmdMove(Vector3 position)
  {
    targeter.ClearTaraget();

    if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

    agent.SetDestination(hit.position);
  }

  #endregion

}
