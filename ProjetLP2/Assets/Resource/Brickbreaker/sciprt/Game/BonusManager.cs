using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// Tracks all currently active bonuses/maluses and displays them with their
/// remaining duration on a shared UI text element.
/// </summary>
public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance { get; private set; }

    [SerializeField] private TextMeshPro bonusText;

    /// <summary>A single active bonus/malus entry being tracked.</summary>
    private class ActiveBonus
    {
        public string Name;
        public float  TimeRemaining;
        public bool   IsInstant; 

        public ActiveBonus(string name, float duration, bool isInstant = false)
        {
            Name          = name;
            TimeRemaining = duration;
            IsInstant     = isInstant;
        }
    }

    private readonly List<ActiveBonus> activeBonuses = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        for (int i = activeBonuses.Count - 1; i >= 0; i--)
        {
            activeBonuses[i].TimeRemaining -= Time.deltaTime;
            if (activeBonuses[i].TimeRemaining <= 0f)
                activeBonuses.RemoveAt(i);
        }

        RefreshUI();
    }

    /// <summary>Registers a new bonus, or refreshes its duration if one with the same name is already active.</summary>
    public void Register(string name, float duration)
    {
        var existing = activeBonuses.Find(b => b.Name == name);
        if (existing != null)
            existing.TimeRemaining = duration;
        else
            activeBonuses.Add(new ActiveBonus(name, duration));
    }

    /// <summary>Rebuilds the bonus list text, one line per active bonus with its remaining time.</summary>
    private void RefreshUI()
    {
        if (bonusText == null) return;

        if (activeBonuses.Count == 0)
        {
            bonusText.text = string.Empty;
            return;
        }

        var sb = new StringBuilder();
        foreach (var b in activeBonuses)
        {
            sb.AppendLine(b.IsInstant ? b.Name : $"{b.Name}  {b.TimeRemaining:F1} s");
        }
        bonusText.text = sb.ToString();
    }
}