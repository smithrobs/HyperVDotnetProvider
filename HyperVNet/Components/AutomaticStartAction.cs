using System;

namespace HyperVNet.Components
{
    ///<summary>Defines the automatic start action.</summary>
    public enum AutomaticStartAction : ushort
    {
        ///<summary>Do not start automatically.</summary>
        Nothing = 2,

        ///<summary>Automatically start if the virtual machine was running when the service stopped.</summary>
        StartIfRunning = 3,

        ///<summary>Always start the virtual machine automatically.</summary>
        AlwaysStart = 4
    }
}