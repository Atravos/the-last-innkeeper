using UnityEngine;

public class ServingWindow : MonoBehaviour
{
    [Header("Serving Window Settings")]
    public string windowName = "Serving Window";
    
    void Start()
    {
        // Optional: Add any setup needed for the serving window
        Debug.Log(windowName + " is ready for service!");
    }
    
    // This component just marks an object as a serving window
    // The actual interaction logic is handled in PlayerController
}