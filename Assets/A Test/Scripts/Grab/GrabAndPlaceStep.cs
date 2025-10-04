using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class GrabAndPlaceStep : MonoBehaviour
{
    [Header("References")]
    public StepManager stepManager;
    public int stepIndex;

    [Header("Grab Objects")]
    public List<XRGrabInteractable> redObjects = new List<XRGrabInteractable>();
    public List<XRGrabInteractable> greenObjects = new List<XRGrabInteractable>();

    [Header("Socket Reference")]
    public XRSocketInteractor tableSocket;

    [Header("Colors")]
    public Color redColor = Color.red;
    public Color greenColor = Color.green;
    public Color defaultColor = Color.white;

    private int greenGrabbedCount = 0;
    private bool stepCompleted = false;

    void Start()
    {
        // Subscribe to grab events for red objects
        foreach (var red in redObjects)
        {
            red.selectEntered.AddListener((args) => OnGrabObject(red, redColor));
            red.selectExited.AddListener((args) => OnReleaseObject(red));
        }

        // Subscribe to grab events for green objects
        foreach (var green in greenObjects)
        {
            green.selectEntered.AddListener((args) => OnGrabObject(green, greenColor));
            green.selectExited.AddListener((args) => OnReleaseGreen(green));
        }

        // Subscribe to socket events
        if (tableSocket != null)
        {
            tableSocket.selectEntered.AddListener(OnObjectPlacedInSocket);
            tableSocket.selectExited.AddListener(OnObjectRemovedFromSocket);
        }

        // Reset colors
        foreach (var obj in redObjects)
            SetColor(obj, defaultColor);
        foreach (var obj in greenObjects)
            SetColor(obj, defaultColor);
    }

    void OnGrabObject(XRGrabInteractable grabbedObj, Color color)
    {
        SetColor(grabbedObj, color);
    }

    void OnReleaseObject(XRGrabInteractable releasedObj)
    {
        SetColor(releasedObj, defaultColor);
    }

    void OnReleaseGreen(XRGrabInteractable releasedObj)
    {
        SetColor(releasedObj, defaultColor);
        greenGrabbedCount++;
    }

    void OnObjectPlacedInSocket(SelectEnterEventArgs args)
    {
        if (stepCompleted) return;

        // Check if the placed object is one of the green cubes
        XRGrabInteractable placedObject = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (greenObjects.Contains(placedObject))
        {
            Debug.Log("✅ Green cube placed in socket!");
            CompleteStep();
        }
        else
        {
            Debug.Log("❌ Non-green object placed in socket.");
        }
    }

    void OnObjectRemovedFromSocket(SelectExitEventArgs args)
    {
        Debug.Log("Object removed from socket.");
    }

    void SetColor(XRGrabInteractable obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = color;
    }

    void CompleteStep()
    {
        if (stepCompleted) return;
        stepCompleted = true;

        if (stepManager != null && stepIndex >= 0 && stepIndex < stepManager.steps.Length)
        {
            stepManager.steps[stepIndex].isDone = true;
          
        }
    }
}