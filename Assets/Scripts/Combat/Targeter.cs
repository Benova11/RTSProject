
using UnityEngine;
using Mirror;
using System;

public class Targeter : NetworkBehaviour
{
  Targetable target;

  public Targetable Target { get { return target; } }

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

}
