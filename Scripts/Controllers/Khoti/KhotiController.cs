using KhotiProject.Scripts.Combat;
using UnityEngine;

namespace KhotiProject.Scripts.Controllers.Khoti
{
    public class KhotiController : BaseController
    {
        public FixedButton launchButton;
        public Fighter fighter;
        private bool isDragging = false;
        private bool autoLaunchEnabled = false;
        private bool launchPressed = false;

        private Vector3 lastMousePosition;
        private float dragStartTime;

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (isLaunched || turnManager == null || turnManager.CurrentPlayerIndex != playerIndex || isHit) return;

            ClickAndDrag();
            InteractWithCombat();

            attributesDisplay.UpdateLaunchTimeLimit((int)(stats.MaxLaunchTime - Time.time + dragStartTime));
            
          
            if(launchPressed)
            {
                LaunchOnRelease();
            }

            if (autoLaunchEnabled && Time.time - dragStartTime > stats.MaxLaunchTime)
            {
                Launch();
                ExecuteAfterLaunch();
            }
        }

        private void ClickAndDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsClickedOnKhoti()) return;

                isDragging = !isDragging;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                float mouseDeltaX = currentMousePosition.x - lastMousePosition.x;
                float mouseDeltaY = currentMousePosition.y - lastMousePosition.y;

                transform.Rotate(Vector3.up, mouseDeltaX * stats.RotationSpeed * Time.deltaTime);

                float newXRotation = transform.eulerAngles.x - mouseDeltaY * stats.RotationSpeed * Time.deltaTime;
                newXRotation = Mathf.Clamp(newXRotation > 180 ? newXRotation - 360 : newXRotation, -90, 0);

                transform.eulerAngles = new Vector3(newXRotation, transform.eulerAngles.y, transform.eulerAngles.z);

                float yRotation = transform.eulerAngles.y;
                attributesDisplay.UpdateRotations(yRotation, newXRotation);

                lastMousePosition = currentMousePosition;
            }
        }

        public void SetupTurn()
        {
            if(launchButton == null)
            {
                launchButton = FindAnyObjectByType<FixedButton>(FindObjectsInactive.Include);
                launchButton.OnButtonPressed += OnLaunchPressed;
            }
            arrow.SetActive(true);
            launchButton.gameObject.SetActive(true);
            ActivateAutoLaunch();
        }

        private void OnLaunchPressed()
        {
            isDragging = !isDragging;
            launchPressed = !launchPressed;
            launchButton.gameObject.SetActive(false);
            fighter.Cancel();
        }

        private void ActivateAutoLaunch()
        {
            dragStartTime = Time.time;
            autoLaunchEnabled = !autoLaunchEnabled;
        }

        private void LaunchOnRelease()
        {
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                Launch();
                ExecuteAfterLaunch();
            }
        }

        private void ExecuteAfterLaunch()
        {
            isDragging = !isDragging;
            autoLaunchEnabled = !autoLaunchEnabled;
            launchPressed = !launchPressed;
            launchButton.gameObject.SetActive(false);
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                
                if (target == null) continue;
                Debug.Log(target.transform.gameObject.name);
                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(target);
                }
            }
        }

        private bool IsClickedOnKhoti()
        {
            RaycastHit hit;

            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                return hit.transform == transform;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}