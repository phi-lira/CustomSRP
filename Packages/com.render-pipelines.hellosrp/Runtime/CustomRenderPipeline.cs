namespace UnityEngine.Rendering.CustomRenderPipeline
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private ShaderTagId m_ShaderTagId = new ShaderTagId("BasePass");
        bool m_IsStereoSupported = false;

        public CustomRenderPipeline(CustomRenderPipelineAsset asset)
        {

        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            FilteringSettings opaqueFilteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            FilteringSettings transparentFilteringSettings = new FilteringSettings(RenderQueueRange.transparent);

            bool enableDynamicBatching = false;
            bool enableInstancing = false;
            PerObjectData perObjectData = PerObjectData.None;

            foreach (Camera camera in cameras)
            {
#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView)
                    ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
#endif

                // Culling. Adjust culling parameters for your needs. One could enable/disable
                // per-object lighting or control shadow caster distance.
                camera.TryGetCullingParameters(m_IsStereoSupported, out var cullingParameters);
                var cullingResults = context.Cull(ref cullingParameters);

                SortingSettings opaqueSortingSettings = new SortingSettings(camera);
                opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

                // ShaderTagId must match the "LightMode" tag inside the shader pass.
                // If not "LightMode" tag is found the object won't render.
                DrawingSettings opaqueDrawingSettings = new DrawingSettings(m_ShaderTagId, opaqueSortingSettings);
                opaqueDrawingSettings.enableDynamicBatching = enableDynamicBatching;
                opaqueDrawingSettings.enableInstancing = enableInstancing;
                opaqueDrawingSettings.perObjectData = perObjectData;

                // Helper method to setup some per-camera shader constants and camera matrices.
                context.SetupCameraProperties(camera, m_IsStereoSupported);

                // Sets active render target and clear based on camera backgroud color.
                var cmd = CommandBufferPool.Get();
                cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
                cmd.ClearRenderTarget(true, true, camera.backgroundColor);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);

                // Render Opaque objects given the filtering and settings computed above.
                // This functions will sort and batch objects.
                context.DrawRenderers(cullingResults, ref opaqueDrawingSettings, ref opaqueFilteringSettings);

                // Renders skybox if required
                if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
                    context.DrawSkybox(camera);

                // Submit commands to GPU. Up to this point all commands have been enqueued in the context.
                // Several submits can be done in a frame to better controls CPU/GPU workload.
                context.Submit();
            }
        }
    }

}
