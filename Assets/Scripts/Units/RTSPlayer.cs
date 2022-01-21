using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private LayerMask buildingBlockLayer = new LayerMask();
    [SerializeField] private Building[] buildings = new Building[0];
    [SerializeField] private float buildingRangeLimit = 5f;

    [SyncVar(hook = nameof(ClientHandleResourcesUpdated))]
    private int resources = 500;
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;
    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string displayName;

    public event Action<int> ClientOnResourcesUpdated;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    private Color teamColor = new Color();
    private List<Unit> myUnits = new List<Unit>();
    private List<Building> myBuildings = new List<Building>();


    public bool CanPlaceBuilding(BoxCollider buildingCollider, Vector3 point)
    {
        if (Physics.CheckBox(
                    point + buildingCollider.center,
                    buildingCollider.size / 2,
                    Quaternion.identity,
                    buildingBlockLayer))
        {
            return false;
        }

        foreach (Building building in myBuildings)
        {
            if ((point - building.transform.position).sqrMagnitude
                <= buildingRangeLimit * buildingRangeLimit)
            {
                return true;
            }
        }

        return false;
    }
    public int GetResources()
    {
        return resources;
    }
    public List<Unit> GetMyUnits()
  {
    return myUnits;
  }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
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
    [Server]
    public void SetResources(int newResources)
    {
        resources = newResources;
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    [Server]
    public void SetTeamColor(Color newTeamColor)
    {
        teamColor = newTeamColor;
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

    private void ClientHandleResourcesUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources);
    }

    public override void OnStartClient()
  {
    if(NetworkServer.active) { return; }

    ((RTSNetworkManager)NetworkManager.singleton).Players.Add(this);

    if (!isClientOnly) { return; }

    Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
    Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
  }
    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
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
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        myUnits.Remove(unit);
    }

    private void AuthorityHandleBuildingSpawned(Building building)
    {
        myBuildings.Add(building);
    }

    private void AuthorityHandleBuildingDespawned(Building building)
    {
        myBuildings.Remove(building);
    }

    internal Color GetTeamColor()
    {
        return teamColor; return teamColor;
    }

    #endregion
}






