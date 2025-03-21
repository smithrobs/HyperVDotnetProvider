﻿using System;

namespace HyperVNet.Components
{
    ///<summary>Defines the current state of a virtual machine.</summary>
    public enum VirtualMachineState : ushort
    {
        ///<summary>The state of the virtual machine could not be determined.</summary>
        Unknown = 0,

        ///<summary>The virtual machine is in an other state.</summary>
        Other = 1,

        ///<summary>The virtual machine is running.</summary>
        Running = 2,

        ///<summary>The virtual machine is turned off.</summary>
        Off = 3,

        ///<summary>The virtual machine is in the process of turning off.</summary>
        ShuttingDown = 4,

        ///<summary>The virtual machine does not support being started or turned off.</summary>
        NotApplicable = 5,

        ///<summary>The virtual machine might be completing commands, and it will drop any new requests.</summary>
        EnabledButOffline = 6,

        ///<summary>The virtual machine is in a test state.</summary>
        InTest = 7,

        ///<summary>The virtual machine might be completing commands, but it will queue any new requests.</summary>
        Deferred = 8,

        ///<summary>The virtual machine is running but in a restricted mode. The behavior of the virtual machine is similar to the Running state, but it processes only a restricted set of commands. All other requests are queued.</summary>
        Quiesce = 9,

        ///<summary>The virtual machine is in the process of starting. New requests are queued.</summary>
        Starting = 10

    }
}