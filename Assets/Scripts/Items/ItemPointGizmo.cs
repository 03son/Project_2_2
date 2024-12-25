using UnityEngine;

[ExecuteInEditMode]
public class ItemPointGizmo : MonoBehaviour
{
    public Color gizmoColor = new Color(0, 0.5f, 0, 0.5f); // 녹색 반투명

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // 색상 설정
        Gizmos.DrawCube(transform.position, transform.localScale); // 큐브 그리기
    }
}

