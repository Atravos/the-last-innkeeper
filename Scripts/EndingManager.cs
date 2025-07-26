using UnityEngine;
using TMPro;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject endingPanel;
    public TextMeshProUGUI endingText;
    
    private bool endingActive = false;
    
    void Start()
    {
        // Auto-find UI elements
        if (endingPanel == null)
            endingPanel = GameObject.Find("EndingPanel");
        if (endingText == null)
            endingText = GameObject.Find("EndingText").GetComponent<TextMeshProUGUI>();
            
        // Make sure ending starts hidden
        if (endingPanel != null)
            endingPanel.SetActive(false);
    }
    
    void Update()
    {
        // Close ending with SPACE
        if (endingActive && Input.GetKeyDown(KeyCode.Space))
        {
            CloseEnding();
        }
    }
    
    public void TriggerHorrorEnding()
    {
        endingActive = true;
        
        // Show ending panel
        if (endingPanel != null)
            endingPanel.SetActive(true);
            
        // Set horror text
        if (endingText != null)
        {
            endingText.text = @"As you approach the serving window, dark shapes emerge from the shadows.

            The customers finally reveal themselves - not people, but ancient hungry spirits.

            'Thank you for preparing yourself so well,' they whisper in voices like wind through graves.

            'Thomas fought us. He was tough and bitter. 

            But you... you've been such a good keeper. 

            You'll taste much sweeter.'

            The darkness closes in...

            THE END";
        }
        
        // Pause the game
        Time.timeScale = 0f;
        
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        
        Debug.Log("HORROR ENDING TRIGGERED - The Last Innkeeper");
    }
    
    void CloseEnding()
    {
        endingActive = false;
        
        if (endingPanel != null)
            endingPanel.SetActive(false);
            
        // Resume game (or could quit/restart here)
        Time.timeScale = 1f;
    
        // For now, just freeze the game
        Time.timeScale = 0f;
    }
}