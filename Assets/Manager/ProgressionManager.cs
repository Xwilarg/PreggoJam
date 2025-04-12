using PreggoJam.SO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private int _levelIndex;
        private int _potionCaught;

        private void Awake()
        {
            Instance = this;
            _progressionUI.localScale = new Vector3(0f, 1f, 1f);
            WarningText.SetActive(false);

            if (!SceneManager.GetAllScenes().Any(x => x.name == "Map"))
            {
                SceneManager.LoadScene("Map", LoadSceneMode.Additive);
            }
        }

        public void GrabPotion()
        {
            _potionCaught++;
            _progressionUI.localScale = new Vector3(_potionCaught / (float)_info.Levels[_levelIndex].ObjectiveCount, 1f, 1f);
        }
    }
}