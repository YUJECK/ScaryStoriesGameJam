﻿using AutumnForest.BossFight;
using AutumnForest.BossFight.Squirrels;
using AutumnForest.Helpers;
using AutumnForest.StateMachineSystem;
using Cysharp.Threading.Tasks;
using System;

namespace AutumnForest.Raccoon.States
{
    public sealed class RaccoonSquirrelSpawnState : StateBehaviour
    {
        private ObjectPool<Squirrel> squirrelPool;

        private int squirrelMinCount;
        private int squirrelMaxCount;
        private float spawnRate;

        public RaccoonSquirrelSpawnState(Squirrel squirrelPrefab, int squirrelMinCount, int squirrelMaxCount, float spawnRate)
        {
            squirrelPool = new(squirrelPrefab, GlobalServiceLocator.GetService<ContainerHelper>().CreatureContainer, squirrelMaxCount, true);

            this.squirrelMinCount = squirrelMinCount;
            this.squirrelMaxCount = squirrelMaxCount;
            this.spawnRate = spawnRate;
        }

        public override async void EnterState(IStateMachineUser stateMachine)
        {
            stateMachine.ServiceLocator.GetService<CreatureAnimator>().SetDefault();

            IsCompleted = false;
            {
                int squirrelCount = UnityEngine.Random.Range(squirrelMinCount, squirrelMaxCount);

                for (int i = 0; i < squirrelCount; i++)
                {
                    squirrelPool.GetFree().transform.position = stateMachine.ServiceLocator.GetService<SpawnPlace>().GetPosition();
                    await UniTask.Delay(TimeSpan.FromSeconds(spawnRate));
                }
            }
            IsCompleted = true;
        }

        public override bool CanEnterNewState() => IsCompleted;
    }
}