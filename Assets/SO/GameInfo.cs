using UnityEngine;

namespace PreggoJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInfo", fileName = "GameInfo")]
    public class GameInfo : ScriptableObject
    {
        public Level[] Levels;
    }

    [System.Serializable]
    public class Level
    {
        public int ObjectiveCount;
    }
}