using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
  [SerializeField] private List<Unit> myUnits = new List<Unit>();

  [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
  bool isPartyOwner = false;

  public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

  public List<Unit> GetMyUnits()
  {
    return myUnits;
  }

  public bool GetIsPartyOwner()
  {
    return isPartyOwner;
  }
  #region Server

  public override void OnStartServer()
  {
    Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
    Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
  }

  public override void OnStopServer()
  {
    Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
    Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
  }

  [Server]
  public void SetPartyOwner(bool state)
  {
    isPartyOwner = state;
  }

  private void ServerHandleUnitSpawned(Unit unit)
  {
    if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

    myUnits.Add(unit);
  }

  private void ServerHandleUnitDespawned(Unit unit)
  {
    if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

    myUnits.Remove(unit);
  }

  #endregion

  #region Client

  public override void OnStartClient()
  {
    if(NetworkServer.active) { return; }

    ((RTSNetworkManager)NetworkManager.singleton).Players.Add(this);

    if (!isClientOnly) { return; }

    Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
    Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
  }

  public void CmdStartGame()
  {
    if(!isPartyOwner) { return; }

    ((RTSNetworkManager)NetworkManager.singleton).StartGame();
  }

  public override void OnStopClient()
  {
    if (!isClientOnly) { return; }

    ((RTSNetworkManager)NetworkManager.singleton).Players.Remove(this);

    if(!hasAuthority) { return; }

    Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
    Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
  }

  void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
  {
    if(!hasAuthority) { return; }

    AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
  }

  private void AuthorityHandleUnitSpawned(Unit unit)
  {
    if (!hasAuthority) { return; }

    myUnits.Add(unit);
  }

  private void AuthorityHandleUnitDespawned(Unit unit)
  {
    if (!hasAuthority) { return; }

    myUnits.Remove(unit);
  }

  #endregion
}





