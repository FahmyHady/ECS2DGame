using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
[UpdateAfter(typeof(PlayerInputSystem))]
[AlwaysUpdateSystem]
public class AnimationStateUpdateSystem : SystemBase
{
    AnimationsContainer container;
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        container = AnimationsContainer.Instance;
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        var bufferConcurrent = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        Entities.WithAll<NeedsIntializingTag>().ForEach((Entity entity, ref AnimationData animationData) =>
        {
            animationData.totalXSteps = (int)container.totalStepsXY.x;
            animationData.totalYSteps = (int)container.totalStepsXY.y;
            animationData.uvWidth = 1f / container.totalStepsXY.x;
            animationData.uvHeight = 1f / container.totalStepsXY.y;
            animationData.scalingFactor = container.scaleTex;
            buffer.RemoveComponent<NeedsIntializingTag>(entity);
        }).WithoutBurst().Run();





        //If the current animation is marked as non looping this goes back to idle animations after RenderSystem says current animation is done

        Entities.WithAll<AnimatedTag, CharacterTag>().WithNone<BusyTag, NeedsResizingTag>().ForEach((Entity entity, ref EntityTypeData entityTypeData, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
         {

             if (!animationData.animationStateConstant)
             {
                 animationData.animationStateConstant = true;

                 AnimationSheet sheetToCopy = container.GetAnimationSheet(entityTypeData.entityType, characterStateData.characterType, characterStateData.nextState);
                 SetAnimationStateData(sheetToCopy, ref animationData, ref characterStateData);
                 buffer.AddComponent<NeedsResizingTag>(entity);
             }

         }).WithoutBurst().Run();





        Dependency = Entities.WithAll<AnimatedTag, BusyTag, CharacterTag>().ForEach((Entity entity, int entityInQueryIndex, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
     {
         if (characterStateData.goBackToIdle)
         {
             bufferConcurrent.RemoveComponent<BusyTag>(entityInQueryIndex, entity);
             characterStateData.goBackToIdle = false;
             animationData.animationStateConstant = false;
             characterStateData.nextState = CharacterState.Idle;
         }
     }).ScheduleParallel(Dependency);





        Dependency = Entities.WithAll<AnimatedTag, DyingTag, CharacterTag>().ForEach((Entity entity, int entityInQueryIndex, ref AnimationData animationData, ref CharacterStateData characterStateData) =>
     {
         animationData.animationStateConstant = false;
         characterStateData.nextState = CharacterState.Dying;
     }).ScheduleParallel(Dependency);






        //This checks the next animation state and sets the current state to it if it exists
        Dependency = Entities.WithAll<AnimatedTag, NeedsResizingTag>().ForEach((Entity entity, int entityInQueryIndex, ref AnimationData animationData) =>
          {
              float xScale = animationData.widthForResizing * 1f / animationData.heightForResizing;
              float scalingFactor = animationData.scalingFactor;

              animationData.scale = new float3(xScale, 1, 1) * scalingFactor;
              bufferConcurrent.RemoveComponent<NeedsResizingTag>(entityInQueryIndex, entity);
              if (animationData.triggerData.hasTrigger)
              {
                  bufferConcurrent.AddComponent<TriggerTag>(entityInQueryIndex, entity);
              }

          }).ScheduleParallel(Dependency);






        Dependency = Entities.WithAll<AnimatedTag>().WithNone<BusyTag>().ForEach((Entity entity, int entityInQueryIndex, ref CharacterStateData characterStateData) =>
        {
            switch (characterStateData.currentState)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Attacking:
                    bufferConcurrent.AddComponent<BusyTag>(entityInQueryIndex, entity);
                    break;
                case CharacterState.SpecialAttack:
                    bufferConcurrent.AddComponent<BusyTag>(entityInQueryIndex, entity);
                    break;
                case CharacterState.Dying:
                    bufferConcurrent.AddComponent<BusyTag>(entityInQueryIndex, entity);
                    break;
                case CharacterState.Walking:
                    bufferConcurrent.AddComponent<BusyTag>(entityInQueryIndex, entity);
                    break;
                default:
                    break;
            }

        }).ScheduleParallel(Dependency);
        Dependency.Complete();
    }


    private void SetAnimationStateData(AnimationSheet sheetToCopy, ref AnimationData animationData, ref CharacterStateData characterStateData)
    {
        animationData.fps = sheetToCopy.animationFps;
        animationData.totalFrames = sheetToCopy.totalFrameCount;
        animationData.currentFrame = characterStateData.nextState == CharacterState.Idle ? UnityEngine.Random.Range(0, animationData.totalFrames) : 0;
        animationData.frameTimer = 0;
        animationData.maxFrameTime = 1f / sheetToCopy.animationFps;
        animationData.triggerData.hasTrigger = sheetToCopy.hasTrigger;
        animationData.triggerData.triggerFrame = sheetToCopy.triggerFrame;
        animationData.xZeroOffset = sheetToCopy.xZeroOffset;
        animationData.yZeroOffset = sheetToCopy.yZeroOffset;
        animationData.widthForResizing = sheetToCopy.widthForResizing;
        animationData.heightForResizing = sheetToCopy.heightForResizing;
        characterStateData.previousState = characterStateData.currentState;
        characterStateData.currentState = characterStateData.nextState;

        animationData.looping = sheetToCopy.looping;

    }


}
