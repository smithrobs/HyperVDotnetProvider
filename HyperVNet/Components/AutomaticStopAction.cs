using System;

namespace HyperVNet.Components
{
    ///<summary>Defines the automatic stop action.</summary>
    public enum AutomaticStopAction : ushort
    {
        ///<summary>Save the virtual machine state.</summary>
        Save = 3,

        ///<summary>Turn off the virtual machine.</summary>
        TurnOff = 2,

        ///<summary>Shutdown the guest operating system.</summary>
        Shutdown = 4
    }
}