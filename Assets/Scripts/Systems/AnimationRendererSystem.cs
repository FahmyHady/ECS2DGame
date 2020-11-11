using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
[UpdateAfter(typeof(AnimationStateUpdateSystem))]
public class AnimationRendererSystem : SystemBase
{
    AnimationsContainer container;
    protected override void OnCreate()
    {
        base.OnCreate();
        container = AnimationsContainer.Instance;
    }
    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;
        Entities.WithAll<CharacterTag>().ForEach((ref AnimationData animationData,ref EntityTypeData entityTypeData ,ref CharacterStateData characterStateData, ref NonUniformScale scale, ref ResizingData resizingData, in RenderMesh renderMesh) =>
        {
            animationData.frameTimer += deltaTime;
            if (animationData.frameTimer >= animationData.maxFrameTime)
            {
                ResizeIfNeeded(ref resizingData, ref scale);
                animationData.frameTimer -= animationData.maxFrameTime;
                AnimationSheet sheetToUse = container.GetAnimationSheet(entityTypeData.entityType, characterStateData.characterType, characterStateData.currentState);
                renderMesh.material.SetTexture("_MainTex", sheetToUse.animationTextures[animationData.currentFrame]);
                if (!animationData.looping && animationData.currentFrame + 1 == animationData.totalFrames)
                {
                    characterStateData.goBackToIdle = true;
                }
                else
                {
                    animationData.currentFrame = (animationData.currentFrame + 1) % animationData.totalFrames;
                }
            }
        }).WithoutBurst().Run();

    }

    private void ResizeIfNeeded(ref ResizingData resizingData, ref NonUniformScale scale)
    {
        if (resizingData.needsResizing)
        {
            resizingData.needsResizing = false;

            float3 sheetScale = new float3(resizingData.xScale, 1, 1) * resizingData.scalingFactor;
            scale.Value = sheetScale;
        }
    }
}

