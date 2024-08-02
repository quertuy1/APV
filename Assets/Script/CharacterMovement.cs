using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;


    [SerializeField] private VectorDampener motionVector;
    private int velXId;
    private int velYId;

    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        motionVector.TargetValue=direction;
        
    }

    private void ToggleSprint(CallbackContext ctx)
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
}
