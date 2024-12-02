using UnityEngine;

[ExecuteInEditMode]
public class ItemPointGizmo : MonoBehaviour
{
    public Color gizmoColor = new Color(0, 0.5f, 0, 0.5f); // ��� ������

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // ���� ����
        Gizmos.DrawCube(transform.position, transform.localScale); // ť�� �׸���
    }
}

