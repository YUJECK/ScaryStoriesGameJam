using AutumnForest.Helpers;
using AutumnForest.Other;
using UnityEngine;

namespace AutumnForest.DialogueSystem
{
    [RequireComponent(typeof(Dialogue))]
    public class InteractiveDialogue : MonoBehaviour, IInteractive, ICreatureComponent
    {
        private Dialogue dialogue;
        public Dialogue Dialogue
        {
            get
            {
                if (dialogue == null)
                    dialogue = GetComponent<Dialogue>();

                return dialogue;
            }

            private set => dialogue = value;
        }

        private void Awake()
        {
            if (dialogue != null)
                dialogue = GetComponent<Dialogue>();
        }

        public void Detect() => Dialogue.StartDialogue();
        public void Interact() => Dialogue.NextPhrase();
        public void DetectionReleased() => Dialogue.EndDialogue();  
    }
}