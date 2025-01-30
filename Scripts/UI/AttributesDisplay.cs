using KhotiProject.Scripts.Constants;
using KhotiProject.Scripts.Stats;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KhotiProject.Scripts.UI
{
    public class AttributesDisplay : MonoBehaviour
    {
        [SerializeField] Image forebarForeground;
        [SerializeField] Image yRotationForeground;
        [SerializeField] Image xRotationForeground;
        [SerializeField] Image healthForeground;
        [SerializeField] TextMeshProUGUI minLaunchForceText;
        [SerializeField] TextMeshProUGUI maxLaunchForceText;
        [SerializeField] TextMeshProUGUI launchForceText;
        [SerializeField] TextMeshProUGUI forceChangeSpeedText;
        [SerializeField] TextMeshProUGUI launchTimeLimitText;
        [SerializeField] TextMeshProUGUI yAxisRotationText;
        [SerializeField] TextMeshProUGUI xAxisRotationText;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] TextMeshProUGUI defenceText;
        [SerializeField] TextMeshProUGUI forceHitText;

        private float forceChangeRate = 2f;
        private bool isIncreasing = true;
        private float minLaunchForce;
        private float maxLaunchForce;
        private float launchTimeLimit;

        private bool turnStart;
        void Update()
        {
            UpdateFillAmount();

            launchTimeLimitText.text = $"{StatsConstants.LaunchTimeLimit}{launchTimeLimit}";
        }

        private void UpdateFillAmount()
        {
            if(turnStart)
            {
                if (isIncreasing)
                {
                    forebarForeground.fillAmount += forceChangeRate * Time.deltaTime;
                    if (forebarForeground.fillAmount >= 1f)
                    {
                        forebarForeground.fillAmount = 1f;
                        isIncreasing = false;
                    }
                }
                else
                {
                    forebarForeground.fillAmount -= forceChangeRate * Time.deltaTime;
                    if (forebarForeground.fillAmount <= 0f)
                    {
                        forebarForeground.fillAmount = 0f;
                        isIncreasing = true;
                    }
                }
                launchForceText.text = string.Empty;
                launchForceText.text = $"{StatsConstants.LaunchForce}{Math.Round(CalculateForce(),2)}";
            }
        }

        public void SetCurrentPlayerStats(KhotiStats stats)
        {
            minLaunchForce = stats.MinLaunchForce;
            maxLaunchForce = stats.MaxLaunchForce;

            minLaunchForceText.text = minLaunchForce.ToString();
            maxLaunchForceText.text = maxLaunchForce.ToString();
            
            forceChangeRate = stats.ForceChangeRate;
            forceChangeSpeedText.text = $"{StatsConstants.ForceChangeRate}{forceChangeRate}";

            yRotationForeground.transform.rotation = Quaternion.Euler(0, 0, 0);
            yAxisRotationText.text = $"{StatsConstants.YAxisRotation}-";
            xRotationForeground.transform.rotation = Quaternion.Euler(0, 0, 0);
            xAxisRotationText.text = $"{StatsConstants.XAxisRotation}-";

            if (stats.Health > 0)
            {
                healthText.text = $"{StatsConstants.Health}{stats.Health}";
            }
            else
            {
                healthText.text = $"{StatsConstants.PlayerDead}";
            }
            
            if(stats.Defence > 0)
            {
                defenceText.text = $"{StatsConstants.Defence}{stats.Defence}";
            }
            else
            {
                defenceText.text = $"{StatsConstants.DefenceBreak}";
            }
            
            forceHitText.text = string.Empty;
            healthForeground.fillAmount = Mathf.Max((float)stats.Health/stats.MaxHealth, 0);
        }

        public float CalculateForce()
        {
            var forceRange = maxLaunchForce - minLaunchForce;
            var appliedForce = minLaunchForce + (forceRange * forebarForeground.fillAmount);

            return appliedForce;
        }

        public void UpdateTurnStart(bool turnStart)
        {
            this.turnStart = turnStart;
        }
        public void UpdateLaunchTimeLimit(int launchTimeLimit)
        {
            this.launchTimeLimit = launchTimeLimit;
        }

        public void UpdateRotations(float yRotation, float xRotation)
        {
            yRotationForeground.transform.rotation = Quaternion.Euler(0, 0, -yRotation);
            yAxisRotationText.text = $"{StatsConstants.YAxisRotation}{Mathf.Round(yRotation)}";
            xRotationForeground.transform.rotation = Quaternion.Euler(0, 0, xRotation);
            xAxisRotationText.text = $"{StatsConstants.XAxisRotation}{Mathf.Round(-xRotation)}";
        }

        public void UpdateForceHit(float forceHit)
        {
            forceHitText.text = $"{StatsConstants.ForceHit}{Math.Round(forceHit, 2)}";
        }
    }
}

