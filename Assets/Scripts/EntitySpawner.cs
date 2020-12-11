using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EntitySpawner : Singleton<EntitySpawner>
{
    public Text playerHpText;
    public Text playerCoinsText;
    public int howManyToSpawn;
    EntityArchetype smallExplosionType;
    EntityArchetype playerType;
    EntityArchetype enemyType;
    EntityManager entityManager;
    private void Start()
    {
        _instance = this;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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
           typeof(NeedsIntializingTag)
           );
        enemyType = entityManager.CreateArchetype(
           typeof(LocalToWorld),
           typeof(Translation),
           typeof(AnimationData),
           typeof(EnemyTag),
           typeof(CharacterStateData),
           typeof(EntityTypeData),
           typeof(CharacterTag),
           typeof(AnimatedTag),
           typeof(NeedsIntializingTag)

           );
        smallExplosionType = entityManager.CreateArchetype(
           typeof(LocalToWorld),
           typeof(AnimatedTag),
           typeof(Translation),
           typeof(AnimationData),
           typeof(EntityTypeData),
           typeof(EffectTag),
           typeof(NeedsIntializingTag)

           );


    }

    public void UpdatePlayerHp(int hp)
    {
        playerHpText.text = hp.ToString();
    }   
    public void UpdatePlayerCoins(int coins)
    {
        playerCoinsText.text = coins.ToString();
    }

    public void CreateSmallExplosionEffectRuntime(EntityCommandBuffer buffer, float3 parentTranslation)
    {
        //Entity myEntity = buffer.CreateEntity(smallExplosionType);
        //buffer.SetSharedComponent(myEntity, new RenderMesh()
        //{
        //    mesh = AnimationsContainer.Instance.quad,
        //    material = new Material(AnimationsContainer.Instance.playerMaterial),
        //    layer = 0,
        //});

        //buffer.SetComponent(myEntity, new Translation()
        //{
        //    Value = parentTranslation
        //});

        //buffer.SetComponent(myEntity, new EntityTypeData()
        //{
        //    entityType = EntityType.Effect
        //}
        //);

    }
    private void CreateEnemy(EntityManager entityManager, EntityArchetype enemyType)
    {
        // NativeArray<Entity> entities = new NativeArray<Entity>(howManyToSpawn, Allocator.Temp);
        //entityManager.CreateEntity(enemyType, entities);


        Entity myEntity = entityManager.CreateEntity(enemyType);
        entityManager.SetComponentData(myEntity, new Translation()
        {
            Value = new float3(UnityEngine.Random.Range(-3, 8f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-3f, 3f))
        });
        //entityManager.SetComponentData(myEntity, new Rotation()
        //{
        //    Value = quaternion.RotateY(Mathf.Deg2Rad * 180)
        //});
        int enemyNumberType = UnityEngine.Random.Range(1, 4);
        CharacterType characterType = CharacterType.EnemyOne;
        switch (enemyNumberType)
        {
            case 1:
                characterType = CharacterType.EnemyOne;
                break;
            case 2:
                characterType = CharacterType.EnemyTwo;
                break;
            case 3:
                characterType = CharacterType.EnemyThree;
                break;

        }
        entityManager.SetComponentData(myEntity, new CharacterStateData()
        {

            characterType = characterType
        });

    }
    public void ChangeSpawnNumber(string spawnNumber)
    {
        int.TryParse(spawnNumber, out howManyToSpawn);
    }
    public void CreatePlayer()
    {
        Entity myEntity = entityManager.CreateEntity(playerType);

        entityManager.SetComponentData(myEntity, new Translation()
        {
            Value = new float3(-5, -12, 0)
        });
    

    }

}
