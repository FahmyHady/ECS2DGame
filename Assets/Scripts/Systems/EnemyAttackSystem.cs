using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAttackSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.WithAll<EnemyTag>().WithNone<BusyTag,DyingTag>().ForEach((ref AnimationData animationData, ref CharacterStateData characterStateData,ref AttackTimerData attackTimerData) =>
        {
            attackTimerData.elapsedAttackTime += deltaTime;
            if (attackTimerData.elapsedAttackTime>=attackTimerData.timeBetweenAttacks)
            {
                attackTimerData.elapsedAttackTime -= attackTimerData.timeBetweenAttacks;
                animationData.animationStateConstant = false;
                characterStateData.nextState = CharacterState.Attacking;
            }    
        }).Schedule();
    }
}