using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private VectorDampener motionVector = new VectorDampener(true);


    private int VelXId;
    private int VelYId;

    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        motionVector.TargetValue = direction;
       
    }
    public void ToggleSprint(CallbackContext ctx) 
    {
        bool val = ctx.ReadValueAsButton();
        motionVector.Clamp = !val;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
    }
#endif
    private void Awake()
    {
        VelXId = Animator.StringToHash("VelX");
        VelYId = Animator.StringToHash("VelY");
    }
    private void Update()
    {
        motionVector.Update();
        Vector2 direction = motionVector.CurrentValue;
        anim.SetFloat(VelXId, direction.x);
        anim.SetFloat(VelYId, direction.y);
    }
    private void OnAnimatorMove()
    {
        float interpolator = Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up));
        Vector3 forward  = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, interpolator);
        Vector3 projected = Vector3.ProjectOnPlane(forward, transform.up);
        Quaternion rotation = Quaternion.LookRotation(projected, transform.up);
        anim.rootRotation = Quaternion.Slerp(anim.rootRotation, rotation, motionVector.CurrentValue.magnitude);
        anim.ApplyBuiltinRootMotion();

    }
}
