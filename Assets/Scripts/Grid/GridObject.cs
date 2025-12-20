using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridObjectSO data;           // assigned right after Instantiate

    private int currentHealth;
    private bool isChipped = false;

    void Awake()
    {
        currentHealth = data.maxHealth;
    }

    /// <summary>
    /// Call this when something impacts this object (damage or knock).
    /// </summary>
    public void ApplyDamage(int amount)
    {
        if (amount <= 0 || !data.isBreakable) return;

        currentHealth -= amount;
        data.onHit.Invoke();

        // handle “chip” threshold
        if (!isChipped && currentHealth <= data.chipThreshold)
        {
            isChipped = true;
            // optional: change sprite/material to cracked version
        }

        // break if dead
        if (currentHealth <= 0)
        {
            data.onBreak.Invoke();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called by your knockback system if this object is shoved.
    /// </summary>
    public bool TryPush(int tiles)
    {
        if (!data.isPushable) return false;
        // your shove logic here (e.g. start a coroutine that moves this transform)
        return true;
    }
}