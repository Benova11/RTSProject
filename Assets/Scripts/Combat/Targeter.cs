
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
  [SerializeField] Targetable target;

  #region Server

  [Command]
  public void CmdSetTarget(GameObject targetGameObject)
  {
    if (!targetGameObject.TryGetComponent(out Targetable newTarget)) { return; }

    target = newTarget;
  }

  [Server]
  public void ClearTaraget()
  {
    target = null;
  }

  #endregion



  #region Client



  #endregion
}
