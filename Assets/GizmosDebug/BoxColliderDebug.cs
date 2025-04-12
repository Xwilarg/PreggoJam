using UnityEngine;

namespace PreggoJam.GizmosDebug
{
    public class BoxColliderDebug : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var coll = GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(new Vector2(transform.position.x + coll.offset.x, transform.position.y + coll.offset.y), new Vector2(coll.size.x * transform.localScale.x, coll.size.y * transform.localScale.y));
        }
    }
}