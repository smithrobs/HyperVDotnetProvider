using System.Collections.Generic;
using Pulumi;
using Pulumi.Hypervprovider;

return await Deployment.RunAsync(() =>
{
    var hypervProvider = new Provider("hyperprovider");

    var customVm = new VirtualMachine("rstestvm", new VirtualMachineArgs
    {
        Vmname = "rsvmtest"
    }, new CustomResourceOptions
    {
        Provider = hypervProvider
    });

    return new Dictionary<string, object?>
    {
        ["vmId"] = customVm.Id
    };
});
