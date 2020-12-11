using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
[UpdateAfter(typeof(SpawnerSystem))]
public class PlayerInputSystem : SystemBase
{
    Entity player;
    protected override void OnCreate()
    {
        base.OnCreate();
    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);
        bool specialAttack = Input.GetKeyDown(KeyCode.Mouse1);
        DynamicBuffer<HealthData> playerHealth =GetBufferFromEntity<HealthData>(true)[player]; ;

        Entities.WithAll<PlayerTag>().ForEach((Entity entity, int entityInQueryIndex, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
         {
             if (playerHealth[0].hp <= 0)
             {
                 animationData.animationStateConstant = false;
                 characterStateData.nextState = CharacterState.Dying;
             }
             if (attack)
             {
                 animationData.animationStateConstant = false;
                 characterStateData.nextState = CharacterState.Attacking;
             }
             if (specialAttack)
             {
                 animationData.animationStateConstant = false;
                 characterStateData.nextState = CharacterState.SpecialAttack;
             }

         }).Schedule();

    }

}