using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
public class SpawnerSystem : SystemBase
{
    EntityArchetype enemyType;
    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
    EntityArchetype playerType;
    EntityManager entityManager;
    EntitySpawner spawner;
    protected override void OnCreate()
    {

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        spawner = EntitySpawner.Instance;
        playerType = entityManager.CreateArchetype(
           typeof(LocalToWorld),
           typeof(Translation),
           typeof(HealthData),
           typeof(AnimationData),
           typeof(PlayerTag),
           typeof(CharacterStateData),
           typeof(EntityTypeData),
           typeof(CharacterTag),
           typeof(AnimatedTag),
           typeof(NeedsIntializingTag),
           typeof(InventoryData)
           );
        enemyType = entityManager.CreateArchetype(
          typeof(LocalToWorld),
          typeof(Translation),
          typeof(PositionToMoveToData),
          typeof(AnimationData),
          typeof(EnemyTag),
          typeof(CharacterStateData),
          typeof(EntityTypeData),
          typeof(CharacterTag),
          typeof(AnimatedTag),
          typeof(NeedsIntializingTag),
          typeof(AttackTimerData),
          typeof(LootData),
          typeof(StrengthData)
          );
        CreatePlayer(entityManager, playerType);
        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }
    protected override void OnUpdate()
    {
        var bufferConcurrent = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        bool spawn = Input.GetKeyDown(KeyCode.Return);
        if (spawn)
        {
            for (int i = 0; i < spawner.howManyToSpawn; i++)
            {
                CreateEnemy(entityManager, enemyType,i);
            }
        }
    }
    private void CreateEnemy(EntityManager entityManager, EntityArchetype enemyType,int count)
    {


        Entity myEntity = entityManager.CreateEntity(enemyType);
        entityManager.SetComponentData(myEntity, new Translation()
        {
            Value = new float3(120, 10, 0)
        });
        entityManager.SetComponentData(myEntity, new PositionToMoveToData()
        {
            positionToMoveTo = new float3(UnityEngine.Random.Range(15, 80f), UnityEngine.Random.Range(-15f, 40f),0.1f),
            speed = 20
        }) ;
        int enemyNumberType = UnityEngine.Random.Range(1, 4);
        CharacterType characterType = CharacterType.EnemyOne;
        int lootOnDeath = 0;
        int strengthOfEnemy = 0;
        switch (enemyNumberType)
        {
            case 1:
                characterType = CharacterType.EnemyOne;
                lootOnDeath = UnityEngine.Random.Range(2, 6);
                strengthOfEnemy = UnityEngine.Random.Range(1, 3);
                break;
            case 2:
                characterType = CharacterType.EnemyTwo;
                lootOnDeath = UnityEngine.Random.Range(4, 9);
                strengthOfEnemy = UnityEngine.Random.Range(2, 6);
                break;
            case 3:
                characterType = CharacterType.EnemyThree;
                lootOnDeath = UnityEngine.Random.Range(6, 15);
                strengthOfEnemy = UnityEngine.Random.Range(4, 8);
                break;
        }
        entityManager.SetComponentData(myEntity, new StrengthData()
        {
            strength = strengthOfEnemy

        });
        entityManager.SetComponentData(myEntity, new LootData()
        {
            coins = lootOnDeath

        });
        entityManager.SetComponentData(myEntity, new CharacterStateData()
        {
            characterType = characterType,
            currentState = CharacterState.Walking,
            nextState = CharacterState.Walking,
            goBackToIdle = false

        });
        entityManager.SetComponentData(myEntity, new AttackTimerData()
        {
            elapsedAttackTime = 0,
            timeBetweenAttacks = UnityEngine.Random.Range(3, 8f)

        });

    }
    private void CreatePlayer(EntityManager entityManager, EntityArchetype playerType)
    {
        Entity myEntity = entityManager.CreateEntity(playerType);

        entityManager.SetComponentData(myEntity, new Translation()
        {
            Value = new float3(-5, -12, 0)
        });

        var inventoryBuffer = GetBufferFromEntity<InventoryData>(false)[myEntity];
        inventoryBuffer.Add(new InventoryData() { coins = 0 });
        var healthBuffer = GetBufferFromEntity<HealthData>(false)[myEntity];
        healthBuffer.Add(new HealthData() { hp = 1000 });

    }

}
