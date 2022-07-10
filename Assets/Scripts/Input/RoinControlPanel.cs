using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct RoinControlPanelState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('R', 'O', 'I', 'N');

    [InputControl(name = "button1", layout = "Button", bit = 0)]
    [InputControl(name = "button2", layout = "Button", bit = 1)]
    public int buttons;
    [InputControl(name = "stick1", layout = "Vector2", format = "VC2S", sizeInBits = 32)]
    [InputControl(name = "stick1/x", format = "SHRT", sizeInBits = 16)]
    public short s1x;
    [InputControl(name = "stick1/y", format = "SHRT", sizeInBits = 16, offset = 2)]
    public short s1y;
    [InputControl(name = "stick2", layout = "Vector2", format = "VC2S", sizeInBits = 32)]
    [InputControl(name = "stick2/x", format = "SHRT", sizeInBits = 16)]
    public short s2x;
    [InputControl(name = "stick2/y", format = "SHRT", sizeInBits = 16, offset = 2)]
    public short s2y;

}

[InputControlLayout(stateType = typeof(RoinControlPanelState))]
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class RoinControlPanel : InputDevice 
{    
    public static RoinControlPanel controlPanel { get; private set; }
    public ButtonControl button1 { get; private set; }    
    public ButtonControl button2 { get; private set; }
    public Vector2Control stick1 { get; private set; }  
    public Vector2Control stick2 { get; private set; }
    public Vector2Control stick3 { get; private set; }

    static RoinControlPanel()
    {
        InputSystem.RegisterLayout<RoinControlPanel>();

        if(!InputSystem.devices.Any(dev => dev is RoinControlPanel))
            controlPanel = InputSystem.AddDevice<RoinControlPanel>();    
    }
    protected override void FinishSetup()
    {        
        base.FinishSetup();
    }

    //[MenuItem("Tools/Add RoinControlPanel")]
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize() 
    {
        InputSystem.QueueStateEvent(controlPanel, new RoinControlPanelState { s1x = 1, s1y = 1 });
    }
}