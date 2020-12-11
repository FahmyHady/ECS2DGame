using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;


public class UpdateUISystem : SystemBase
{
    Entity player;
    HealthData healthData;
    InventoryData inventoryData;
    protected override void OnCreate()
    {
        base.OnCreate();

    }
    protected override void OnUpdate()
    {
        player = GetSingletonEntity<PlayerTag>();
        healthData = GetBufferFromEntity<HealthData>(true)[player][0];
        inventoryData = GetBufferFromEntity<InventoryData>(true)[player][0];
        EntitySpawner.Instance?.UpdatePlayerHp(Mathf.Clamp(healthData.hp, 0, int.MaxValue));
        EntitySpawner.Instance?.UpdatePlayerCoins(inventoryData.coins);
    }
}