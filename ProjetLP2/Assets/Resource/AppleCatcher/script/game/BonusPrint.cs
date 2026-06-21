using TMPro;
using UnityEngine;
using System.Collections;

public class BonusPrint : MonoBehaviour
{
    public static BonusPrint Instance;
    public TextMeshPro effectText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        effectText.SetText("");
    }

    public void ShowEffect(string label, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(Display(label, duration));
    }

    private IEnumerator Display(string label, float duration)
    {
        float remaining = duration;
        while (remaining > 0f)
        {
            effectText.SetText($"{label} {remaining:F1}s");
            remaining -= Time.deltaTime;
            yield return null;
        }
        effectText.SetText("");
    }
}