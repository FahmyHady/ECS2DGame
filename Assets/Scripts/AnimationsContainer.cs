
using UnityEngine;

[System.Serializable]
public class AnimationSheet
{
    public float widthForResizing;
    public float heightForResizing;
    public int totalFrameCount;
    public int animationFps;
    public int xZeroOffset;
    public int yZeroOffset;
    public bool looping;
    public bool hasTrigger;
    public int triggerFrame;
}
public class AnimationsContainer : Singleton<AnimationsContainer>
{
    public Camera cameraToUse;



    public Vector2 totalStepsXY;
    public Material playerMaterial;
    [Header("Player Animations Data")]
    public AnimationSheet player_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet player_StaffAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet player_SpecialAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet player_DeathAnimationSheet = new AnimationSheet();

    [Header("Spitter Animations Data")]
    public AnimationSheet enemyOne_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_BasicAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_WalkAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyOne_DeathAnimationSheet = new AnimationSheet();
    [Header("Chomper Animations Data")]
    public AnimationSheet enemyTwo_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyTwo_BasicAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyTwo_WalkAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyTwo_DeathAnimationSheet = new AnimationSheet();
    
    [Header("Gunner Animations Data")]
    public AnimationSheet enemyThree_IdleAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyThree_BasicAttackAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyThree_WalkAnimationSheet = new AnimationSheet();
    public AnimationSheet enemyThree_DeathAnimationSheet = new AnimationSheet();
    [Header("Effects Animations Data")]
    public AnimationSheet smallExplosionEffect = new AnimationSheet();
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
    public AnimationSheet GetAnimationSheet(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.Effect:
                return smallExplosionEffect;
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
            case CharacterState.SpecialAttack:
                return player_SpecialAttackAnimationSheet;
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
                return GetEnemyTwoSheet(nextState);
            case CharacterType.EnemyThree:
                return GetEnemyThreeSheet(nextState);
        }
        return null;
    }

    AnimationSheet GetEnemyOneSheet(CharacterState nextState)
    {
        switch (nextState)
        {
            case CharacterState.Idle:
                return enemyOne_IdleAnimationSheet;     
            case CharacterState.Walking:
                return enemyOne_WalkAnimationSheet;
            case CharacterState.Attacking:
                return enemyOne_BasicAttackAnimationSheet;
            case CharacterState.Dying:
                return enemyOne_DeathAnimationSheet;

        }
        return null;
    }
    AnimationSheet GetEnemyTwoSheet(CharacterState nextState)
    {
        switch (nextState)
        {
            case CharacterState.Idle:
                return enemyTwo_IdleAnimationSheet;
            case CharacterState.Attacking:
                return enemyTwo_BasicAttackAnimationSheet;
            case CharacterState.Walking:
                return enemyTwo_WalkAnimationSheet;
            case CharacterState.Dying:
                return enemyTwo_DeathAnimationSheet;

        }
        return null;
    }
    AnimationSheet GetEnemyThreeSheet(CharacterState nextState)
    {
        switch (nextState)
        {
            case CharacterState.Idle:
                return enemyThree_IdleAnimationSheet;
            case CharacterState.Attacking:
                return enemyThree_BasicAttackAnimationSheet;
            case CharacterState.Walking:
                return enemyThree_WalkAnimationSheet;
            case CharacterState.Dying:
                return enemyThree_DeathAnimationSheet;

        }
        return null;
    }
}
