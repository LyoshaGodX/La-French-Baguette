using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public Sprite emptySlot;
    public List<Image> slots = new List<Image>();
    [SerializeField] public List<GameObject> possibleItems = new List<GameObject>();
    public List<GameObject> items = new List<GameObject>(); // Changed to GameObject
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
                items[i] = item;
                AddItemToSlot(i, item.GetComponent<BaseItem>().UIImage);
            }
        }
    }

    private void AddSlot()
    {
        // Create new game object from scratch (not using the prefab) with the following structure
        // and put it into Market panel as a child:
        // EmptySlot (<Image>) -(child)-> UIRepresentation (<Image>)
        GameObject newSlot = new GameObject("EmptySlot");
        newSlot.transform.SetParent(transform);
        
        GameObject uiRepresentation = new GameObject("UIRepresentation");
        uiRepresentation.transform.SetParent(newSlot.transform);
        
        // Add drag and drop functionality to the UI representation
        uiRepresentation.AddComponent<CanvasGroup>();
        uiRepresentation.AddComponent<DragDrop>();
        
        Image slotImage = newSlot.AddComponent<Image>();
        slots.Add(slotImage);
        
        Image uiImage = uiRepresentation.AddComponent<Image>();
        slotImage.sprite = emptySlot;
        
        uiImage.enabled = false;
        
        // Resize the UI representation to match the size of the empty slot
        slotImage.rectTransform.sizeDelta = new Vector2(emptySlot.rect.width, emptySlot.rect.height);
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
        slots[slot].transform.GetChild(0).GetComponent<Image>().sprite = null;
        slots[slot].transform.GetChild(0).GetComponent<Image>().enabled = false;
    }
}