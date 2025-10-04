using UnityEngine;

public class TeleportStepTrigger : MonoBehaviour
{
    [Header("References")]
    public StepManager stepManager;
    public int stepIndex = 0;
    public Transform playerHead;  // XR Origin or Camera
    public float triggerDistance = 1.5f;

    private bool stepCompleted = false;

    void Start()
    {
        if (playerHead == null)
            playerHead = Camera.main.transform;
    }
    void Update()
    {
        if (stepCompleted) return;
        if (playerHead == null) return;

        float distance = Vector3.Distance(transform.position, playerHead.position);

        if (distance <= triggerDistance)
        {
            CompleteStep();
        }
    }

    private void CompleteStep()
    {
        stepCompleted = true;
        if (stepManager != null && stepIndex >= 0 && stepIndex < stepManager.steps.Length)
        {
            stepManager.steps[stepIndex].isDone = true;
        }
    }
}