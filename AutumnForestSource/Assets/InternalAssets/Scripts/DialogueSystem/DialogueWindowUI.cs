﻿using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace AutumnForest.DialogueSystem
{
    [RequireComponent(typeof(Animator))]
    public class DialogueWindowUI : UIWindow
    {
        private const string windowEnableAnimationName = "DialogueWindowEnable";
        private const string windowDisableAnimationName = "DialogueWindowDisable";

        private Animator animator;

        [SerializeField] private GameObject dialogueWindowUI;
        [SerializeField] private Text dialogueTextUI;
        [SerializeField] private Text dialogueNameUI;

        [SerializeField] private float textSpeed = 0.02f;

        private CancellationTokenSource token = new();
        //костыль момент
        private UniTask disableTask;

        private void Start()
        {
            dialogueWindowUI.SetActive(false);
            animator = GetComponent<Animator>();

            GlobalServiceLocator.GetService<DialogueManager>().OnDialogueStarted += OnDialogueStarted;
            GlobalServiceLocator.GetService<DialogueManager>().OnDialogueEnded += OnDialogueEnded;
            GlobalServiceLocator.GetService<DialogueManager>().OnPhraseSwitched += ShowPhrase;
        }

        private void OnDialogueStarted(Dialogue dialogue)
        {
            SelfEnable();
            dialogueNameUI.text = dialogue.dialogueName;
        }
        private void OnDialogueEnded(Dialogue dialogue)
        {
            SelfDisable();
        }

        private async void ShowPhrase(string name, string text)
        {
            dialogueNameUI.text = name;
            dialogueTextUI.text = "";

            for (int i = 0; i < text.Length; i++)
            {
                dialogueTextUI.text += text[i];
                await UniTask.Delay(TimeSpan.FromSeconds(textSpeed), cancellationToken: token.Token);
            }
        }

        protected override async void SelfEnable()
        {
            if (disableTask.Status == UniTaskStatus.Pending)
                await disableTask;

            dialogueWindowUI.SetActive(true);
            animator.Play(windowEnableAnimationName);
        }
        protected override async void SelfDisable()
        {
            dialogueTextUI.text = "";
            dialogueNameUI.text = "";

            animator.Play(windowDisableAnimationName);
            float waitTime = animator.GetCurrentAnimatorStateInfo(0).length - 0.8f;

            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

            dialogueWindowUI.SetActive(false);
        }
    }
}