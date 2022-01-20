using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
  [SerializeField] GameObject lobbyUI = null;
  [SerializeField] Button startGameButton;

  private void Start()
  {
    RTSNetworkManager.ClientOnConnected += HandleClientConnected;
    RTSPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
  }

  private void OnDestroy()
  {
    RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
    RTSPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
  }

  void AuthorityHandlePartyOwnerStateUpdated(bool state)
  {
    startGameButton.gameObject.SetActive(state);
  }

  void HandleClientConnected()
  {
    lobbyUI.SetActive(true);
  }

  public void StartGame()
  {
    NetworkClient.connection.identity.GetComponent<RTSPlayer>().CmdStartGame();
  }

  public void LeaveLobby()
  {
    if (NetworkServer.active && NetworkClient.isConnected)
      NetworkManager.singleton.StopHost();
    else
    {
      NetworkManager.singleton.StopClient();
      SceneManager.LoadScene(0);
    }
  }
}
