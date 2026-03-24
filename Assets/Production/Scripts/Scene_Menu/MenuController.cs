using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Groups")] public CanvasGroup menuGroup;
    public CanvasGroup loadingPanel;
    public CanvasGroup transitionOverlay;

    [Header("Loading UI")] public Image progressFill;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI hintText;

    [Header("Transition")] public Image fullscreenBlack;
    public Animator reticleAnim;

    [Header("Settings")] public string arSceneName = "Scene_AR";
    public float fadeDuration = 0.4f;

    readonly string[] _hints =
    {
        "Initialisation caméra AR...",
        "Chargement assets 3D...",
        "Calibration capteurs...",
        "Compilation shaders...",
        "Presque prêt..."
    };

    void Start()
    {
        Debug.Log(">>> MenuController Start OK");
    }
    
    // Appelé par le Button onClick
    public void OnPlayPressed()
    {
        Debug.Log(">>> OnPlayPressed appelé !");
        StartCoroutine(TransitionToAR());
    }

    IEnumerator TransitionToAR()
    {
        Debug.Log(">>> TransitionToAR démarré");
        Debug.Log("menuGroup = " + menuGroup);
        Debug.Log("loadingPanel = " + loadingPanel);
        Debug.Log("transitionOverlay = " + transitionOverlay);
        Debug.Log("progressFill = " + progressFill);

        // Fade out du menu
        yield return FadeGroup(menuGroup, 1f, 0f, fadeDuration);
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;

        // Fade in du loading panel
        yield return FadeGroup(loadingPanel, 0f, 1f, fadeDuration);

        // Chargement asynchrone
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

        progressFill.fillAmount = 1f;
        progressText.text = "100%";
        yield return new WaitForSeconds(0.6f);

        // Fade in overlay de transition
        yield return FadeGroup(transitionOverlay, 0f, 1f, 0.3f);
        reticleAnim.SetTrigger("Activate");
        yield return new WaitForSeconds(1.2f);

        // Fade au noir final switch scène
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