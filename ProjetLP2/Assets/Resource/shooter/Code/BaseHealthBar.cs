using UnityEngine;
using UnityEngine.UI;

///<summary>
///Displays the Base's health as a classic UI fill bar.
///Updates the Image's fillAmount whenever BaseManager's health changes
///(damage or passive regeneration).
///</summary>
public class BaseHealthBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Leave empty to auto-find the BaseManager in the scene.")]
    [SerializeField] private BaseManager baseManager;

    [Tooltip("The Image component with Image Type = Filled.")]
    [SerializeField] private Image fillImage;

    [Header("Optional Color Feedback")]
    [SerializeField] private bool useColorGradient = true;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    void Start()
    {
        if (baseManager == null)
        {
            baseManager = Object.FindAnyObjectByType<BaseManager>();
            if (baseManager == null)
            {
                Debug.LogError("BaseHealthBar: No BaseManager found in the scene!");
                return;
            }
        }

        if (fillImage == null)
        {
            Debug.LogError("BaseHealthBar: fillImage is not assigned!");
            return;
        }

        baseManager.OnHealthChanged += UpdateHealthBar;

        // Initialize the bar with the current health value
        UpdateHealthBar(baseManager.CurrentHealth, baseManager.MaxHealth);
    }

    void OnDestroy()
    {
        if (baseManager != null)
            baseManager.OnHealthChanged -= UpdateHealthBar;
    }

    ///<summary>
    ///Updates the fill amount (0 to 1) and optionally the color based on health ratio.
    ///Called automatically whenever BaseManager.OnHealthChanged fires.
    ///</summary>
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float ratio = maxHealth > 0 ? currentHealth / maxHealth : 0f;
        fillImage.fillAmount = ratio;

        if (useColorGradient)
        {
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, ratio);
        }
    }
}