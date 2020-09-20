using UnityEngine;


[RequireComponent(typeof(BoxCollider))]

public class LevelTrigger : MonoBehaviour
{
    public Color BoxColor;
    public bool GizmoWireframe;

    public void OnDrawGizmos()
    {
        var transform1 = transform;
        var cachedColor = Gizmos.color;
        Gizmos.color = BoxColor;
        if (GizmoWireframe)
        {
            Gizmos.DrawWireCube(transform1.position, transform1.localScale);
        }
        else
        {
            Gizmos.DrawCube(transform1.position, transform1.localScale);
        }
        Gizmos.color = cachedColor;
    }
}
