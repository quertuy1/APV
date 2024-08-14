using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class AimControler : MonoBehaviour
{
    [SerializeField] private VectorDampener lookVector;
    [SerializeField] private AimControler chestAim;
    [SerializeField] private Transform aimRig;
  public void Aim(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        //Desencadenar la activacion de la camara de apuntado
        chestAim.enabled =val;
        // Activar o desactivar el constraint de apuntado del pecho
        aimRig.gameObject.SetActive(val);


    
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.TargetValue = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        lookVector.Update();
        aimRig.RotateAround(aimRig.position, transform.up, lookVector.CurrentValue.x);
    }

    void awake()
    {
        aimRig.gameObject.SetActive(false);
    }
}
