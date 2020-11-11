//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Entities;
//using Unity.Rendering;
//using Unity.Transforms;
//using System;

//public class TriggerCallsSystem : SystemBase
//{
//    EndSimulationEntityCommandBufferSystem _EndSimulationEntityCommandBufferSystem;
//    public Action basicAttacktriggerAction;
//    protected override void OnCreate()
//    {
//        base.OnCreate();
//        _EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//    }
//    protected override void OnUpdate()
//    {
//        var buffer = _EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
//        Dependency = Entities.WithAll<TriggerTag>().ForEach((Entity entity, int entityInQueryIndex, ref AnimationData animationData) =>
//        {
//            switch (animationData.currentState)
//            {
//                case AnimationState.Attacking:
//                    if (animationData.currentFrame==animationData.triggerData.triggerFrame)
//                    {
//                        //buffer.CreateEntity(entityInQueryIndex,);
//                    }
//                    break;
//                case AnimationState.Hit:
//                    break;
//                default:
//                    break;
//            }
//            buffer.RemoveComponent<TriggerTag>(entityInQueryIndex, entity);
//        }).Schedule(Dependency);
//    }

//    void CreateBullet()
//    {
//        EntityArchetype type = EntityManager.CreateArchetype(typeof(LocalToWorld), typeof(Translation), typeof(RenderMesh), typeof(RenderBounds), typeof(NonUniformScale), typeof(AnimationData), typeof(ResizingData));

//    }
//}
