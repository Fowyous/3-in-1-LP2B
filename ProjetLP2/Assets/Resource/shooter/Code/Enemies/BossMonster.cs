using UnityEngine;
using System.Collections;

public class BossMonster : MonoBehaviour, IEnemy
{
  [Header("Boss Stats")]
  [SerializeField] private float health = 15f;
  [SerializeField] private float damage = 5f;
  [SerializeField] private float speed = 1.5f;

  [Header("Boss Arsenal")]
  [SerializeField] private GameObject electricPrefab;
  [SerializeField] private GameObject flamePrefab;
  [SerializeField] private GameObject laserPrefab;
  [SerializeField] private GameObject sniperPrefab;

  [Header("Weapon Setup")]
  [SerializeField] private Transform firePoint;

  public float Health { get => health; set => health = value; }
  public float Damage { get => damage; set => damage = value; }
  public bool IsAlive { get; private set; } = true;
  public GameObject Target { get; set; }

  private Rigidbody2D rb;
  private int currentAttackPhase = 0;
  private LaserChargeEffect chargeEffect;

  // Reference to the currently active laser, so it can be destroyed if the Boss dies mid-shot
  private GameObject activeLaser;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    chargeEffect = GetComponent<LaserChargeEffect>();
    StartCoroutine(BossAttackCycle());
  }

  void Update()
  {
    NextMove(Target);
  }

  public void TakeDamage(float damageAmount)
  {
    if (!IsAlive) return;
    Health -= damageAmount;
    if (Health <= 0)
    {
      IsAlive = false;
      if (chargeEffect != null) chargeEffect.StopChargeEffect();
      DestroyActiveLaser();
      Destroy(gameObject);
    }
  }

  public void Shoot(GameObject bullet)
  {
    if (bullet == null || firePoint == null) return;

    // For lasers, lower the spawn position by 20 units to balance the offset of the custom pivot of the lazer
    Vector3 spawnPos = firePoint.position;
    if (bullet.GetComponent<HorizontalLaser>() != null)
      spawnPos += Vector3.down * -20.0f;

    GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);

    // FlameCone needs a target reference to compute its lifetime correctly
    FlameCone flameCone = projectile.GetComponent<FlameCone>();
    if (flameCone != null && Target != null)
      flameCone.SetTarget(Target);

    // HorizontalLaser needs to follow the Boss's firePoint, since the Boss keeps hovering
    HorizontalLaser laserScript = projectile.GetComponent<HorizontalLaser>();
    if (laserScript != null)
    {
      laserScript.SetSource(firePoint);
      activeLaser = projectile; // Keep a reference so it can be cleaned up if the Boss dies
    }
  }

  ///<summary>
  ///Destroys the currently active laser, if any. Called when the Boss dies.
  ///</summary>
  private void DestroyActiveLaser()
  {
    if (activeLaser != null)
    {
      Destroy(activeLaser);
      activeLaser = null;
    }
  }

  public void NextMove(GameObject target)
  {
    if (rb == null || !IsAlive) return;
    if (transform.position.x > 4.0f)
      rb.linearVelocity = Vector3.left * speed;
    else
    {
      float hoverY = Mathf.Sin(Time.time * 2f) * speed;
      rb.linearVelocity = new Vector2(0, hoverY);
    }
  }

  private IEnumerator BossAttackCycle()
  {
    yield return new WaitForSeconds(2f);

    while (IsAlive)
    {
      currentAttackPhase = Random.Range(0, 4);

      switch (currentAttackPhase)
      {
        case 0:
          Shoot(electricPrefab);
          break;

        case 1:
          for (int i = 0; i < 2; i++)
          {
            Shoot(flamePrefab);
            yield return new WaitForSeconds(0.3f);
          }
          break;

        case 2:
          if (chargeEffect != null) chargeEffect.PlayChargeEffect();
          yield return new WaitForSeconds(1.0f);
          Shoot(laserPrefab);
          break;

        case 3:
          for (int i = 0; i < 4; i++)
          {
            Shoot(sniperPrefab);
            yield return new WaitForSeconds(0.12f);
          }
          break;
      }

      yield return new WaitForSeconds(4f);
    }
  }

  ///<summary>
  ///Handles collision with the UFO or the base — even the Boss can crash into them
  ///if it ever drifts far enough left (e.g. pushed back, or future behavior change).
  ///</summary>
  private void OnTriggerEnter2D(Collider2D collision)
  {
    UFO player = collision.GetComponent<UFO>();
    if (player != null)
    {
      player.TakeDamage(Damage);
      return;
    }

    if (collision.CompareTag("Base"))
    {
      BaseManager baseScript = collision.GetComponent<BaseManager>();
      if (baseScript != null)
        baseScript.TakeDamage(Damage);
    }
  }
}
