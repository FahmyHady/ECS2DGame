using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class SpawnerSystem : SystemBase
{

    protected override void OnUpdate()
    {
    }
    protected override void OnCreate()
    {

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype archetype = entityManager.CreateArchetype(typeof(Translation),
                                                                  typeof(RenderMesh),
                                                                  typeof(LocalToWorld),
                                                                  typeof(RenderBounds),
                                                                  typeof(WorldRenderBounds)
                                                                  );

    }
}
