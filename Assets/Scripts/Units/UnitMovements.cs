using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class UnitMovements : NetworkBehaviour
{
  [SerializeField] NavMeshAgent agent = null;


  #region Server

  [Command]
  public void CmdMove(Vector3 position)
  {
    if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

    agent.SetDestination(hit.position);
  }

  #endregion

}
