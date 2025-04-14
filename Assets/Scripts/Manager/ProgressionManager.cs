using DG.Tweening;
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

        public bool IsProgressionFull => _potionCaught >= _info.Levels[LevelIndex].ObjectiveCount;

        public int LevelIndex { private set; get; }
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
            _progressionUI.DOScaleX(_potionCaught / (float)_info.Levels[LevelIndex].ObjectiveCount, .7f);
        }

        public void NextLevel()
        {
            LevelIndex++;
            _potionCaught = 0;
            _progressionUI.localScale = new Vector3(0f, 1f, 1f);
        }
    }
}