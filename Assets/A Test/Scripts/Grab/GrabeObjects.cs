using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabeObjects : MonoBehaviour
{
    [Header("References")]
    public StepManager stepManager;
    public int stepIndex = 0;

    [Header("Objects to Grab")]
    public XRGrabInteractable[] grabObjects; // assign both objects here

    private bool[] grabbedFlags;
    private bool stepCompleted = false;

    void Start()
    {
        grabbedFlags = new bool[grabObjects.Length];

        // subscribe to grab events
        for (int i = 0; i < grabObjects.Length; i++)
        {
            int index = i; // local copy for closure
            grabObjects[i].selectEntered.AddListener((args) => OnObjectGrabbed(index));
        }
    }

    private void OnObjectGrabbed(int index)
    {
        if (stepCompleted) return;

        grabbedFlags[index] = true;
        Debug.Log($"Object {index} grabbed ✅");

        // check if all have been grabbed at least once
        bool allGrabbed = true;
        foreach (bool grabbed in grabbedFlags)
        {
            if (!grabbed)
            {
                allGrabbed = false;
                break;
            }
        }

        if (allGrabbed)
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
            Debug.Log($"✅ Step {stepIndex} completed — Player grabbed both objects (near & far).");
        }
    }
}