using System.Collections.Generic;
using UnityEngine;
using Breadnone;
using UnityEngine.UIElements;
using System.Collections;

public class VTweenRotateTo : MonoBehaviour
{
    [field: SerializeField] public GameObject objectToRotate { get; set; }
    [field: SerializeField] public float duration { get; set; } = 1f;
    [field: SerializeField] public float delay { get; set; }
    [field: SerializeField] public int loopCount { get; set; }
    [field: SerializeField] public float angle { get; set; }
    [field: SerializeField] public Ease ease { get; set; } = Ease.Linear;
    [field: SerializeField] public bool pingpong { get; set; }
    [field: SerializeField] public bool setActive { get; set; }
    [field: SerializeField] public bool unscaledTime { get; set; }
    [field: SerializeField] public VectorDirection direction { get; set; }
    private Vector3 defaultPos;
    void Awake()
    {
        if (objectToRotate != null)
            defaultPos = objectToRotate.transform.position;
    }
    public void StartTween()
    {
        if (delay > 0)
            StartCoroutine(StartDelay());
        else
            RotateToTargetObject();
    }
    private IEnumerator StartDelay()
    {
        if (!unscaledTime)
            yield return new WaitForSeconds(delay);
        else
            yield return new WaitForSecondsRealtime(delay);
        RotateToTargetObject();
    }
    public void RotateToTargetObject()
    {
        Vector3 vecDir = Vector3.left;

        if (direction == VectorDirection.Vector3Up)
        {
            vecDir = Vector3.up;
        }
        else if (direction == VectorDirection.Vector3Down)
        {
            vecDir = Vector3.down;
        }
        else if (direction == VectorDirection.Vector3Left)
        {
            vecDir = Vector3.left;
        }
        else if (direction == VectorDirection.Vector3Right)
        {
            vecDir = Vector3.right;
        }
        else if (direction == VectorDirection.Vector3Forward)
        {
            vecDir = Vector3.forward;
        }
        else if (direction == VectorDirection.Vector3Back)
        {
            vecDir = Vector3.back;
        }

        VTween.rotate(objectToRotate, angle, vecDir, duration).setLoop(loopCount).setUnscaledTime(unscaledTime).setPingPong(pingpong).setInactiveOnComplete(setActive).setEase(ease).setLoop(loopCount);
    }
    public void SetToDefaultPosition()
    {
        objectToRotate.transform.position = defaultPos;
    }
}
