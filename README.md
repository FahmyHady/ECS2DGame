# ECS2DGame

**A 2020 Unity DOTS experiment in sprite animation, gameplay state, and instanced rendering.**

The central technical study is a rendering pipeline that turns entity animation data into sprite-sheet UVs and transform matrices, culls against an orthographic camera, sorts visible sprites by vertical position, and submits instanced mesh draws.

Built with **Unity 2019.4.2f1** and the preview-era Entities API. This is a historical engineering experiment; its value is in the pipeline and tradeoffs it exposes.

## Animation and rendering pipeline

```text
Input / gameplay state
    → animation selection
    → frame timing, sprite-sheet UVs, and transform matrices
    → camera bounds filtering and 20 vertical buckets
    → per-bucket sorting and render-array assembly
    → Graphics.DrawMeshInstanced batches of up to 1,023 instances
```

The implementation uses `SystemBase`, component data, tag components, dynamic buffers, command buffers, scheduled jobs, and native collections. Burst annotations are present on render-data jobs. Managed scene objects supply animation definitions and the camera.

## Source tour

| File | Responsibility |
| --- | --- |
| [AnimationData](Assets/Scripts/Components/AnimationData.cs) | Per-entity animation state and rendering data |
| [AnimationStateUpdateSystem](Assets/Scripts/Systems/AnimationStateUpdateSystem.cs) | Select animation data and transition gameplay tags |
| [AnimationSetupFrameSystem](Assets/Scripts/Systems/AnimationSetupFrameSystem.cs) | Advance frames, calculate UVs, and handle animation completion |
| [AnimationRendererSystem](Assets/Scripts/Systems/AnimationRendererSystem.cs) | Camera filtering, bucket sorting, job coordination, and instanced drawing |
| [EntitySpawner](Assets/Scripts/EntitySpawner.cs) | Entity construction and initial data |
| [MoveSystem](Assets/Scripts/Systems/MoveSystem.cs) | Movement and its connection to animation state |

The renderer's active sorting path uses `SortJob` on each bucket. The separate nested `SortingJob` implementation is experimental code and is not the sorting job scheduled by that path.

## Open the project

1. Clone the repository and open its root using **Unity 2019.4.2f1**.
2. Restore the versions recorded in [Packages/manifest.json](Packages/manifest.json), including **Entities 0.11.2-preview.1**, **Hybrid Renderer 0.5.2-preview.4**, and **Universal RP 7.3.1**.
3. Open [Assets/Scenes/Main.unity](Assets/Scenes/Main.unity).

This code uses legacy DOTS APIs such as `Translation` and `ToConcurrent`. Moving it to a newer Entities release requires a migration. A fresh import, playable build, and performance benchmark have not been verified for this documentation update.

## Engineering tradeoffs

The project separates animation selection, frame preparation, and rendering into distinct systems, making their responsibilities visible. Its remaining costs are also visible: repeated dependency completion, per-frame allocations, a fixed 20-bucket layout, and managed singleton bridges all constrain scalability.

There is a known movement condition worth addressing before extending the demo: `MoveSystem` continues only when all three position axes differ from the destination, so it can finish early when one axis already matches. A modern revision would correct that condition, tighten job dependencies and buffer lifetimes, then profile alternatives to the current renderer.

No frame-rate or entity-count performance claim is made here. The repository is a source-level case study in assembling a data-oriented animation pipeline.

## Assets and attribution

The engineering focus is [Assets/Scripts](Assets/Scripts). The repository also contains Unity 2D Game Kit material and other supporting assets; those are not presented as original artwork or original sample infrastructure. Retain the terms and notices associated with any third-party content when reusing it.
