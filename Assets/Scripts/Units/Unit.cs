using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;
using System.Collections;

public class Unit : NetworkBehaviour
{
  [SerializeField] UnitMovements unitMovements = null;
  [SerializeField] UnityEvent onSelected = null;
  [SerializeField] UnityEvent onDeselected = null;


    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public UnitMovements UnitMovements { get { return unitMovements; } }


    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }

        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }

        AuthorityOnUnitDespawned?.Invoke(this);
    }


    #region Client

    [Client]
  public void Select()
  {
    if (hasAuthority)
    {
      onSelected?.Invoke();
    }
  }

  [Client]
  public void Deselect()
  {
    if (hasAuthority)
    {
      onDeselected?.Invoke();
    }
  }

    #endregion
    [Client]
    public void select()
    {
        if (!hasAuthority) { return; }

        onSelected?.Invoke();
    }

    [Client]
    public void deselect()
    {
        if (!hasAuthority) { return; }

        onDeselected?.Invoke();
    }

    #endregion

}


