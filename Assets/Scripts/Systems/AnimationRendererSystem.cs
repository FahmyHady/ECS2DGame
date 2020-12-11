using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
[UpdateAfter(typeof(AnimationSetupFrameSystem))]
public class AnimationRendererSystem : SystemBase
{
    Camera camera;
    AnimationsContainer container;
    struct RenderData : IComparable<RenderData>
    {
        public float Yposition;
        public Matrix4x4 matrix;
        public Vector4 uv;
        public int CompareTo(RenderData other)
        {
            return this.Yposition.CompareTo(other.Yposition);
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        container = AnimationsContainer.Instance;
        camera = container.cameraToUse;
    }
    [BurstCompile]
    struct SortingJob : IJob
    {
        public NativeArray<RenderData> arrayToSort;

        public void Execute()
        {

            for (int i = 0; i < arrayToSort.Length; i++)
            {
                for (int j = 0; j < arrayToSort.Length; j++)
                {
                    if (arrayToSort[i].Yposition < arrayToSort[j].Yposition)
                    {
                        var temp = arrayToSort[i];
                        arrayToSort[i] = arrayToSort[j];
                        arrayToSort[j] = temp;
                    }
                }
            }
        }

    }
    [BurstCompile]
    struct FillArraysJob : IJobParallelFor
    {
        public NativeArray<RenderData> renderDatas;
        [NativeDisableContainerSafetyRestriction] public NativeArray<Matrix4x4> matrix4X4s;
        [NativeDisableContainerSafetyRestriction] public NativeArray<Vector4> uvs;
        public int startIndex;
        public void Execute(int index)
        {
            RenderData data = renderDatas[index];
            matrix4X4s[startIndex + index] = data.matrix;
            uvs[startIndex + index] = data.uv;
        }
    }
    struct NativeQueueToArrayJob : IJob
    {
        public NativeArray<RenderData> nativeArray;
        public NativeQueue<RenderData> nativeQueue;

        public void Execute()
        {
            int index = 0;
            RenderData entity;
            while (nativeQueue.TryDequeue(out entity))
            {
                nativeArray[index] = entity;
                index++;
            }
        }
    }

    protected override void OnUpdate()
    {
        EntityQuery entityQuery = GetEntityQuery(typeof(AnimationData));
        NativeArray<AnimationData> animationDatas = entityQuery.ToComponentDataArray<AnimationData>(Allocator.TempJob);
        NativeQueue<RenderData>[] nativeQueueArray = new NativeQueue<RenderData>[20];
        for (int i = 0; i < nativeQueueArray.Length; i++)
        {
            nativeQueueArray[i] = new NativeQueue<RenderData>(Allocator.TempJob);
        }
        float3 cameraPos = camera.transform.position;
        float cameraWidth = camera.aspect * camera.orthographicSize;
        float marginX = cameraWidth / 10f;
        float xMin = cameraPos.x - cameraWidth - marginX;
        float xMax = cameraPos.x + cameraWidth + marginX;
        float cameraSliceSize = camera.orthographicSize * 2 / 20;
        float yBottom = cameraPos.y - camera.orthographicSize;//Bottom Cull Position
        float yTop1 = cameraPos.y + camera.orthographicSize;//Top Cull Position


        float yTop2 = yTop1 - cameraSliceSize * 1f;
        float yTop3 = yTop1 - cameraSliceSize * 2f;
        float yTop4 = yTop1 - cameraSliceSize * 3f;
        float yTop5 = yTop1 - cameraSliceSize * 4f;
        float yTop6 = yTop1 - cameraSliceSize * 5f;
        float yTop7 = yTop1 - cameraSliceSize * 6f;
        float yTop8 = yTop1 - cameraSliceSize * 7f;
        float yTop9 = yTop1 - cameraSliceSize * 8f;
        float yTop10 = yTop1 - cameraSliceSize * 9f;
        float yTop11 = yTop1 - cameraSliceSize * 10f;
        float yTop12 = yTop1 - cameraSliceSize * 11f;
        float yTop13 = yTop1 - cameraSliceSize * 12f;
        float yTop14 = yTop1 - cameraSliceSize * 13f;
        float yTop15 = yTop1 - cameraSliceSize * 14f;
        float yTop16 = yTop1 - cameraSliceSize * 15f;
        float yTop17 = yTop1 - cameraSliceSize * 16f;
        float yTop18 = yTop1 - cameraSliceSize * 17f;
        float yTop19 = yTop1 - cameraSliceSize * 18f;
        float yTop20 = yTop1 - cameraSliceSize * 19f;

        float marginY = camera.orthographicSize / 10f;
        yTop1 += marginY;
        yBottom -= marginY;

        NativeQueue<RenderData>.ParallelWriter writer0 = nativeQueueArray[0].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer1 = nativeQueueArray[1].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer2 = nativeQueueArray[2].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer3 = nativeQueueArray[3].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer4 = nativeQueueArray[4].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer5 = nativeQueueArray[5].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer6 = nativeQueueArray[6].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer7 = nativeQueueArray[7].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer8 = nativeQueueArray[8].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer9 = nativeQueueArray[9].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer10 = nativeQueueArray[10].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer11 = nativeQueueArray[11].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer12 = nativeQueueArray[12].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer13 = nativeQueueArray[13].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer14 = nativeQueueArray[14].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer15 = nativeQueueArray[15].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer16 = nativeQueueArray[16].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer17 = nativeQueueArray[17].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer18 = nativeQueueArray[18].AsParallelWriter();
        NativeQueue<RenderData>.ParallelWriter writer19 = nativeQueueArray[19].AsParallelWriter();

        Dependency = Entities.ForEach((ref Translation translation, ref AnimationData animationData) =>
          {
              animationData.yPos = translation.Value.y;
          }).Schedule(Dependency);
        Dependency.Complete();

        Dependency = Entities.ForEach((ref Translation translation, ref AnimationData animationData) =>
           {
               float yPos = translation.Value.y;
               float xPos = translation.Value.x;
               if (xPos > xMin && xPos < xMax && yPos > yBottom && yPos < yTop1)
               {
                   //valid pos
                   RenderData renderData = new RenderData { matrix = animationData.matrix, Yposition = yPos, uv = animationData.uv };
                   if (yPos < yTop20)
                   {
                       writer19.Enqueue(renderData);
                   }
                   else if (yPos < yTop19)
                   {
                       writer18.Enqueue(renderData);

                   }
                   else if (yPos < yTop18)
                   {
                       writer17.Enqueue(renderData);

                   }
                   else if (yPos < yTop17)
                   {
                       writer16.Enqueue(renderData);

                   }
                   else if (yPos < yTop16)
                   {
                       writer15.Enqueue(renderData);

                   }
                   else if (yPos < yTop15)
                   {
                       writer14.Enqueue(renderData);

                   }
                   else if (yPos < yTop14)
                   {
                       writer13.Enqueue(renderData);

                   }
                   else if (yPos < yTop13)
                   {
                       writer12.Enqueue(renderData);

                   }
                   else if (yPos < yTop12)
                   {
                       writer11.Enqueue(renderData);

                   }
                   else if (yPos < yTop11)
                   {
                       writer10.Enqueue(renderData);

                   }
                   else if (yPos < yTop10)
                   {
                       writer9.Enqueue(renderData);

                   }
                   else if (yPos < yTop9)
                   {
                       writer8.Enqueue(renderData);

                   }
                   else if (yPos < yTop8)
                   {
                       writer7.Enqueue(renderData);

                   }
                   else if (yPos < yTop7)
                   {
                       writer6.Enqueue(renderData);

                   }
                   else if (yPos < yTop6)
                   {
                       writer5.Enqueue(renderData);

                   }
                   else if (yPos < yTop5)
                   {
                       writer4.Enqueue(renderData);

                   }
                   else if (yPos < yTop4)
                   {
                       writer3.Enqueue(renderData);

                   }
                   else if (yPos < yTop3)
                   {
                       writer2.Enqueue(renderData);

                   }
                   else if (yPos < yTop2)
                   {
                       writer1.Enqueue(renderData);

                   }
                   else
                   {
                       writer0.Enqueue(renderData);
                   }
               }

           }).Schedule(Dependency);
        Dependency.Complete();

        NativeArray<RenderData>[] arrayOfRenderDataNativeArray = new NativeArray<RenderData>[20];
        for (int i = 0; i < 20; i++)
        {
            arrayOfRenderDataNativeArray[i] = new NativeArray<RenderData>(nativeQueueArray[i].Count, Allocator.TempJob);
        }
        NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(20, Allocator.TempJob);
        for (int i = 0; i < 20; i++)
        {
            NativeQueueToArrayJob nativeQueueToArrayJob = new NativeQueueToArrayJob()
            {
                nativeArray = arrayOfRenderDataNativeArray[i],
                nativeQueue = nativeQueueArray[i]
            };
            jobHandles[i] = nativeQueueToArrayJob.Schedule();
        }
        JobHandle.CompleteAll(jobHandles);
        for (int i = 0; i < jobHandles.Length; i++)
        {
            jobHandles[i] = arrayOfRenderDataNativeArray[i].SortJob(jobHandles[i]);
        }
        JobHandle.CompleteAll(jobHandles);

        int totalVisibleUnits = arrayOfRenderDataNativeArray.Sum((NativeArray) => NativeArray.Length);
        NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(totalVisibleUnits, Allocator.TempJob);
        NativeArray<Vector4> uvs = new NativeArray<Vector4>(totalVisibleUnits, Allocator.TempJob);

        int startingIndex = 0;
        for (int i = 0; i < 20; i++)
        {
            FillArraysJob fillArraysJob = new FillArraysJob()
            {
                renderDatas = arrayOfRenderDataNativeArray[i],
                matrix4X4s = matrices,
                uvs = uvs,
                startIndex = startingIndex

            };
            startingIndex += arrayOfRenderDataNativeArray[i].Length;
            jobHandles[i] = fillArraysJob.Schedule(arrayOfRenderDataNativeArray[i].Length, 10);
        }
        JobHandle.CompleteAll(jobHandles);
        for (int i = 0; i < 20; i++)
        {
            nativeQueueArray[i].Dispose();
            arrayOfRenderDataNativeArray[i].Dispose();
        }
        int sliceMaxCount = 1023;
        Matrix4x4[] matricesArray = new Matrix4x4[sliceMaxCount];
        Vector4[] uvsArray = new Vector4[sliceMaxCount];

        Mesh quadMesh = container.quad;
        Material mat = container.playerMaterial;
        int shaderPropertyId = Shader.PropertyToID("_MainTex_UV");
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        for (int i = 0; i < totalVisibleUnits; i += sliceMaxCount)
        {
            int sliceSize = math.min(sliceMaxCount, totalVisibleUnits - i);
            NativeArray<Matrix4x4>.Copy(matrices, i, matricesArray, 0, sliceSize);
            NativeArray<Vector4>.Copy(uvs, i, uvsArray, 0, sliceSize);
            materialPropertyBlock.SetVectorArray(shaderPropertyId, uvsArray);
            Graphics.DrawMeshInstanced(quadMesh, 0, mat, matricesArray, sliceSize, materialPropertyBlock);
        }
        jobHandles.Dispose();
        animationDatas.Dispose();
        uvs.Dispose();
        matrices.Dispose();
        Dependency.Complete();

    }

}

