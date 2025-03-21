using HyperV;
using HyperVNet.Components;
using HyperVNet.Definitions;
using Pulumi.Experimental.Provider;

namespace HyperVNet
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            return Provider.Serve(args, "0.0.1", host => new HyperVProvider(host), CancellationToken.None);
        }

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Let's build a HYPER-V");

        //    // Debugging:
        //    // Get-WmiObject -Namespace "ROOT/virtualization/v2" -Query "SELECT * FROM Msvm_ResourcePool WHERE ResourceSubType = ""Microsoft:Hyper-V:Memory"" AND Primordial = True"

        //    // RSS TODO - remote connect fails?
        //    var vmms = new VMMS("HUME");

        //    //var vmms = new VMMS();

        //    // Define the Virtual Machine
        //    var virtualMachineDefinition = new VirtualMachineDefinition(
        //        "testvm1",
        //        @"C:\ProgramData\Microsoft\Windows\Hyper-V"
        //    )
        //    {
        //        Memory =
        //        {
        //            Startup = 4096
        //        },
        //        Processor =
        //        {
        //            Quantity = 2
        //        }
        //    };
        //    virtualMachineDefinition.ScsiControllers[0].Drives[0] = new VirtualHardDrive(new VirtualHardDisk(
        //        VirtualHardDiskFormat.Vhdx,
        //        VirtualHardDiskType.FixedSize,
        //        1,
        //        @"F:\hyperv\testvm1.vhdx"
        //    ));
        //    //virtualMachineDefinition.ScsiControllers[0].Drives[1] = new VirtualDvdDrive();
        //    //virtualMachineDefinition.NetworkAdapters[0].VirtualSwitch = "VM Switch Name";
        //    virtualMachineDefinition.AutomaticStop.Action = AutomaticStopAction.Shutdown;

        //    // Create the Virtual Machine
        //    var vmName = vmms.CreateVirtualMachine(virtualMachineDefinition);
        //    Console.WriteLine($"vm name = {vmName}");
        //}
    }
}
