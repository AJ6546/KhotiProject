using System;
using TMPro;
using UnityEngine;

namespace KhotiProject.Scripts.UI.DamageTxt
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageText;
        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}

