using UnityEngine;

[System.Serializable]
public class StepData
{
    public Transform target;        // where instructor walks after step done
    public AudioClip introAudio;    // first audio (welcome/instruction)
    public AudioClip outroAudio;    // second audio (you passed, continue)
    public bool isDone;             // flag for completion
}


public class StepManager : MonoBehaviour
{
    [Header("References")]
    public InstructorController instructor;

    [Header("Steps")]
    public StepData[] steps;  // 4 steps (each has 2 audios)

    private int currentStep = 0;
    private bool waitingForCompletion = false;

    void Start()
    {
        // Subscribe to instructor's arrival event
        instructor.OnReachedTarget += HandleInstructorArrived;

        PlayIntroAudio();
    }

    void Update()
    {
        if (currentStep < steps.Length && waitingForCompletion)
        {
            if (steps[currentStep].isDone)
            {
                waitingForCompletion = false;
                PlayOutroAudio();
            }
        }
    }

    void PlayIntroAudio()
    {
        if (currentStep >= steps.Length) return;

        StepData step = steps[currentStep];
        instructor.PlayAudio(step.introAudio);
        waitingForCompletion = true;
    }

    void PlayOutroAudio()
    {
        StepData step = steps[currentStep];
        instructor.PlayAudio(step.outroAudio, step.target);
    }

    void HandleInstructorArrived()
    {
        currentStep++;
        if (currentStep < steps.Length)
        {
            PlayIntroAudio(); // play intro only after reaching target
        }
        else
        {
            Debug.Log("All steps finished!");
        }
    }
}