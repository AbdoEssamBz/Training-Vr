using UnityEngine;

public class GrabSingleObject : MonoBehaviour
{
    public StepManager stepManager;
    public int stepIndex = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {

            Debug.Log("Thorw");
            CompleteStep();
        }


    }

    public void CompleteStep()
    {
        if (stepManager != null && stepIndex >= 0 && stepIndex < stepManager.steps.Length)
        {
            stepManager.steps[stepIndex].isDone = true;
            Debug.Log($"Step {stepIndex} marked as DONE ✅");
        }
    }
}
/*
 public StepManager stepManager;
    public int stepIndex = 0;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <-10)
        {

            Debug.Log("Thorw");
            CompleteStep();
        }

        
    }

    public void CompleteStep()
    {
        if (stepManager != null && stepIndex >= 0 && stepIndex < stepManager.steps.Length)
        {
            stepManager.steps[stepIndex].isDone = true;
            Debug.Log($"Step {stepIndex} marked as DONE ✅");
        }
    }
}



*/