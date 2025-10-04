using UnityEngine;

public class StepButton : MonoBehaviour
{
    public StepManager stepManager;
    public int stepIndex = 0; // which step this button completes

    // Call this from a Unity UI Button OnClick()
    public void CompleteStep()
    {
        if (stepManager != null && stepIndex >= 0 && stepIndex < stepManager.steps.Length)
        {
            stepManager.steps[stepIndex].isDone = true;
            Debug.Log($"Step {stepIndex} marked as DONE ✅");
        }
    }
}