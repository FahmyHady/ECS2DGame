using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AnimationData : IComponentData
{
    public int totalXSteps; // of all sprite sheet
    public int totalYSteps; // of all sprite sheet
    public float uvWidth ;
    public float uvHeight ;
    public float scalingFactor; //Should be constant for all as well
    public float3 scale; 
    public float widthForResizing;
    public float heightForResizing;
    public AnimationTriggerData triggerData;
    public int currentFrame;
    public int totalFrames;
    public int fps;
    public float frameTimer;
    public float maxFrameTime;
    public bool looping;
    public bool animationStateConstant;
    public int xZeroOffset;
    public int yZeroOffset;
    public Vector4 uv;
    public Matrix4x4 matrix;
    public float yPos;

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
public struct PositionToMoveToData : IComponentData
{
    public float3 positionToMoveTo;
    public float speed;
}
public struct AttackTimerData : IComponentData
{
    public float timeBetweenAttacks;
    public float elapsedAttackTime;
}