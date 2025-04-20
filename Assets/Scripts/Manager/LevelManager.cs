using PreggoJam.Prop;
using UnityEngine;

namespace PreggoJam.Manager
{
    public class LevelManager : MonoBehaviour
    {
        private void Start()
        {
            foreach (var lr in FindObjectsByType<LevelRequirement>(FindObjectsSortMode.None))
            {
                if (ProgressionManager.Instance.LevelIndex < lr.TargetRequirement)
                {
                    lr.GetComponent<IActivable>().Toggle(false);
                }
            }
        }
    }
}