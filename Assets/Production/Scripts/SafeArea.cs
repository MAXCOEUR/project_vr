using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2 _lastScreenSize = new Vector2(0, 0);
    private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        Refresh();
    }

    void Update()
    {
        // On vérifie si l'écran a changé (rotation ou redimensionnement)
        if (_lastSafeArea != Screen.safeArea || 
            _lastScreenSize.x != Screen.width || _lastScreenSize.y != Screen.height || 
            _lastOrientation != Screen.orientation)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        Rect safeArea = Screen.safeArea;

        // On sauvegarde l'état actuel pour la prochaine vérification
        _lastSafeArea = safeArea;
        _lastScreenSize.x = Screen.width;
        _lastScreenSize.y = Screen.height;
        _lastOrientation = Screen.orientation;

        // Calcul des ancres normalisées (entre 0 et 1)
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Application aux ancres du RectTransform
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}