using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
  [SerializeField] Targeter targeter = null;
  [SerializeField] GameObject projectilePrefab;
  [SerializeField] Transform projectileSpawnPoint;
  [SerializeField] float fireRange = 5f;
  [SerializeField] float fireRate = 1f;
  [SerializeField] float rotationSpeed = 20f;

  float lastFireTime;

  [ServerCallback]
  private void Update()
  {
    Targetable target = targeter.Target;

    if (target == null) return;

    if (!CanFireAtTarget()) return;

    Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    if(Time.time > (1 / fireRate) + lastFireTime)
    {
      Quaternion projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projectileSpawnPoint.position);

      GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

      NetworkServer.Spawn(projectileInstance, connectionToClient);

      lastFireTime = Time.time;
    }
  }

  [Server]
  bool CanFireAtTarget()
  {
    return (targeter.Target.transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
  }
}
