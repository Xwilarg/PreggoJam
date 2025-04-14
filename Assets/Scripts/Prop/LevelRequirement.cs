using UnityEngine;

namespace PreggoJam.Prop
{
    public class LevelRequirement : MonoBehaviour
    {
        [SerializeField]
        private int _targetRequirement;
        public int TargetRequirement => _targetRequirement;
    }
}