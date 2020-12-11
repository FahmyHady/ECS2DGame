using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
[UpdateAfter(typeof(AnimationStateUpdateSystem))]

public class AnimationSetupFrameSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var bufferConcurrent = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        float deltaTime = Time.DeltaTime;
        Dependency = Entities.WithAll<AnimatedTag>().WithNone<WaitingToBeDestroyedTag>().ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
                {
                    animationData.frameTimer += deltaTime;
                    if (animationData.frameTimer >= animationData.maxFrameTime)
                    {
                        animationData.frameTimer -= animationData.maxFrameTime;
                        if (!characterStateData.goBackToIdle && !animationData.looping && animationData.currentFrame + 1 == animationData.totalFrames)
                        {
                            switch (characterStateData.currentState)
                            {
                                case CharacterState.Attacking:
                                    characterStateData.goBackToIdle = true;
                                    break;
                                case CharacterState.SpecialAttack:
                                    characterStateData.goBackToIdle = true;
                                    break;
                                case CharacterState.Dying:
                                    bufferConcurrent.AddComponent<WaitingToBeDestroyedTag>(entityInQueryIndex, entity);
                                    break;
                            }
                        }
                        else
                        {
                            animationData.currentFrame = (animationData.currentFrame + 1) % animationData.totalFrames;
                        }
                        float uvOffsetX = animationData.uvWidth * ((animationData.currentFrame + animationData.xZeroOffset) % animationData.totalXSteps);
                        int yOffset = (animationData.currentFrame + animationData.xZeroOffset) / animationData.totalXSteps;
                        float uvOffsetY = animationData.uvHeight * Mathf.Abs(yOffset - animationData.yZeroOffset);
                        animationData.uv = new Vector4(animationData.uvWidth, animationData.uvHeight, uvOffsetX, uvOffsetY);
                        animationData.matrix = Matrix4x4.TRS(translation.Value, quaternion.identity, animationData.scale);
                    }
                }).ScheduleParallel(Dependency);
    }

}

