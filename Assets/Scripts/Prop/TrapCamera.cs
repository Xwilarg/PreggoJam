using PreggoJam.Player;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace PreggoJam.Prop
{
    public class TrapCamera : MonoBehaviour
    {
        [SerializeField]
        private Material _visionMat;
        [SerializeField]
        private float _maxCameraRotationDown, _maxCameraRotationUp, _cameraSpeed;
        [SerializeField]
        private Ease _cameraEase;
        private PolygonCollider2D _coll;

        private void Awake()
        {
            RenderPipelineManager.endCameraRendering += OnPostRenderCallback;
            _coll = GetComponent<PolygonCollider2D>();
            UpdateCollider();
        }

        private void OnDestroy()
        {
            RenderPipelineManager.endCameraRendering -= OnPostRenderCallback;
        }
        private void Start()
        {
            Sequence camSeq = DOTween.Sequence();
            camSeq.Append(transform.GetChild(0).DORotate(new Vector3(0, 0, _maxCameraRotationDown), _cameraSpeed).SetEase(_cameraEase));
            camSeq.Append(transform.GetChild(0).DORotate(new Vector3(0, 0, _maxCameraRotationUp), _cameraSpeed).SetEase(_cameraEase));
            camSeq.OnUpdate(() => UpdateCollider());
            camSeq.SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().ResetPlayer();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2? prev = null;
            foreach (var pos in CameraVision(.01f))
            {
                if (prev == null)
                {
                    prev = pos;
                    Gizmos.DrawLine(prev.Value, transform.position);
                }
                else
                {
                    Gizmos.DrawLine(prev.Value, pos);
                    prev = pos;
                }
            }
            Gizmos.DrawLine(prev.Value, transform.position);
        }

        private void UpdateCollider()
        {
            List<Vector2> points = new();
            points.Add(Vector2.zero);
            foreach (var pos in CameraVision(.01f))
            {
                points.Add(pos - (Vector2)transform.position);
            }
            _coll.points = points.ToArray();
        }
        private IEnumerable<Vector2> CameraVision(float incr)
        {
            var from = (Vector2)transform.position;
            var localDir = transform.position - transform.GetChild(0).up;
            var angleRad = Mathf.Atan2(localDir.y - from.y, localDir.x - from.x);

            for (float i = Mathf.PI / 4; i < 3 * Mathf.PI / 4; i += incr)
            {
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

                yield return pos;
            }
        }

        private void OnPostRenderCallback(ScriptableRenderContext _, Camera c)
        {
            if (!gameObject.activeInHierarchy) return;

            GL.PushMatrix();

            GL.LoadOrtho();

            _visionMat.SetPass(0);

            Vector2? prevPos = null;

            var from = (Vector2)transform.position;
            foreach (var pos in CameraVision(0.001f))
            {
                GL.Begin(GL.TRIANGLES); // Performances :thinking:

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