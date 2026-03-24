using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public enum Type { Tree, Rock }
    public Type type;

    void Start()
    {
        if (type == Type.Tree)
            GameManager.Instance.trees.Add(gameObject);
        else
            GameManager.Instance.rocks.Add(gameObject);
    }
}