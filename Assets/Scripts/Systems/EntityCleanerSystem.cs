using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using System;
using UnityEngine.SceneManagement;
[UpdateAfter(typeof(TriggerCallsSystem))]
public class EntityCleanerSystem : SystemBase
{
    float elapsedTime;
    int playerDeathCount;
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
    DynamicBuffer<HealthData> playerHealthBuffer;
    Entity player;
    protected override void OnCreate()
    {
        base.OnCreate();
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        playerHealthBuffer = GetBufferFromEntity<HealthData>()[player];
        var bufferConcurrent = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        DynamicBuffer<InventoryData> playerInventoryBuffer = GetBufferFromEntity<InventoryData>()[player];
        Dependency = Entities.WithAll<WaitingToBeDestroyedTag, EnemyTag>().ForEach((Entity entity, int entityInQueryIndex, ref LootData lootData) =>
           {
               var playerInventory = playerInventoryBuffer[0];
               playerInventory.coins += lootData.coins;
               playerInventoryBuffer[0] = playerInventory;

               bufferConcurrent.DestroyEntity(entityInQueryIndex, entity);

           }).Schedule(Dependency);
        Dependency.Complete();


        Entities.WithAll<WaitingToBeDestroyedTag, PlayerTag>().ForEach((Entity entity, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
        {
            elapsedTime += Time.DeltaTime;
            if (elapsedTime >= 5)
            {
                var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer();
                buffer.RemoveComponent<WaitingToBeDestroyedTag>(entity);
                buffer.RemoveComponent<DyingTag>(entity);
                buffer.RemoveComponent<BusyTag>(entity);
                playerDeathCount++;
                playerHealthBuffer[0] = new HealthData() { hp = 1000 * playerDeathCount * 2 };
                animationData.animationStateConstant = false;
                characterStateData.nextState = CharacterState.Idle;
                elapsedTime = 0;
            }
        }).WithoutBurst().Run();

    }


}
