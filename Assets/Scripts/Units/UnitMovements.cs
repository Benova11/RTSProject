using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class UnitMovements : NetworkBehaviour
{
  [SerializeField] NavMeshAgent agent = null;
  [SerializeField] Targeter targeter;
  [SerializeField] float chaseRange = 10f;

  #region Server

  [ServerCallback]
  private void Update()
  {
    Targetable target = targeter.Target;

    if (target != null)
    {
      if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
      {
        agent.SetDestination(target.transform.position);
      }
      else if (agent.hasPath)
      {
        agent.ResetPath();
      }

      return;
    }

    if (!agent.hasPath) { return; }

    if (agent.remainingDistance > agent.stoppingDistance) { return; }

    agent.ResetPath();
  }
    public void ServerMove(Vector3 position)
    {
        targeter.ClearTaraget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
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
