using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public Sprite emptySlot;
    public GameObject slotPrefab;
    private List<Image> slots = new List<Image>();
    private List<ValleyItem> items = new List<ValleyItem>();
    private System.Random random = new System.Random();

    private void Start()
    {
        // Initialize with 6 slots
        for (int i = 0; i < 6; i++)
        {
            AddSlot();
            items.Add(null);
        }
    }

    private void AddSlot()
    {
        GameObject newSlot = Instantiate(slotPrefab, transform);
        slots.Add(newSlot.GetComponent<Image>());
    }

    public void AddItem(ValleyItem item, int slot)
    {
        if (slot < 0 || slot >= items.Count)
        {
            Debug.LogError("Invalid slot number");
            return;
        }

        items[slot] = item;
        slots[slot].sprite = item.GetComponent<SpriteRenderer>().sprite;
        NormalizeChances();
    }

    private void NormalizeChances()
    {
        float totalChance = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                totalChance += item.Chance;
            }
        }

        foreach (var item in items)
        {
            if (item != null)
            {
                item.Chance /= totalChance;
            }
        }
    }

    public ValleyItem RollDice(int slot)
    {
        if (slot < 0 || slot >= items.Count)
        {
            Debug.LogError("Invalid slot number");
            return null;
        }

        float roll = (float)random.NextDouble();
        float cumulative = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                cumulative += item.Chance;
                if (roll <= cumulative)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public void ClearSlot(int slot)
    {
        if (slot < 0 || slot >= items.Count)
        {
            Debug.LogError("Invalid slot number");
            return;
        }

        items[slot] = null;
        slots[slot].sprite = emptySlot;
    }
}