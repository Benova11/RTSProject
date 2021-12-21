using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
  [SerializeField] UnitMovements unitMovements = null;
  [SerializeField] UnityEvent onSelected = null;
  [SerializeField] UnityEvent onDeselected = null;

  public UnitMovements UnitMovements { get { return unitMovements; } }

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

}
