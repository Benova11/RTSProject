using UnityEngine;
using Mirror;

public class Targetable : NetworkBehaviour
{
  [SerializeField] Transform aimAtPoint = null;

  public Transform GetAimAtPoint()
  {
    return aimAtPoint;
  }
}
