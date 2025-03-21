﻿using System;

namespace HyperVNet.Components
{
    ///<summary>The file format of the virtual hard disk.</summary>
    public enum VirtualHardDiskFormat : ushort
    {
        ///<summary>Generation 1 virtual hard disk format.</summary>
        Vhd = 2,

        ///<summary>Generation 2 virtual hard disk format.</summary>
        Vhdx = 3
    }
}