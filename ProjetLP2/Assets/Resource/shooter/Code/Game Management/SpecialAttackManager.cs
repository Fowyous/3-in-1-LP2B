using UnityEngine;

///<summary>
///Singleton responsible for executing the player's special attack:
///destroys every enemy currently on screen at once.
///Visual/audio explosion feedback (animation + sound) is handled separately
///by each enemy's own death logic (DeathExplosion, PowerUpDropper, etc.),
///since destroying the GameObject still triggers those normally.
///</summary>
public class SpecialAttackManager : MonoBehaviour
{
  public static SpecialAttackManager Instance { get; private set; }

  [SerializeField] Canvas explosionCanvas;
  [SerializeField] private CircleExpander circleExpander;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Debug.LogWarning("Multiple SpecialAttackManager instances detected!");
      Destroy(gameObject);
    }
  }

    ///<summary>
    ///Finds every active enemy in the scene (anything implementing IEnemy,
    ///including the Boss) and destroys them immediately.
    ///Each enemy's own TakeDamage/death logic is bypassed here since we go
    ///straight to destruction, but their OnDestroy-based effects (explosions)
    ///still fire normally.
    ///</summary>
    [System.Obsolete]
    public void DetonateAllEnemies()
  {
    // FindObjectsByType works on any MonoBehaviour; IEnemy is an interface,
    // so we look for MonoBehaviour components implementing it via a base search.
    MonoBehaviour[] allBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

    explosionCanvas.gameObject.SetActive(true);
    //explosionCanvas.GetComponent<CircleExplosion>().PlayExplosion();
    circleExpander.ActivateCircle();
    explosionCanvas.gameObject.SetActive(false);

    int destroyedCount = 0;
    foreach (MonoBehaviour behaviour in allBehaviours)
    {
      if (behaviour is IEnemy enemy && behaviour.gameObject != null)
      {
        Destroy(behaviour.gameObject);
        destroyedCount++;
      }
    }

    Debug.Log($"SpecialAttackManager: Special attack triggered, {destroyedCount} enemies destroyed.");
  }
}
