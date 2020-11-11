using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
public class EntitySpawner : Singleton<EntitySpawner>
{
    public GameObject quadprefab;
    public int howManyToSpawn;
    public bool ecs;
    private void Start()
    {
        _instance = this;
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype playerType = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(NonUniformScale),
            typeof(AnimationData),
            typeof(ResizingData),
            typeof(PlayerTag),
            typeof(CharacterStateData),
            typeof(EntityTypeData),
            typeof(CharacterTag)
            );
        EntityArchetype enemyType = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(NonUniformScale),
            typeof(AnimationData),
            typeof(ResizingData),
            typeof(EnemyTag),
            typeof(CharacterStateData),
            typeof(EntityTypeData),
            typeof(CharacterTag)
            );

        CreatePlayer(entityManager, playerType);
        CreateEnemy(entityManager, enemyType);
    }

    private void CreateEnemy(EntityManager entityManager, EntityArchetype enemyType)
    {
        Entity myEntity = entityManager.CreateEntity(enemyType);
        entityManager.SetSharedComponentData(myEntity, new RenderMesh()
        {
            mesh = AnimationsContainer.Instance.quad,
            material = new Material(AnimationsContainer.Instance.playerMaterial),
            layer = 0,
        });
        entityManager.SetComponentData(myEntity, new Translation()
        {
            Value = new float3(2, 0, 0)
        });
        entityManager.SetComponentData(myEntity, new Rotation()
        {
            Value =  quaternion.RotateY(Mathf.Deg2Rad * 180)
        });
        entityManager.SetComponentData(myEntity, new CharacterStateData()
        {
            characterType = CharacterType.EnemyOne
        }) ;
    }

    private void CreatePlayer(EntityManager entityManager, EntityArchetype playerType)
    {
        int spawnNumber = (int)Mathf.Sqrt(howManyToSpawn);
        for (int i = 0; i < spawnNumber; i++)
        {
            for (int j = 0; j < spawnNumber; j++)
            {
                if (ecs)
                {
                    Entity myEntity = entityManager.CreateEntity(playerType);
                    entityManager.SetSharedComponentData(myEntity, new RenderMesh()
                    {
                        mesh = AnimationsContainer.Instance.quad,
                        material = new Material(AnimationsContainer.Instance.playerMaterial),
                        layer = 0,
                    });
                    entityManager.SetComponentData(myEntity, new Translation()
                    {
                        Value = new float3(i * 2, j * 2, 0)
                    });
                }
                else
                {
                    Instantiate(quadprefab, new Vector3(i * 2, j * 2, 0), Quaternion.identity);
                }
            }
        }
    }
}
