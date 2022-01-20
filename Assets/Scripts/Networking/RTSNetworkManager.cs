using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;

public class RTSNetworkManager : NetworkManager
{ 
  [SerializeField] GameObject unitSpawnerPrefab;

  public static event Action ClientOnConnected;
  public static event Action ClientOnDisconnected;

  bool isGameInProgress = false;

  public List<RTSPlayer> Players { get; } = new List<RTSPlayer>();


  public override void OnServerConnect(NetworkConnection conn)
  {
    if(!isGameInProgress) { return; }
    conn.Disconnect();
  }

  public override void OnServerDisconnect(NetworkConnection conn)
  {
    RTSPlayer player =  conn.identity.GetComponent<RTSPlayer>();

    Players.Remove(player);

    base.OnServerDisconnect(conn);
  }

  public override void OnStopServer()
  {
    Players.Clear();

    isGameInProgress = false;
  }

  public void StartGame()
  {
    if(Players.Count < 2) { return; }

    isGameInProgress = true;

    ServerChangeScene("GameScene");
  }

  public override void OnServerSceneChanged(string sceneName)
  {
    foreach(RTSPlayer player in Players)
    {
      GameObject baseInstance =  Instantiate(unitSpawnerPrefab, GetStartPosition().position, Quaternion.identity);
      NetworkServer.Spawn(baseInstance, player.connectionToClient);
    }
  }

  public override void OnClientConnect(NetworkConnection conn)
  {
    base.OnClientConnect(conn);
    ClientOnConnected?.Invoke();
  }

  public override void OnClientDisconnect(NetworkConnection conn)
  {
    base.OnClientDisconnect(conn);
    ClientOnDisconnected?.Invoke();
  }

  public override void OnStopClient()
  {
    Players.Clear();
  }

  public override void OnServerAddPlayer(NetworkConnection conn)
  {
    base.OnServerAddPlayer(conn);

    //todo uncomment after nisims push
    //Players.Add(player);


    //player.SetPartyOwner(Players.Count == 1);

    //todo to delete
    //GameObject spawnerInstacne = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
    //NetworkServer.Spawn(spawnerInstacne, conn);
  }

}
