using AutumnForest.BossFight.Fox.States;
using AutumnForest.Health;
using AutumnForest.Helpers;
using AutumnForest.Projectiles;
using AutumnForest.StateMachineSystem;
using System;
using UnityEngine;

namespace AutumnForest.BossFight.Fox
{
    public class FoxStateMachineUser : MonoBehaviour, IStateMachineUser
    {
        [SerializeField] private ParticleSystem jumpParticle;
        [Header("SFX")]
        [SerializeField] private AudioSource castSoundEffect;
        [Header("Services")]
        [SerializeField] private Shooting shooting;
        [SerializeField] private CreatureAnimator animator;
        [SerializeField] private CreatureHealth creatureHealth;
        [SerializeField] private Transform[] points;

        public StateMachine StateMachine { get; private set; }
        public LocalServiceLocator ServiceLocator { get; private set; }

        public event Action<StateBehaviour> OnStateChanged;

        private StateBehaviour[] attackPatterns;

        private void Awake()
        {
            StateBehaviour[] patterns =
            {
                new TimingState(2.5f),
                new TimingState(3.5f),
                new FoxFirstFlowerPattern(points, castSoundEffect, 1f, "FoxCasting"),
                new FoxFirstFlowerPattern(points, castSoundEffect, 0.5f, "FoxCasting"),
                new FoxSingleSwordCastState(15),
                new FoxSerialSwordThowing(points, castSoundEffect, 0.1f, 0.5f, "FoxCasting"),
            };

            attackPatterns = patterns;

            creatureHealth.OnDied += OnDie;

            ServiceLocator = new(shooting, creatureHealth, animator);
            StateMachine = new(this, false);

            StateMachine.OnMachineWorking += StateChoosing;

            DisableObject();
        }

        private void OnDie()
        {
            StateMachine.DisableStateMachine();
            animator.PlayAnimation("FoxJump");
        }
        private void DisableObject() => gameObject.SetActive(false); //for animator
        private void PlayJumpParticle() => jumpParticle.Play(); //for animator
        private void StateChoosing()
        {
            OnStateChanged?.Invoke(ObjectRandomizer.GetRandom(attackPatterns));
        }
    }
}