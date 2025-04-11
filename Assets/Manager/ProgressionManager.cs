using PreggoJam.SO;
using UnityEngine;

namespace PreggoJam.Manager
{
    public class ProgressionManager : MonoBehaviour
    {
        public static ProgressionManager Instance { private set; get; }

        [SerializeField]
        private GameInfo _info;

        [SerializeField]
        private RectTransform _progressionUI;

        private int _levelIndex;
        private int _potionCaught;

        private void Awake()
        {
            Instance = this;
            _progressionUI.localScale = new Vector3(0f, 1f, 1f);
        }

        public void GrabPotion()
        {
            _potionCaught++;
            _progressionUI.localScale = new Vector3(_potionCaught / (float)_info.Levels[_levelIndex].ObjectiveCount, 1f, 1f);
        }
    }
}