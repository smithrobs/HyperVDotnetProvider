﻿using HyperV;
using System;
using System.IO;
using System.Management;

namespace HyperVNet.Components
{
    ///<summary>Defines a boot device.</summary>
    public class BootDevice
    {
        ///<summary>The firmware description of the boot device.</summary>
        public string Description { get; }

        ///<summary>The type of boot device.</summary>
        public BootDeviceType DeviceType { get; }

        ///<summary>A device specific value describing the boot media.</summary>
        public string Value { get; }

        private readonly string bootEntry;

        ///<summary>Initializes a new instance of the <see cref="BootDevice"/> class from the specified WMI string.</summary>
        ///<param name="bootEntry">The WMI string for this boot device.</param>
        public BootDevice(string bootEntry)
        {
            this.bootEntry = bootEntry;
            DeviceType = BootDeviceType.Unknown;
            Value = "";

            using (ManagementObject bootSource = new ManagementObject(bootEntry))
            {
                Description = (string)bootSource["BootSourceDescription"];
                switch ((uint)bootSource["BootSourceType"])
                {
                    case 1:
                        using (ManagementObject resource = VMMS.GetRelatedSettings(bootSource, VMMS.Settings.Resource))
                        {
                            switch ((ushort)resource["ResourceType"])
                            {
                                case 16: DeviceType = BootDeviceType.DvdDrive; break;
                                case 17: DeviceType = BootDeviceType.HardDrive; break;
                            }
                            using (ManagementObject storage = VMMS.GetRelatedSettings(resource, VMMS.Settings.Storage))
                                if (storage != null)
                                    Value = Path.GetFileName(((string[])storage["HostResource"])[0]);
                        }
                        break;

                    case 2:
                        using (ManagementObject networkAdapter = VMMS.GetRelatedSettings(bootSource, VMMS.Settings.NetworkAdapter))
                        {
                            DeviceType = BootDeviceType.NetworkAdapter;
                            using (ManagementObject switchPort = VMMS.GetRelatedSettings(networkAdapter, VMMS.Settings.SwitchPort))
                                if (switchPort != null)
                                    if ((ushort)switchPort["EnabledState"] == 2)
                                        Value = (string)switchPort["LastKnownSwitchName"];
                                    else
                                        Value = "None";
                        }
                        break;

                    case 3:
                        DeviceType = BootDeviceType.File;
                        Value = Path.GetFileName(((string)bootSource["FirmwareDevicePath"]).Split('/')[1]);
                        break;
                }
            }
        }

        ///<summary>Returns the WMI string for this boot device.</summary>
        public override string ToString()
        {
            return bootEntry;
        }
    }
}