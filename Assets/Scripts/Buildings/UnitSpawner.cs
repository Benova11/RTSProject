using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour,IPointerClickHandler
{
  [SerializeField] GameObject unitPrefab;
  [SerializeField] Transform unitSpawnPoint;

  #region Server

  [Command]
  void CmdSpawnUnit()
  {
    GameObject unitInstance = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);

    NetworkServer.Spawn(unitInstance, connectionToClient);
  }

  #endregion

  #region Client

  [ClientCallback]
  public void OnPointerClick(PointerEventData eventData)
  {
    Debug.Log("dsfsd");
    if (eventData.button == PointerEventData.InputButton.Right && hasAuthority)
      CmdSpawnUnit();
  }

  #endregion
}
