using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
  [SerializeField] GameObject landingPagePanel = null;
  [SerializeField] TMP_InputField adressInput = null;
  [SerializeField] Button joinButton = null;

  private void OnEnable()
  {
    RTSNetworkManager.ClientOnConnected += HandleClientConnected;
    RTSNetworkManager.ClientOnConnected += HandleClientDisconnected;
  }

  private void OnDisable()
  {
    RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
    RTSNetworkManager.ClientOnConnected -= HandleClientDisconnected;
  }

  public void Join()
  {
    string address = adressInput.text;

    NetworkManager.singleton.networkAddress = address;
    NetworkManager.singleton.StartClient();

    joinButton.interactable = false;
  }


  void HandleClientConnected()
  {
    joinButton.interactable = true;

    landingPagePanel.SetActive(false);
    gameObject.SetActive(false);
  }

  void HandleClientDisconnected()
  {
    joinButton.interactable = true;
  }
}
