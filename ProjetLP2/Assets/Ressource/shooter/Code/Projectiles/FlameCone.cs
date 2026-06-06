using UnityEngine;

public class FlameCone : MonoBehaviour
{
    [Header("Flame Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float directDamage = 1f;      // 1 damage on hit 
    [SerializeField] private float burnDamagePerSec = 0.5f; // 0.5 damage per second 
    [SerializeField] private float burnDuration = 4f;       // Lasts for 4 seconds 
    [SerializeField] private float lifeTime = 1.2f;         // Short distance/lifetime [cite: 105, 131]
    [SerializeField] private float expansionSpeed = 1.5f;   // How fast the flame widens

    ///<summary>
    ///Sets up automatic destruction for short-range flame effect.
    ///</summary>
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    ///<summary>
    ///Moves the flame forward and expands its size to mimic a cone shape.
    ///</summary>
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
        // Expand the Y scale over time to create a cone/beam visual effect
        transform.localScale += new Vector3(0, expansionSpeed * Time.deltaTime, 0);
    }

    ///<summary>
    ///Applies initial damage and triggers the continuous burn effect on the player.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(directDamage);
            
            // TODO: Trigger a Burn Coroutine on the player script for continuous damage
            Debug.Log($"UFO hit by FlameCone! {directDamage} direct damage + Burn effect applied ({burnDamagePerSec}/s for {burnDuration}s).");
            
            Destroy(gameObject);
        }
    }
}
