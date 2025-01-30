using KhotiProject.Scripts.Stats;
using UnityEngine;

public class FollowBody : MonoBehaviour
{
    [SerializeField] KhotiStats stats = null;
    [SerializeField] float yPos;

    private void LateUpdate()
    {
        if (stats != null)
        {
            var statsTransform = stats.transform;
            transform.position = new Vector3(statsTransform.position.x, yPos, statsTransform.position.z);
        }
    }
}
