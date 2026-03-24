using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public enum Type { Tree, Rock }
    public Type type;

    public bool isOccupied = false;

    void Start()
    {
        if (type == Type.Tree)
            GameManager.Instance.trees.Add(gameObject);
        else
            GameManager.Instance.rocks.Add(gameObject);
    }
}