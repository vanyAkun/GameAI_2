using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject npcDeath; // The NPC that needs to be killed for the new NPC to appear
    public GameObject newNPC; // The new NPC that will appear

   

    void Update()
    {
        // Check if the NPC to monitor is null (which means it's destroyed)
        if (npcDeath == null)
        {
            // Check if the new NPC is not active in the scene
            if (newNPC != null && !newNPC.activeSelf)
            {
                // Activate the new NPC
                newNPC.SetActive(true);
            }
            // If using a prefab and you want to instantiate a new NPC
            

            // Optional: Disable this script if it's no longer needed
            
        }
    }
}