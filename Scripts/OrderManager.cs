using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI orderText;
    public GameObject orderPanel;
    
    [Header("Game Settings")]
    public int currentNight = 1;
    public int ordersPerNight = 3;
    public int currentOrderIndex = 0;
    
    [Header("Current Order")]
    public List<string> currentOrderItems = new List<string>();
    public string customerName = "";
    
    // Order database - different orders for each night
    private string[][] night1Orders = {
        new string[] {"Bread", "Ale"},
        new string[] {"Meat", "Mushrooms"},
        new string[] {"Cheese", "Wine"}
    };
    
    private string[][] night2Orders = {
        new string[] {"Bread", "Mystery Herb"},
        new string[] {"Old Meat", "Strange Mushrooms"},
        new string[] {"Moldy Cheese", "Blood Wine"}
    };
    
    private string[][] night3Orders = {
        new string[] {"Bone Meal", "Tears of Sorrow"},
        new string[] {"Rotting Flesh", "Nightmare Fungus"},
        new string[] {"The Keeper's Blood"} // Final impossible order
    };
    
    private string[] customerNames = {"Traveler", "Merchant", "Stranger", "Hooded Figure", "The Pale One"};
    
    void Start()
    {
        // Auto-find UI elements
        if (orderText == null)
            orderText = GameObject.Find("OrderText").GetComponent<TextMeshProUGUI>();
        if (orderPanel == null)
            orderPanel = GameObject.Find("OrderPanel");
            
        // Start first order
        GenerateNewOrder();
    }
    
    void Update()
    {
        // Toggle order panel with TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (orderPanel != null)
                orderPanel.SetActive(!orderPanel.activeInHierarchy);
        }
    }
    
    public void GenerateNewOrder()
    {
        currentOrderItems.Clear();
        
        // Select order based on current night
        string[] orderList;
        if (currentNight == 1)
            orderList = night1Orders[currentOrderIndex % night1Orders.Length];
        else if (currentNight == 2)
            orderList = night2Orders[currentOrderIndex % night2Orders.Length];
        else
            orderList = night3Orders[currentOrderIndex % night3Orders.Length];
        
        // Add items to current order
        for (int i = 0; i < orderList.Length; i++)
        {
            currentOrderItems.Add(orderList[i]);
        }
        
        // Pick random customer name
        customerName = customerNames[Random.Range(0, customerNames.Length)];
        
        UpdateOrderDisplay();
        
        Debug.Log("New order generated for " + customerName);
    }
    
    void UpdateOrderDisplay()
    {
        if (orderText != null)
        {
            string orderDisplay = "NIGHT " + currentNight + " - ORDER " + (currentOrderIndex + 1) + "\n\n";
            orderDisplay += "Customer: " + customerName + "\n\n";
            orderDisplay += "Requests:\n";
            
            for (int i = 0; i < currentOrderItems.Count; i++)
            {
                orderDisplay += "• " + currentOrderItems[i] + "\n";
            }
            
            orderDisplay += "\nPress TAB to toggle this panel";
            orderDisplay += "\nBring items to serving window when ready";
            
            orderText.text = orderDisplay;
        }
    }
    
    public bool CheckOrderCompletion(Inventory playerInventory)
    {
        // SPECIAL CASE: Night 3 horror ending
        if (currentNight == 3 && currentOrderItems.Contains("The Keeper's Blood"))
        {
            Debug.Log("TRIGGERING HORROR ENDING!");
            
            // Trigger the horror ending
            EndingManager endingManager = FindObjectOfType<EndingManager>();
            if (endingManager != null)
            {
                endingManager.TriggerHorrorEnding();
                return true;
            }
            else
            {
                Debug.Log("EndingManager not found! Make sure you created the EndingManager GameObject.");
                return false;
            }
        }
        
        // Normal order checking for Nights 1 & 2
        foreach (string item in currentOrderItems)
        {
            if (!playerInventory.items.Contains(item))
            {
                Debug.Log("Missing: " + item);
                return false;
            }
        }
        
        // Remove items from inventory
        foreach (string item in currentOrderItems)
        {
            playerInventory.items.Remove(item);
        }
        
        Debug.Log("Order completed for " + customerName + "!");
        
        // Move to next order
        currentOrderIndex++;
        if (currentOrderIndex >= ordersPerNight)
        {
            // Night complete
            CompleteNight();
        }
        else
        {
            GenerateNewOrder();
        }
        
        return true;
    }
    
    void CompleteNight()
    {
        Debug.Log("Night " + currentNight + " completed!");
        
        // Show journal entry after completing night
        JournalManager journalManager = FindObjectOfType<JournalManager>();
        if (journalManager != null)
        {
            journalManager.ShowJournalEntry(currentNight);
        }
        
        if (currentNight >= 3)
        {
            Debug.Log("Game completed!");
            // Game over - journal will handle ending
        }
        else
        {
            currentNight++;
            currentOrderIndex = 0;
            Debug.Log("Moving to Night " + currentNight);
            GenerateNewOrder();
        }
    }
}