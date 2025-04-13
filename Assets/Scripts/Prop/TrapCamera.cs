using UnityEngine;
using UnityEngine.Rendering;

namespace PreggoJam.Prop
{
    public class TrapCamera : MonoBehaviour
    {
        [SerializeField]
        private Material _visionMat;

        private Camera _cam;

        private void Awake()
        {
            RenderPipelineManager.endCameraRendering += OnPostRenderCallback;
        }

        private void OnDestroy()
        {
            RenderPipelineManager.endCameraRendering -= OnPostRenderCallback;
        }

        private void OnPostRenderCallback(ScriptableRenderContext _, Camera c)
        {
            Debug.Log("hello");
            GL.PushMatrix();

            GL.LoadOrtho();

            _visionMat.SetPass(0);

            Vector2? prevPos = null;

            var from = (Vector2)transform.position;
            var localDir = transform.position - transform.right;
            var angleRad = Mathf.Atan2(localDir.y - from.y, localDir.x - from.x);

            for (float i = Mathf.PI / 4; i < 3 * Mathf.PI / 4; i += .001f)
            {

                GL.Begin(GL.TRIANGLES); // Performances :thinking:
                Vector2 pos;


                var angle = angleRad + i - Mathf.PI / 2;

                var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                float maxRange = 20f;
                var hit = Physics2D.Raycast(from, dir, maxRange, LayerMask.GetMask("Map"));
                if (hit.collider == null)
                {
                    pos = from + dir * maxRange;
                }
                else
                {
                    pos = hit.point;
                }

                if (prevPos != null)
                {
                    DrawTriangle(from, prevPos.Value, pos);
                }
                prevPos = pos;
                GL.End();
            }
            GL.PopMatrix();
        }

        private void DrawTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            GL.Vertex(WorldToViewport(point1));
            GL.Vertex(WorldToViewport(point2));
            GL.Vertex(WorldToViewport(point3));
            GL.Vertex(WorldToViewport(point1));
        }

        private Vector3 WorldToViewport(Vector2 pos)
        {
            Vector3 newPos = Camera.main.WorldToViewportPoint(pos);
            newPos.z = 0f;
            return newPos;
        }
    }
}