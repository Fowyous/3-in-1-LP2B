using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance { get; private set; }

    [SerializeField] private TextMeshPro bonusText;

    private class ActiveBonus
    {
        public string Name;
        public float TimeRemaining;
        public bool IsInstant;

        public ActiveBonus(string name, float duration, bool isInstant = false)
        {
            Name          = name;
            TimeRemaining = duration;
            IsInstant     = isInstant;
        }
    }

    private readonly List<ActiveBonus> activeBonuses = new();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        // Décrémenter les timers et supprimer les expirés
        for (int i = activeBonuses.Count - 1; i >= 0; i--)
        {
            activeBonuses[i].TimeRemaining -= Time.deltaTime;
            if (activeBonuses[i].TimeRemaining <= 0f)
                activeBonuses.RemoveAt(i);
        }
        RefreshUI();
    }

    // Pour les bonus/malus avec une durée
    public void Register(string name, float duration)
    {
        // Si le bonus est déjà actif, on refresh sa durée
        var existing = activeBonuses.Find(b => b.Name == name);
        if (existing != null)
            existing.TimeRemaining = duration;
        else
            activeBonuses.Add(new ActiveBonus(name, duration));
    }
    
    private void RefreshUI()
    {
        if (bonusText == null) return;

        if (activeBonuses.Count == 0)
        {
            bonusText.text = string.Empty;
            return;
        }

        var sb = new System.Text.StringBuilder();
        foreach (var b in activeBonuses)
        {
            // Les instantanés n'affichent pas de timer
            sb.AppendLine(b.IsInstant ? b.Name : $"{b.Name}  {b.TimeRemaining:F1}s");
        }
        bonusText.text = sb.ToString();
    }
}