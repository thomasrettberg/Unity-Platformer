using UnityEngine;

public class Utils {

    /// <summary>
    /// Zeichnet den Boxcollider.
    /// </summary>
    /// <param name="mb">MonoBehaviour Objekt, in der der Boxcollider gezeichnet werden soll.</param>
    /// <param name="color">Die Farbe des Boxcolliders</param>
	public static void DrawBoxCollider(MonoBehaviour mb, Color color)
    {
        if (UnityEditor.Selection.activeGameObject != mb.gameObject)
        {
            BoxCollider boxCollider = mb.GetComponent<BoxCollider>();
            if (mb != null)
            {
                Gizmos.color = color;
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = mb.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                Gizmos.matrix = oldMatrix;
            }
        }
    }
}
