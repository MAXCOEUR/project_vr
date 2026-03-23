using UnityEngine;
using UnityEngine.UI;

namespace Production.Scripts.Scene_Menu
{
    public class PixelBgAnimator : MonoBehaviour
    {
        [Header("Pixel grid")]
        public int columns = 20;
        public int rows = 36;
        public Color pixelColor = new Color(0.22f, 1f, 0.56f, 0.1f);

        RectTransform _rect;
        GameObject[] _pixels;

        void Start()
        {
            _rect = GetComponent<RectTransform>();
            _pixels = new GameObject[columns * rows];

            for (int i = 0; i < _pixels.Length; i++)
            {
                var go = new GameObject("px", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(transform, false);

                var rt = go.GetComponent<RectTransform>();
                float cellW = 1f / columns;
                float cellH = 1f / rows;
                int col = i % columns;
                int row = i / columns;

                rt.anchorMin = new Vector2(col * cellW, row * cellH);
                rt.anchorMax = new Vector2((col + 1) * cellW, (row + 1) * cellH);
                rt.offsetMin = rt.offsetMax = Vector2.one * 1f; // 1px gap

                go.GetComponent<Image>().color = Color.clear;
                _pixels[i] = go;
            }

            InvokeRepeating(nameof(FlickerPixel), 0f, 0.08f);
        }

        void FlickerPixel()
        {
            int idx = Random.Range(0, _pixels.Length);
            var img = _pixels[idx].GetComponent<Image>();
            float a = Random.value < 0.07f ? Random.Range(0.05f, 0.4f) : 0f;
            img.color = new Color(pixelColor.r, pixelColor.g, pixelColor.b, a);
        }
    }
}

