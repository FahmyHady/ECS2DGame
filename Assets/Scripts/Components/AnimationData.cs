using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AnimationData : IComponentData
{
    public AnimationTriggerData triggerData;
    public int currentFrame;
    public int totalFrames;
    public int fps;
    public float frameTimer;
    public float maxFrameTime;
    public bool looping;
    //Animated Object datas
}
public struct EffectData : IComponentData
{
    public EffectType type;
    public bool animationStateConstant;
}
public struct CharacterStateData : IComponentData
{
    public CharacterType characterType;
    public CharacterState currentState;
    public CharacterState nextState;
    public CharacterState previousState;
    public bool animationStateConstant;
    public bool goBackToIdle;
}
public struct EntityTypeData : IComponentData
{
    public EntityType entityType;
}

public struct AnimationTriggerData
{
    public bool hasTrigger;
    public int triggerFrame;
}
public struct ResizingData : IComponentData
{
    public bool needsResizing;
    public float xScale;
    public float scalingFactor;
}