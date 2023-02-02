﻿using AutumnForest.Projectiles;
using AutumnForest.StateMachineSystem;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace AutumnForest.BossFight.Raccoon.States
{
    public sealed class TripleShotState : StateBehaviour
    {
        private Rigidbody2D projectile;
        private float shotSpeed;
        private float shotSpread;
        private float shotRate;

        public TripleShotState(Rigidbody2D projectile, float shotSpeed, float shotSpread, float shotRate)
        {
            if (projectile == null)
                throw new NullReferenceException(nameof(projectile));

            this.projectile = projectile;
            this.shotSpeed = shotSpeed;
            this.shotSpread = shotSpread;
            this.shotRate = shotRate;
        }

        public override async void EnterState(IStateMachineUser stateMachine)
        {
            IsCompleted = false;
            {
                stateMachine.ServiceLocator.GetService<Shooting>().TransformRotation.Enable();
                stateMachine.ServiceLocator.GetService<Shooting>().TransformRotation.RotationType = TransformRotation.RotateType.ByTarget;

                await Shoot(stateMachine);
            }
            IsCompleted = true;
        }
        public override void ExitState(IStateMachineUser stateMachine)
        {
            stateMachine.ServiceLocator.GetService<Shooting>().TransformRotation.Disable();
        }

        private async UniTask Shoot(IStateMachineUser stateMachine)
        {
            for (int i = 0; i < 3; i++)
            {
                stateMachine.ServiceLocator.GetService<Shooting>()
                    .ShootWithInstantiate(projectile, shotSpeed, UnityEngine.Random.Range(-shotSpread, shotSpread));

                await UniTask.Delay(TimeSpan.FromSeconds(shotRate));
            }
        }
    }
}