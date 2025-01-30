using KhotiProject.Scripts.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace KhotiProject.Scripts.UI.OverheadDisplays 
{
    public class OverheadHealthBar : MonoBehaviour
    {
        public Image foreground;
        public Canvas rootCanvas = null;

        public void UpdateRootCanvas(float healthFraction)
        {
            if (Mathf.Max(healthFraction, 0) == 0)
            {
                rootCanvas.enabled = false;
                return;
            }

            foreground.fillAmount = Mathf.Max(0, healthFraction);
            rootCanvas.enabled = true;
        }
    }
}