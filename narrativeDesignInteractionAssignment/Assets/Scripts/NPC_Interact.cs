using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn;
using Yarn.Unity;

public class NPC_Interact : MonoBehaviour
{
    //Done! check if player is in the trigger radius
    //Done! activate interaction text
    //Done! check if player is in the trigger when they press the interaction key
    //Done! activate Yarn Spinner -- our Dialogue runner
    //Done! Checking player controller for movement
    //Done! player movement is locked

    //semi finished... 'limit' (and eventually control) camera movement

    public TextMeshProUGUI interactText;
    public string interaction;
    public bool canInteract;
    public DialogueRunner dialogue;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            setText(interaction);
            canInteract = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            setText("");
            canInteract = false;
        }
    }

    //setter method for interact text
    public void setText(string txt)
    {
        interactText.text = txt;
    }

    public void Update()
    {
        if(canInteract)
        {
            if(Keyboard.current.eKey.wasPressedThisFrame) //new syntax, no longer getKeyDown
            {
                Debug.Log("You talked to the NPC");
                setText("");
                dialogue.StartDialogue("Start"); //be mindful of the case of yarnSpinner node
                canInteract = false;
            }
        }
    }
}
