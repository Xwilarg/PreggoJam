using UnityEngine;

namespace PreggoJam.Prop
{
    public class LevelRequirement : MonoBehaviour
    {
        [SerializeField]
        private int _targetRequirement;
        public int TargetRequirement => _targetRequirement;

        private Color[] Colors = new Color[]
        {
            Color.green,
            Color.blue,
            Color.yellow,
            Color.red,
            Color.black
        };
        private void OnDrawGizmos()
        {
            if (_targetRequirement > 0)
            {
                Gizmos.color = Colors[_targetRequirement - 1];
                Gizmos.DrawCube(transform.position, Vector2.one * .5f);
            }
        }
    }
}