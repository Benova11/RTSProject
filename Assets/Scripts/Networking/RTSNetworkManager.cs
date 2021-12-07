using UnityEngine;
using Mirror;

public class RTSNetworkManager : NetworkManager
{ 
  [SerializeField] GameObject unitSpawnerPrefab;

  public override void OnServerAddPlayer(NetworkConnection conn)
  {
    base.OnServerAddPlayer(conn);

    GameObject spawnerInstacne = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
    NetworkServer.Spawn(spawnerInstacne, conn);
  }

}
