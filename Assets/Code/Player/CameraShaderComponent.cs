using UnityEngine;

namespace Game.Utils
{
    public sealed class CameraShaderComponent : MonoBehaviour
    {
        [Header("Effects (applied in order)")]
        [SerializeField] private Material vignette;
        [SerializeField] private Material dithering;
        [SerializeField] private Material blur;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (vignette == null && dithering == null && blur == null)
            {
                Graphics.Blit(src, dest);
                return;
            }

            RenderTexture temp1 = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
            RenderTexture temp2 = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

            RenderTexture currentSrc = src;
            RenderTexture currentDest = temp1;

            if (vignette != null)
            {
                Graphics.Blit(currentSrc, currentDest, vignette);
                currentSrc = currentDest;
                currentDest = (currentDest == temp1) ? temp2 : temp1;
            }

            if (dithering != null)
            {
                Graphics.Blit(currentSrc, currentDest, dithering);
                currentSrc = currentDest;
                currentDest = (currentDest == temp1) ? temp2 : temp1;
            }

            if (blur != null)
            {
                Graphics.Blit(currentSrc, currentDest, blur);
                currentSrc = currentDest;
            }

            Graphics.Blit(currentSrc, dest);

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }
    }
}