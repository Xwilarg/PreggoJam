using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PreggoJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        [SerializeField]
        private Transform _playerTransform, _houseTransform;

        [SerializeField]
        private GameObject _homeUI;

        [SerializeField]
        private CinemachineCamera _cam;
        private CinemachinePositionComposer _camComp;

        private float _baseZoom;

        private void Awake()
        {
            Instance = this;

            _camComp = _cam.GetComponent<CinemachinePositionComposer>();
            _homeUI.SetActive(false);
            if (!SceneManager.GetAllScenes().Any(x => x.name == "Map"))
            {
                SceneManager.LoadScene("Map", LoadSceneMode.Additive);
            }

            _baseZoom = _cam.Lens.OrthographicSize;
        }

        private bool _canPlay = true;
        public bool CanPlay
        {
            set
            {
                _canPlay = value;
                _playerTransform.gameObject.SetActive(value);
                _cam.Target.TrackingTarget = _canPlay ? _playerTransform : _houseTransform;
                _camComp.Composition.DeadZone.Enabled = value;
                _homeUI.SetActive(!value);
                _cam.Lens.OrthographicSize = _canPlay ? _baseZoom : 3f;
            }
            get => _canPlay;
        }

        public void ReloadMap()
        {
            StartCoroutine(ReloadMapInternal());
        }
        private IEnumerator ReloadMapInternal()
        {
            yield return SceneManager.UnloadSceneAsync("Map");
            ProgressionManager.Instance.NextLevel();
            yield return SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
            CanPlay = true;
        }
    }
}