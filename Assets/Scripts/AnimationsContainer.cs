
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class AnimationSheet
{
    public Texture[] animationTextures;
    public int animationFps;
    public bool looping;
    public bool hasTrigger;
    public int triggerFrame;
}
public class AnimationsContainer : Singleton<AnimationsContainer>
{
    public Material playerMaterial;
    [Header("Player Animations Data")]
    public AnimationSheet player_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet player_StaffAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet player_HitAnimationSheet = new AnimationSheet();
    public AnimationSheet player_DeathAnimationSheet = new AnimationSheet();

    [Header("Basic Enemy Animations Data")]
    public AnimationSheet enemyOne_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_BasicAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_HitAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_DeathAnimationSheet = new AnimationSheet();
    [Header("Effects Animations Data")]
    public AnimationSheet basicAttackEffect = new AnimationSheet();
    public Mesh quad;
    public float scaleTex = 3;
    private void Awake()
    {
        _instance = this;
    }
    public AnimationSheet GetAnimationSheet(EntityType entityType, CharacterType characterType, CharacterState nextState)
    {
        switch (entityType)
        {
            case EntityType.Character:
                return GetCharacterSheet(characterType, nextState);
            case EntityType.Effect:
                break;
        }
        return null;
    }

     AnimationSheet GetCharacterSheet(CharacterType characterType, CharacterState nextState)
    {
        if (characterType == CharacterType.Player)
        {

            return GetPlayerSheet(nextState);
        }
        else
        {
            return GetEnemySheet(characterType, nextState);

        }

    }

    AnimationSheet GetPlayerSheet(CharacterState nextState)
    {
        switch (nextState)
        {
            case CharacterState.Idle:
                return player_IdleAnimationSheet;
            case CharacterState.Attacking:
                return player_StaffAttackAnimationSheet;
            case CharacterState.Hit:
                return player_HitAnimationSheet;
            case CharacterState.Dying:
                return player_DeathAnimationSheet;
        }
        return null;
    }
    AnimationSheet GetEnemySheet(CharacterType enemyType, CharacterState nextState)
    {
        switch (enemyType)
        {
            case CharacterType.EnemyOne:
                return GetEnemyOneSheet(nextState);
            case CharacterType.EnemyTwo:
                break;
        }
        return null;
    }

    AnimationSheet GetEnemyOneSheet(CharacterState nextState)
    {
        switch (nextState)
        {
            case CharacterState.Idle:
                return enemyOne_IdleAnimationSheet;
            case CharacterState.Attacking:
                return enemyOne_BasicAttackAnimationSheet;
            case CharacterState.Hit:
                return enemyOne_HitAnimationSheet;
            case CharacterState.Dying:
                return enemyOne_DeathAnimationSheet;

        }
        return null;
    }
}
