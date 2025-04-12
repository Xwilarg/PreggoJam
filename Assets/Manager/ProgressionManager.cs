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

        [SerializeField]
        private GameObject _warningText;
        public GameObject WarningText => _warningText;

        public bool IsProgressionFull => _potionCaught >= _info.Levels[_levelIndex].ObjectiveCount;

        private int _levelIndex;
        private int _potionCaught;

        private void Awake()
        {
            Instance = this;
            _progressionUI.localScale = new Vector3(0f, 1f, 1f);
            WarningText.SetActive(false);
        }

        public void GrabPotion()
        {
            _potionCaught++;
            _progressionUI.localScale = new Vector3(_potionCaught / (float)_info.Levels[_levelIndex].ObjectiveCount, 1f, 1f);
        }

        public void NextLevel()
        {
            _levelIndex++;
            _potionCaught = 0;
            _progressionUI.localScale = new Vector3(0f, 1f, 1f);
        }
    }
}