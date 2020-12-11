using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using System;
using Unity.Mathematics;

[UpdateBefore(typeof(AnimationRendererSystem))]
public class MoveSystem : SystemBase
{
    //  EntityArchetype smallExplosion;
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        Vector3 currentTranslation = Vector3.zero;
        Dependency = Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, ref PositionToMoveToData positionToMoveToData, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
            {
                bool3 flag = translation.Value != positionToMoveToData.positionToMoveTo;
                if (flag.x && flag.y && flag.z)
                {
                    translation.Value = Vector3.MoveTowards(translation.Value, positionToMoveToData.positionToMoveTo, deltaTime * positionToMoveToData.speed);
                }
                else
                {
                    buffer.RemoveComponent<PositionToMoveToData>(entityInQueryIndex, entity);
                    buffer.RemoveComponent<BusyTag>(entityInQueryIndex, entity);
                    animationData.animationStateConstant = false;
                    characterStateData.nextState = CharacterState.Idle;
                }

            }).Schedule(Dependency);
        Dependency.Complete();
    }

}
