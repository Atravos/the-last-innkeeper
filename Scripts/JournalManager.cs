using UnityEngine;
using TMPro;
using System.Collections;

public class JournalManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject journalPanel;
    public TextMeshProUGUI journalText;
    
    [Header("Story Content")]
    private string[] journalEntries = {
        // After Night 1
        @"Day 1 - Found this journal hidden behind the bar

'The Inn has been in my family for generations. 
Business has been... strange lately. The customers 
order normal food, but they never seem to eat it. 
They just stare. And their eyes... something's not right.

I found bite marks on the cellar door this morning.
What kind of animal could do that?

- Thomas, Previous Innkeeper'",

        // After Night 2  
        @"Day 15 - Another page, more desperate

'They're asking for impossible things now. 
""Tears of the mountain"" - what does that even mean? 
""Blood wine"" - I tried serving them regular wine 
with food coloring. They knew. They KNEW.

The townspeople avoid this place now. I'm alone 
with... whatever these things are. I hear scratching 
in the walls at night. Whispers in languages 
I don't understand.

I should leave. But I can't. Something won't let me.

- Thomas'",

        // After Night 3
        @"Day 23 - Final Entry

'They told me what they want tonight. 
They want ME. They consumed the innkeeper before Thomas, 
and the one before him. This place... it feeds on us.

The customers aren't customers. They're spirits, 
ancient and hungry. They've been playing with me, 
making me cook, making me serve, fattening me up 
like livestock.

Tonight they asked for ""the keeper's blood.""
That's when I understood. I'm not the innkeeper.
I'm the meal.

If anyone finds this journal... RUN. Don't stay. 
Don't serve them. They're already watching you.

- Thomas

[The handwriting becomes erratic]

I can hear them coming up the stairs...'"
    };
    
    private bool isJournalOpen = false;
    private int currentEntry = 0;
    
    void Start()
    {
        // Auto-find UI elements
        if (journalPanel == null)
            journalPanel = GameObject.Find("JournalPanel");
        if (journalText == null)
            journalText = GameObject.Find("JournalText").GetComponent<TextMeshProUGUI>();
            
        // Make sure journal starts hidden
        if (journalPanel != null)
            journalPanel.SetActive(false);
    }
    
    void Update()
    {
        // Close journal with SPACE
        if (isJournalOpen && Input.GetKeyDown(KeyCode.Space))
        {
            CloseJournal();
        }
    }
    
    public void ShowJournalEntry(int nightCompleted)
    {
        if (nightCompleted > 0 && nightCompleted <= journalEntries.Length)
        {
            currentEntry = nightCompleted - 1;
            isJournalOpen = true;
            
            if (journalPanel != null)
                journalPanel.SetActive(true);
                
            if (journalText != null)
                journalText.text = journalEntries[currentEntry];
            
            // Pause the game while reading
            Time.timeScale = 0f;
            
            // Unlock cursor for UI interaction
            Cursor.lockState = CursorLockMode.None;
            
            Debug.Log("Showing journal entry for Night " + nightCompleted);
        }
    }
    
    void CloseJournal()
    {
        isJournalOpen = false;
        
        if (journalPanel != null)
            journalPanel.SetActive(false);
            
        // Resume game
        Time.timeScale = 1f;
        
        // Lock cursor back
        Cursor.lockState = CursorLockMode.Locked;
        
        // Check if this was the final entry
        if (currentEntry >= 2) // Night 3 completed
        {
            ShowGameEnding();
        }
        
        Debug.Log("Journal closed");
    }
    
    void ShowGameEnding()
    {
        Debug.Log("GAME COMPLETED - The Last Innkeeper");
        Debug.Log("You now understand the truth of this cursed place...");
        
        // Could add ending screen here, or return to menu
        // For now, just a debug message
    }
}