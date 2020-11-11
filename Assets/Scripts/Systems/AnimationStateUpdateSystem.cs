using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

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
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);
        var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        //If the current animation is marked as non looping this goes back to idle animations after RenderSystem says current animation is done
        Entities.WithAll<BusyTag, CharacterTag>().ForEach((Entity entity, ref CharacterStateData characterStateData) =>
        {
            if (characterStateData.goBackToIdle)
            {
                buffer.RemoveComponent<BusyTag>(entity);
                characterStateData.goBackToIdle = false;
                characterStateData.animationStateConstant = false;
                characterStateData.nextState = CharacterState.Idle;
            }
        }).Schedule();
        //This checks the next animation state and sets the current state to it if it exists
        Entities.WithAll<CharacterTag>().WithNone<BusyTag>().ForEach((Entity entity, ref EntityTypeData entityTypeData, ref AnimationData animationData, ref CharacterStateData characterStateData, ref ResizingData resizingData) =>
       {
           if (attack)
           {
               characterStateData.animationStateConstant = false;
               characterStateData.nextState = CharacterState.Attacking;
           }
           if (!characterStateData.animationStateConstant)
           {
               characterStateData.animationStateConstant = true;
               AnimationSheet sheetToCopy = container.GetAnimationSheet(entityTypeData.entityType, characterStateData.characterType, characterStateData.nextState);
               AddBusyTag(characterStateData.nextState, entity, buffer);
               CheckTriggerData(animationData, entity, buffer);
               SetAnimationStateData(sheetToCopy, ref animationData, ref characterStateData);
               SetResizingData(sheetToCopy.animationTextures[0].width, sheetToCopy.animationTextures[0].height, ref resizingData);
           }

       }).WithoutBurst().Run();
    }


    private void SetAnimationStateData(AnimationSheet sheetToCopy, ref AnimationData animationData, ref CharacterStateData characterStateData)
    {
        animationData.fps = sheetToCopy.animationFps;
        animationData.totalFrames = sheetToCopy.animationTextures.Length;
        animationData.currentFrame = 0;
        animationData.frameTimer = 0;
        animationData.maxFrameTime = 1f / sheetToCopy.animationFps;
        animationData.triggerData.hasTrigger = sheetToCopy.hasTrigger;
        animationData.triggerData.triggerFrame = sheetToCopy.triggerFrame;

        characterStateData.previousState = characterStateData.currentState;
        characterStateData.currentState = characterStateData.nextState;

        characterStateData.nextState = CharacterState.None;
        animationData.looping = sheetToCopy.looping;

    }

    private void SetResizingData(float width, float height, ref ResizingData resizingData)
    {
        //Add Resizing Data
        resizingData.needsResizing = true;
        resizingData.xScale = width * 1f / height;
        resizingData.scalingFactor = container.scaleTex;
    }


    private void CheckTriggerData(AnimationData animationData, Entity entity, EntityCommandBuffer buffer)
    {
        if (animationData.triggerData.hasTrigger)
        {
            buffer.AddComponent<TriggerTag>(entity);
        }
    }
    private void AddBusyTag(CharacterState nextState, Entity entity, EntityCommandBuffer buffer)
    {
        switch (nextState)
        {
            case CharacterState.Attacking:
                buffer.AddComponent<BusyTag>(entity);
                break;
            case CharacterState.Hit:
                buffer.AddComponent<BusyTag>(entity);
                break;
            case CharacterState.Dying:
                buffer.AddComponent<BusyTag>(entity);
                break;
        }
    }
}
