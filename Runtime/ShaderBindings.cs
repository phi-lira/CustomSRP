namespace UnityEngine.Rendering.CustomRenderPipeline
{
    public static class ShaderPassTag
    {
        public static ShaderTagId forwardLit = new ShaderTagId("ForwardLit");
    }

    public static class ShaderBindings
    {
        const string kPerFrameShaderVariablesTag = "SetPerFrameShaderVariables";
        const string kPerCameraShaderVariablesTag = "SetPerCameraShaderVariables";

        // Time constants
        public static int time = Shader.PropertyToID("_Time");
        public static int sinTime = Shader.PropertyToID("_SinTime");
        public static int cosTime = Shader.PropertyToID("_CosTime");
        public static int deltaTime = Shader.PropertyToID("unity_DeltaTime");

        // Ambient and Fog constants
        public static int ambientSky = Shader.PropertyToID("unity_AmbientSky");
        public static int ambientEquator = Shader.PropertyToID("unity_AmbientEquator");
        public static int ambientGround = Shader.PropertyToID("unity_AmbientGround");
        public static int fogColor = Shader.PropertyToID("unity_FogColor");
        public static int fogParams = Shader.PropertyToID("unity_FogParams");

        // Camera and Screen
        public static int worldSpaceCameraPos = Shader.PropertyToID("_WorldSpaceCameraPos");
        public static int projectionParams = Shader.PropertyToID("_ProjectionParams");
        public static int screenParams = Shader.PropertyToID("_ScreenParams");
        public static int zBufferParams = Shader.PropertyToID("_ZBufferParams");
        public static int orthoParams = Shader.PropertyToID("unity_OrthoParams");
        public static int cameraProjection = Shader.PropertyToID("unity_CameraProjection");
        public static int cameraInvProjection = Shader.PropertyToID("unity_CameraInvProjection");
        public static int cameraWorldClipPlanes = Shader.PropertyToID("unity_CameraWorldClipPlanes");

        // Transformations
        public static int matrixV = Shader.PropertyToID("unity_MatrixV");
        public static int matrixP = Shader.PropertyToID("unity_MatrixP");
        public static int matrixVP = Shader.PropertyToID("unity_MatrixVP");

        public static int matrixIV = Shader.PropertyToID("unity_MatrixInvV");
        public static int matrixIP = Shader.PropertyToID("unity_MatrixInvP");
        public static int matrixIVP = Shader.PropertyToID("unity_MatrixInvVP");

        public static void SetPerFrameShaderVariables(ScriptableRenderContext context)
        {
#if UNITY_EDITOR
            float time = Application.isPlaying ? Time.time : Time.realtimeSinceStartup;
#else
            float time = Time.time;
#endif
            float deltaTime = Time.deltaTime;
            float smoothDeltaTime = Time.smoothDeltaTime;

            float timeEights = time / 8f;
            float timeFourth = time / 4f;
            float timeHalf = time / 2f;

            // Time values
            Vector4 timeVector = time * new Vector4(1f / 20f, 1f, 2f, 3f);
            Vector4 sinTimeVector = new Vector4(Mathf.Sin(timeEights), Mathf.Sin(timeFourth), Mathf.Sin(timeHalf), Mathf.Sin(time));
            Vector4 cosTimeVector = new Vector4(Mathf.Cos(timeEights), Mathf.Cos(timeFourth), Mathf.Cos(timeHalf), Mathf.Cos(time));
            Vector4 deltaTimeVector = new Vector4(deltaTime, 1f / deltaTime, smoothDeltaTime, 1f / smoothDeltaTime);

            CommandBuffer cmd = CommandBufferPool.Get(kPerFrameShaderVariablesTag);
            cmd.SetGlobalVector(ShaderBindings.time, timeVector);
            cmd.SetGlobalVector(ShaderBindings.sinTime, sinTimeVector);
            cmd.SetGlobalVector(ShaderBindings.cosTime, cosTimeVector);
            cmd.SetGlobalVector(ShaderBindings.deltaTime, deltaTimeVector);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public static void SetPerCameraShaderVariables(ScriptableRenderContext context, Camera camera)
        {
            Rect pixelRect = camera.pixelRect;
            float cameraWidth = (float)pixelRect.width;
            float cameraHeight = (float)pixelRect.height;

            Matrix4x4 projMatrix = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false);
            Matrix4x4 viewMatrix = camera.worldToCameraMatrix;
            Matrix4x4 viewProjMatrix = projMatrix * viewMatrix;

            Matrix4x4 invViewMatrix = Matrix4x4.Inverse(viewMatrix);
            Matrix4x4 invProjectionMatrix = Matrix4x4.Inverse(projMatrix);
            Matrix4x4 invViewProjMatrix = Matrix4x4.Inverse(viewProjMatrix);

            float projectionFlip = SystemInfo.graphicsUVStartsAtTop ? -1.0f : 1.0f;

            float near = camera.nearClipPlane;
            float far = camera.farClipPlane;
            float invNear = Mathf.Approximately(near, 0.0f) ? 0.0f : 1.0f / near;
            float invFar = Mathf.Approximately(far, 0.0f) ? 0.0f : 1.0f / far;
            float isOrthographic = camera.orthographic ? 1.0f : 0.0f;

            // From http://www.humus.name/temp/Linearize%20depth.txt
            // But as depth component textures on OpenGL always return in 0..1 range (as in D3D), we have to use
            // the same constants for both D3D and OpenGL here.
            // OpenGL would be this:
            // zc0 = (1.0 - far / near) / 2.0;
            // zc1 = (1.0 + far / near) / 2.0;
            // D3D is this:
            float zc0 = 1.0f - far * invNear;
            float zc1 = far * invNear;

            Vector4 zBufferParams = new Vector4(zc0, zc1, zc0 * invFar, zc1 * invFar);

            if (SystemInfo.usesReversedZBuffer)
            {
                zBufferParams.y += zBufferParams.x;
                zBufferParams.x = -zBufferParams.x;
                zBufferParams.w += zBufferParams.z;
                zBufferParams.z = -zBufferParams.z;
            }

            Vector4 projectionParams = new Vector4(projectionFlip, near, far, 1.0f * invFar);
            Vector4 orthoParams = new Vector4(camera.orthographicSize * camera.aspect, camera.orthographicSize, 0.0f, isOrthographic);

            CommandBuffer cmd = CommandBufferPool.Get(kPerCameraShaderVariablesTag);
            cmd.SetGlobalVector(ShaderBindings.worldSpaceCameraPos, camera.transform.position);
            cmd.SetGlobalVector(ShaderBindings.projectionParams, projectionParams);
            cmd.SetGlobalVector(ShaderBindings.screenParams, new Vector4(cameraWidth, cameraHeight, 1.0f + 1.0f / cameraWidth, 1.0f + 1.0f / cameraHeight));
            cmd.SetGlobalVector(ShaderBindings.zBufferParams, zBufferParams);
            cmd.SetGlobalVector(ShaderBindings.orthoParams, orthoParams);
            // TODO: missing unity_CameraWorldClipPlanes[6], currently set by context.SetupCameraProperties

            cmd.SetGlobalMatrix(ShaderBindings.matrixV, viewMatrix);
            cmd.SetGlobalMatrix(ShaderBindings.matrixP, projMatrix);
            cmd.SetGlobalMatrix(ShaderBindings.matrixVP, viewProjMatrix);

            cmd.SetGlobalMatrix(ShaderBindings.matrixIV, invViewMatrix);
            cmd.SetGlobalMatrix(ShaderBindings.matrixIP, viewMatrix);
            cmd.SetGlobalMatrix(ShaderBindings.matrixIVP, invViewProjMatrix);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}