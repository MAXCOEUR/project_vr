using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Production.Scripts.Scene_Menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Groups")]
        public CanvasGroup menuGroup;
        public CanvasGroup loadingPanel;
        public CanvasGroup transitionOverlay;

        [Header("Loading UI")]
        public Image progressFill;        // l'Image Filled
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI hintText;

        [Header("Transition")]
        public Image fullscreenBlack;
        public Animator reticleAnim;

        [Header("Settings")]
        public string arSceneName = "Scene_AR";
        public float fadeDuration = 0.4f;

        readonly string[] _hints = {
            "Initialisation caméra AR...",
            "Chargement assets 3D...",
            "Calibration capteurs...",
            "Compilation shaders...",
            "Presque prêt..."
        };

        // Appelé par le Button onClick (drag le MenuManager dans l'Inspector du Button)
        public void OnPlayPressed()
        {
            StartCoroutine(TransitionToAR());
        }

        IEnumerator TransitionToAR()
        {
            // 1. Fade out du menu
            yield return FadeGroup(menuGroup, 1f, 0f, fadeDuration);
            menuGroup.interactable = false;
            menuGroup.blocksRaycasts = false;

            // 2. Fade in du loading panel
            yield return FadeGroup(loadingPanel, 0f, 1f, fadeDuration);

            // 3. Chargement asynchrone
            AsyncOperation op = SceneManager.LoadSceneAsync(arSceneName);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                float pct = op.progress / 0.9f;
                progressFill.fillAmount = pct;
                progressText.text = Mathf.RoundToInt(pct * 100) + "%";
                hintText.text = _hints[Mathf.Min(Mathf.FloorToInt(pct * _hints.Length), _hints.Length - 1)];
                yield return null;
            }

            // Compléter visuellement à 100%
            progressFill.fillAmount = 1f;
            progressText.text = "100%";
            yield return new WaitForSeconds(0.6f);

            // 4. Fade in overlay de transition
            yield return FadeGroup(transitionOverlay, 0f, 1f, 0.3f);
            reticleAnim.SetTrigger("Activate");
            yield return new WaitForSeconds(1.2f);

            // 5. Fade au noir final → switch scène
            float t = 0f;
            Color c = fullscreenBlack.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fullscreenBlack.color = new Color(c.r, c.g, c.b, t / fadeDuration);
                yield return null;
            }

            op.allowSceneActivation = true;
        }

        IEnumerator FadeGroup(CanvasGroup group, float from, float to, float duration)
        {
            float t = 0f;
            group.alpha = from;
            while (t < duration)
            {
                t += Time.deltaTime;
                group.alpha = Mathf.Lerp(from, to, t / duration);
                yield return null;
            }
            group.alpha = to;
        }
    }
}