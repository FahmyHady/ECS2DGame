using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using System;
[UpdateBefore(typeof(AnimationRendererSystem))]
[UpdateAfter(typeof(AnimationStateUpdateSystem))]
public class TriggerCallsSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
    Entity player;
    protected override void OnCreate()
    {
        base.OnCreate();
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        DynamicBuffer<HealthData> playerHealth = GetBufferFromEntity<HealthData>(false)[player];
        Unity.Collections.NativeArray<Entity> enemies = GetEntityQuery(typeof(EnemyTag)).ToEntityArray(Unity.Collections.Allocator.TempJob);
        Dependency = Entities.WithAll<TriggerTag, EnemyTag>().ForEach((Entity entity, int entityInQueryIndex, ref CharacterStateData characterStateData, ref AnimationData animationData, ref StrengthData strengthData) =>
             {
                 if (animationData.currentFrame == animationData.triggerData.triggerFrame)
                 {
                     switch (characterStateData.currentState)
                     {
                         case CharacterState.Attacking:
                             var playerHealthBuffer = playerHealth[0];
                             playerHealthBuffer.hp -= strengthData.strength;
                             playerHealth[0] = playerHealthBuffer;
                             buffer.RemoveComponent<TriggerTag>(entityInQueryIndex, entity);
                             break;
                     }
                 }
             }).Schedule(Dependency);

        Dependency = Entities.WithAll<TriggerTag, PlayerTag>().ForEach((Entity entity, int entityInQueryIndex, ref CharacterStateData characterStateData, ref AnimationData animationData) =>
             {
                 if (enemies.Length > 0)
                 {
                     if (animationData.currentFrame == animationData.triggerData.triggerFrame)
                     {
                         switch (characterStateData.currentState)
                         {
                             case CharacterState.Attacking:
                                 buffer.AddComponent<DyingTag>(entityInQueryIndex, enemies[0]);
                                 buffer.RemoveComponent<TriggerTag>(entityInQueryIndex, entity);
                                 break;

                             case CharacterState.SpecialAttack:
                                 if (animationData.currentFrame == animationData.triggerData.triggerFrame)
                                 {
                                     for (int i = 0; i < enemies.Length; i++)
                                     {
                                         buffer.AddComponent<DyingTag>(entityInQueryIndex, enemies[i]);
                                     }
                                     buffer.RemoveComponent<TriggerTag>(entityInQueryIndex, entity);
                                 }
                                 break;
                         }
                     }
                 }
             }).ScheduleParallel(Dependency);

        Dependency.Complete();
        enemies.Dispose();
    }

}
