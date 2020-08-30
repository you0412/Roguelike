using UnityEditor;
using UnityEngine;

public static class BoxCollider2DMenuItem
{
    [MenuItem("CONTEXT/BoxCollider2D/Refit")]
    private static void Refit(MenuCommand command)
    {
        var collider = command.context as BoxCollider2D;
        var renderer = collider.GetComponent<Renderer>();
        var size = renderer.bounds.size;

        Undo.RecordObject(collider, "Refit");
        collider.size = size;
    }
}