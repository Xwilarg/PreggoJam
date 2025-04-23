using DG.Tweening;
using PreggoJam.SO;
using System.Collections;
using System.Linq;
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

        [SerializeField]
        private Sprite[] _bellySprites;

        [SerializeField]
        private SpriteRenderer _bellySR;

        public bool IsProgressionFull => _potionCaught >= _info.Levels[LevelIndex].ObjectiveCount;

        public int LevelIndex { private set; get; }
        private int _potionCaught;

        private int _bellyIndex;

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

        public void UpdateBelly()
        {
            float bellyTarget = _potionCaught * _bellySprites.Length / (float)_info.Levels.Sum(x => x.ObjectiveCount);
            StartCoroutine(UpdateTo(Mathf.FloorToInt(bellyTarget)));
        }

        private IEnumerator UpdateTo(int max)
        {
            for (; _bellyIndex < max; _bellyIndex++)
            {
                _bellySR.sprite = _bellySprites[_bellyIndex];
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}