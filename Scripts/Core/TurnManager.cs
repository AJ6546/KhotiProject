using KhotiProject.Scripts.Controllers.Khoti;
using KhotiProject.Scripts.Stats;
using KhotiProject.Scripts.UI;
using System.Linq;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] CameraFollow cameraFollow;
        [SerializeField] Transform[] khotiList;
        [SerializeField] AttributesDisplay attributesDisplay;

        private int currentPlayerIndex = -1;

        public int CurrentPlayerIndex => currentPlayerIndex;

        public void Start()
        {
            FindAnyObjectByType<FixedButton>().gameObject.SetActive(false);
        }

        public void InitiateGame()
        {
            KhotiStats[] khotiStatsArray = FindObjectsByType<KhotiStats>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            khotiList = khotiStatsArray.Select(khoti => khoti.transform).ToArray();

            ShuffleKhotiList();
            AssignPlayerIndices();
            NextTurn();
        }

        public void NextTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % khotiList.Length;

            cameraFollow.SetTarget(khotiList[currentPlayerIndex]);
            cameraFollow.SetCameraFollow();

            var currentPlayerStats = khotiList[currentPlayerIndex].GetComponent<KhotiStats>();

            attributesDisplay.SetCurrentPlayerStats(currentPlayerStats);
            attributesDisplay.UpdateTurnStart(true);

            var currentPlayerController = khotiList[currentPlayerIndex].GetComponent<BaseController>();
            currentPlayerController.ResetLaunchStatus();

            var playerController = khotiList[currentPlayerIndex].GetComponent<KhotiController>();
            if(playerController != null)
            {
                playerController.SetupTurn();
            }

            var aiController = khotiList[currentPlayerIndex].GetComponent<AIController>();
            if (aiController != null)
            {
                aiController.SetupTurn();
            }

            Debug.Log($"Player {currentPlayerIndex + 1}'s turn");
        }

        private void ShuffleKhotiList()
        {
            khotiList = khotiList.OrderBy(x => Random.value).ToArray();
        }

        private void AssignPlayerIndices()
        {
            for (int i = 0; i < khotiList.Length; i++)
            {
                var khotiController = khotiList[i].GetComponent<BaseController>();
                if (khotiController != null)
                {
                    khotiController.SetPlayerIndex(i);
                }
            }
        }
        public void RemoveKhoti(Transform khoti)
        {
            var khotiListTemp = khotiList.ToList();
            int removedIndex = khotiListTemp.IndexOf(khoti);

            if (removedIndex != -1)
            {
                khotiListTemp.RemoveAt(removedIndex);
                khotiList = khotiListTemp.ToArray();

                if (removedIndex <= currentPlayerIndex)
                {
                    currentPlayerIndex--;
                }

                currentPlayerIndex = Mathf.Clamp(currentPlayerIndex, 0, khotiList.Length - 1);

                AssignPlayerIndices();

                Debug.Log($"Removed Khoti. Current player index: {currentPlayerIndex}");
            }
        }

    }

}
