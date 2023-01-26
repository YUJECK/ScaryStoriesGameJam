﻿using AutumnForest.BossFight.Raccoon;
using AutumnForest.Projectiles;
using AutumnForest.StateMachineSystem;
using UnityEngine;

namespace AutumnForest.Raccoon
{
    public class RaccoonStateVariator : MonoBehaviour, IStateContainerVariator
    {
        [Header("Prefabs")]
        [SerializeField] private Transform projectileContainer;
        [SerializeField] private Projectile conePrefab;
        [SerializeField] private Projectile shirtPrefab;

        public IStateContainer InitStates()
        {
            return new RaccoonStatesContainer(
                new RaccoonIdleState(),
                new RaccoonRoundShotState(conePrefab, projectileContainer));
        }
    }
}