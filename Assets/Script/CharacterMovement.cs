using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private VectorDampener motionVector = new VectorDampener(clamp:true);
    private int velXId;
    private int velYId;

    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        motionVector.TargetValue=direction;
        
    }

    public void ToggleSprint(CallbackContext ctx)
    {
        
        bool val = ctx.ReadValueAsButton();
        motionVector.Clamp =!val;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
     
    }
#endif

    private void Awake()
    {
        velXId = Animator.StringToHash("VelX");
        velYId = Animator.StringToHash("VelY");
    }

    private void Update()
    {
        motionVector.Update();
        Vector2 direction = motionVector.CurrentValue;
        anim.SetFloat(velXId, motionVector.CurrentValue.x);
        anim.SetFloat(velYId, motionVector.CurrentValue.y);
    }

    private void OnAnimatorMove()
    {
        //anim.ApplyBuiltinRootMotion();
        float interpolator = Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up));
        Vector3 forward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, interpolator);
        Vector3 projected = Vector3.ProjectOnPlane(forward, transform.up);
        Quaternion rotation = Quaternion.LookRotation(projected, transform.up); 
        anim.rootRotation = Quaternion.Slerp(Quaternion.identity, rotation, motionVector.CurrentValue.magnitude);
        anim.ApplyBuiltinRootMotion();

    }
}
