using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public Sprite emptySlot;
    public GameObject slotPrefab;
    private List<Image> slots = new List<Image>();
    private List<Image> uiItems = new List<Image>();
    [SerializeField] public List<GameObject> possibleItems = new List<GameObject>();
    private List<BaseItem> items = new List<BaseItem>();
    private System.Random random = new System.Random();

    private void Start()
    {
        // Initialize with 6 slots
        for (int i = 0; i < 6; i++)
        {
            AddSlot();
            items.Add(null);
        }
        
        NormalizeChances();
    }
    
    public void Reroll()
    {
        Debug.Log("Rerolling inventory...");
        for (int i = 0; i < slots.Count; i++)
        {
            // Remove the old item if there is one
            if (items[i] != null)
            {
                items[i] = null;
            }
            
            // Roll the dice for the new item and set the UI representation
            Debug.Log("Rolling dice for slot " + i);
            GameObject item = RollDice(i);
            Debug.Log("Rolled item: " + item);
            if (item != null)
            {
                items[i] = item.GetComponent<BaseItem>();
                AddItemToSlot(i, item.GetComponent<BaseItem>().UIImage);
            }
        }
    }

    private void AddSlot()
    {
        GameObject newSlot = Instantiate(slotPrefab, transform);
        slots.Add(newSlot.GetComponent<Image>());
        // Make the slot's child invisible
        newSlot.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }
    
    public void AddItem(BaseItem item, int slot)
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
    
    public void AddItemToSlot(int slot, Sprite itemSprite)
    {
        if (slot < 0 || slot >= slots.Count)
        {
            Debug.LogError("Invalid slot number");
            return;
        }
        
        Image slotImage = slots[slot].transform.GetChild(0).GetComponent<Image>();
        // Set sprite size according to the units in the sprite
        slotImage.rectTransform.sizeDelta = new Vector2(itemSprite.rect.width, itemSprite.rect.height);
        slotImage.sprite = itemSprite;
        slotImage.enabled = true;
        Debug.Log("Enabled status: " + slotImage.enabled);
        Debug.Log("Added item to slot " + slot);
    }


    private void NormalizeChances()
    {
        float totalChance = 0;
        foreach (var item in possibleItems)
        {
            if (item != null)
            {
                totalChance += item.GetComponent<BaseItem>().Chance;
            }
        }
        
        Debug.Log("Total chance: " + totalChance);

        foreach (var item in possibleItems)
        {
            if (item != null)
            {
                item.GetComponent<BaseItem>().Chance /= totalChance;
                Debug.Log("Normalized chance: " + item.GetComponent<BaseItem>().Chance);
            }
        }
    }

    public GameObject RollDice(int slot)
    {
        if (slot < 0 || slot >= slots.Count)
        {
            Debug.LogError("Invalid slot number");
            return null;
        }

        float roll = (float)random.NextDouble();
        
        float currentChance = 0;
        foreach (var item in possibleItems)
        {
            if (item != null)
            {
                currentChance += item.GetComponent<BaseItem>().Chance;
                if (roll < currentChance)
                {
                    return item;
                }
            }
        }
        
        return null;
    }

    public void ClearSlot(int slot)
    {
        if (slot < 0 || slot >= slots.Count)
        {
            Debug.LogError("Invalid slot number");
            return;
        }

        items[slot] = null;
        slots[slot].sprite = emptySlot;
    }
}