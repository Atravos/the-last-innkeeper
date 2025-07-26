using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("Items")]
    public List<string> items = new List<string>();
    public List<string> keys = new List<string>(); // NEW: Separate key storage
    public int maxItems = 10;
    
    [Header("UI References")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI interactionPrompt;
    
    private bool isInventoryOpen = false;
    
    void Start()
    {
        // Auto-find UI elements if not assigned - with safety checks
        if (inventoryPanel == null)
        {
            GameObject found = GameObject.Find("InventoryPanel");
            if (found != null)
                inventoryPanel = found;
        }
        
        if (inventoryText == null)
        {
            GameObject found = GameObject.Find("InventoryText");
            if (found != null)
                inventoryText = found.GetComponent<TextMeshProUGUI>();
        }
        
        if (interactionPrompt == null)
        {
            GameObject found = GameObject.Find("InteractionPrompt");
            if (found != null)
                interactionPrompt = found.GetComponent<TextMeshProUGUI>();
        }
            
        // Make sure inventory starts closed
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
            
        // Make sure interaction prompt starts hidden
        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false);
    }
    
    public bool AddItem(string itemName)
    {
        // Check if it's a key
        if (itemName.Contains("Key"))
        {
            keys.Add(itemName);
            Debug.Log("Picked up key: " + itemName);
            UpdateInventoryUI();
            return true;
        }
        else
        {
            // Regular item
            if (items.Count < maxItems)
            {
                items.Add(itemName);
                Debug.Log("Picked up: " + itemName);
                UpdateInventoryUI();
                return true;
            }
            else
            {
                Debug.Log("Inventory full!");
                return false;
            }
        }
    }
    
    // NEW: Check if player has a specific key
    public bool HasKey(string keyType)
    {
        return keys.Contains(keyType);
    }
    
    // NEW: Use a key (remove it from inventory)
    public bool UseKey(string keyType)
    {
        if (keys.Contains(keyType))
        {
            keys.Remove(keyType);
            UpdateInventoryUI();
            return true;
        }
        return false;
    }
    
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isInventoryOpen);
            if (isInventoryOpen)
                UpdateInventoryUI();
        }
    }
    
    public void UpdateInventoryUI()
    {
        if (inventoryText != null)
        {
            string inventoryContent = "INVENTORY\n\n";
            
            // Show Keys
            inventoryContent += "KEYS (" + keys.Count + "):\n";
            if (keys.Count == 0)
            {
                inventoryContent += "• None\n";
            }
            else
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    inventoryContent += "• " + keys[i] + "\n";
                }
            }
            
            inventoryContent += "\nITEMS (" + items.Count + "/" + maxItems + "):\n";
            if (items.Count == 0)
            {
                inventoryContent += "• None\n";
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    inventoryContent += "• " + items[i] + "\n";
                }
            }
            
            inventoryText.text = inventoryContent;
        }
    }
    
    // Show interaction prompt when looking at items
    public void ShowInteractionPrompt(string itemName)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.text = "Press E to pickup " + itemName;
            interactionPrompt.gameObject.SetActive(true);
        }
    }
    
    // Show interaction prompt for doors
    public void ShowDoorPrompt(string message)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.text = message;
            interactionPrompt.gameObject.SetActive(true);
        }
    }
    
    // Hide interaction prompt when not looking at items
    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }
}