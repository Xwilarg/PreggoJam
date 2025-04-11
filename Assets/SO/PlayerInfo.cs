using UnityEngine;

namespace PreggoJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Basic Movements")]
        public float Speed;
        public float JumpForce;
        public float JumpCooldown;
    }
}