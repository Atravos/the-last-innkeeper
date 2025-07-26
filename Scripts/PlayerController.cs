using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 8f;
    public float gravity = 20f;
    
    // Interaction variables
    public float interactRange = 3f;
    public Camera playerCamera;
    
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Inventory inventory;
    private OrderManager orderManager; // NEW: Reference to order system
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<Inventory>();
        orderManager = FindObjectOfType<OrderManager>(); // NEW: Find order manager
        
        // Auto-find camera if not assigned
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }
    
    void HandleMovement()
    {
        if (controller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpHeight;
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    
    void HandleInteraction()
    {
        // Check what we're looking at
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // Check for pickup items (ingredients)
            PickupItem item = hit.transform.GetComponent<PickupItem>();
            if (item != null && item.canPickup)
            {
                inventory.ShowInteractionPrompt(item.itemName);
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (inventory.AddItem(item.itemName))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            // NEW: Check for serving window
            else
            {
                ServingWindow servingWindow = hit.transform.GetComponent<ServingWindow>();
                if (servingWindow != null)
                {
                    inventory.ShowInteractionPrompt("Press E to serve order");
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Try to complete the current order
                        if (orderManager != null)
                        {
                            if (orderManager.CheckOrderCompletion(inventory))
                            {
                                Debug.Log("Order served successfully!");
                                // Update inventory UI after items are removed
                                inventory.UpdateInventoryUI();
                            }
                            else
                            {
                                Debug.Log("You don't have the right ingredients!");
                            }
                        }
                    }
                }
                else
                {
                    inventory.HideInteractionPrompt();
                }
            }
        }
        else
        {
            inventory.HideInteractionPrompt();
        }
        
        // Toggle inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.ToggleInventory();
        }
    }
}