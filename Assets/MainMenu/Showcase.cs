using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class Showcase : MonoBehaviour
{
    private CinemachineDollyCart cdc;
    private void Start()
    {
        cdc = GetComponent<CinemachineDollyCart>();
    }
    public void next(InputAction.CallbackContext ctx)
    {
        if (ctx.started && cdc.m_Speed < 5)
            cdc.m_Speed += 5;
    }
    public void previous(InputAction.CallbackContext ctx)
    {
        if (ctx.started && cdc.m_Speed > -5)
            cdc.m_Speed -= 5;
    }
}
