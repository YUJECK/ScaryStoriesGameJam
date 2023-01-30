using AutumnForest.Health;
using AutumnForest.Helpers;
using CreaturesAI.CombatSkills;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace AutumnForest.BossFight.Squirrels
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CreatureHealth))]
    public class DefaultSquirrel : Squirrel
    {
        [SerializeField] private Shooting shooting;
        private CreatureHealth health;
        private Animator animator;

        [SerializeField] private string shotAnimation;
        [SerializeField] private float shootRate = 2.5f;
        [SerializeField] private float shootSpeed = 10;
        [SerializeField] private float spread = 10;

        private CancellationTokenSource token;

        private void Awake()
        {
            health = GetComponent<CreatureHealth>();
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            health.OnDie += SpawnHealAcorn;
            shooting.OnShoot += OnShoot;

            token = new();
            Shooting(token.Token);
        }
        private void OnDisable()
        {
            health.OnDie -= SpawnHealAcorn;
            shooting.OnShoot -= OnShoot;

            token.Cancel();
        }
        private void OnDestroy() => token.Cancel();

        private void SpawnHealAcorn()
        {
            GlobalServiceLocator.GetService<SomePoolsContainer>().AcornHealPool.GetFree().transform.position = transform.position;
        }

        private void OnShoot(Rigidbody2D obj)
        {
            animator.Play(shotAnimation);
        }

        private async void Shooting(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                if(gameObject.activeInHierarchy)
                {
                    shooting.ShootWithoutInstantiate(GlobalServiceLocator.GetService<SomePoolsContainer>().AcornPool.GetFree().Rigidbody2D,
                        shootSpeed, UnityEngine.Random.Range(0, spread), ForceMode2D.Impulse);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(shootRate));
            }
        }
    }
}