namespace UnityEngine.Rendering.CustomRenderPipeline
{
    [CreateAssetMenu]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline(this);
        }
    }
}


