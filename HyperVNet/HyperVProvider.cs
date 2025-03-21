using HyperV;
using HyperVNet.Components;
using HyperVNet.Definitions;
using Pulumi.Experimental.Provider;

namespace HyperVNet
{
    public class HyperVProvider : Pulumi.Experimental.Provider.Provider
    {
        readonly IHost host;
        string parameter;

        public HyperVProvider(IHost host)
        {
            this.host = host;
            this.parameter = "hypervprovider";
        }

        public override Task<ParameterizeResponse> Parameterize(ParameterizeRequest request, CancellationToken ct)
        {
            // RSS TODO: should impl?

            return base.Parameterize(request, ct);
        }

        public override Task<GetSchemaResponse> GetSchema(GetSchemaRequest request, CancellationToken ct)
        {
            var schema = @"
{
    ""name"": ""NAME"",
    ""version"": ""1.0.0"",
    ""resources"": {
        ""NAME:index:VirtualMachine"": {
            ""description"": ""A test resource that represents a virtual machine."",
            ""properties"": {
                ""vmname"": {
                    ""$ref"": ""pulumi.json#/Any"",
                    ""description"": ""Name of the virtual machine.""
                }
            },
            ""inputProperties"": {
                ""vmname"": {
                    ""$ref"": ""pulumi.json#/Any"",
                    ""description"": ""Name of the virtual machine.""
                }
            },
            ""type"": ""object""
        }
    }PARAM
}
";
            //            var parameterization = @"
            //,
            //    ""parameterization"": {
            //        ""baseProvider"": {
            //            ""name"": ""testprovider"",
            //            ""version"": ""0.0.1""
            //        },
            //        ""parameter"": ""UTFBYTES""
            //    }
            //";

            // Very hacky schema build just to test out that parameterization calls are made
            //parameterization = parameterization.Replace("UTFBYTES", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(parameter)));
            schema = schema.Replace("NAME", parameter);
            //schema = schema.Replace("PARAM", parameterization);
            schema = schema.Replace("PARAM", "");

            return Task.FromResult(new GetSchemaResponse
            {
                Schema = schema,
            });
        }

        public override Task<CheckResponse> CheckConfig(CheckRequest request, CancellationToken ct)
        {
            return Task.FromResult(new CheckResponse { Inputs = request.NewInputs });
        }

        public override Task<DiffResponse> DiffConfig(DiffRequest request, CancellationToken ct)
        {
            return Task.FromResult(new DiffResponse());
        }

        public override Task<ConfigureResponse> Configure(ConfigureRequest request, CancellationToken ct)
        {
            return Task.FromResult(new ConfigureResponse());
        }

        public override Task<InvokeResponse> Invoke(InvokeRequest request, CancellationToken ct)
        {
            return base.Invoke(request, ct);
        }

        public override Task<CreateResponse> Create(CreateRequest request, CancellationToken ct)
        {
            if (request.Type == parameter + ":index:VirtualMachine")
            {
                var outputs = new Dictionary<string, PropertyValue>
                {
                    { "vmname", request.Properties["vmname"] }
                };

                // get props
                if (!request.Properties["vmname"].TryGetString(out var vmName))
                {
                    throw new Exception($"Missing required param 'vmname'");
                }

                // **********************
                // create VM
                var vmms = new VMMS();

                // Define the Virtual Machine
                var virtualMachineDefinition = new VirtualMachineDefinition(
                    vmName,
                    @"C:\ProgramData\Microsoft\Windows\Hyper-V"
                )
                {
                    Memory =
                    {
                        Startup = 4096
                    },
                    Processor =
                    {
                        Quantity = 2
                    }
                };
                virtualMachineDefinition.ScsiControllers[0].Drives[0] = new VirtualHardDrive(new VirtualHardDisk(
                    VirtualHardDiskFormat.Vhdx,
                    VirtualHardDiskType.FixedSize,
                    1,
                    $@"F:\hyperv\{vmName}.vhdx"
                ));
                //virtualMachineDefinition.ScsiControllers[0].Drives[1] = new VirtualDvdDrive();
                //virtualMachineDefinition.NetworkAdapters[0].VirtualSwitch = "VM Switch Name";
                virtualMachineDefinition.AutomaticStop.Action = AutomaticStopAction.Shutdown;

                // Create the Virtual Machine
                var vmNameId = vmms.CreateVirtualMachine(virtualMachineDefinition);
                // **********************

                // housekeeping

                return Task.FromResult(new CreateResponse {
                    Id = vmNameId,
                    Properties = outputs,
                });
            }

            throw new Exception($"Unknown resource type '{request.Type}'");
        }

        public override Task<ReadResponse> Read(ReadRequest request, CancellationToken ct)
        {
            // RSS TODO

            var response = new ReadResponse {
                Id = request.Id,
                Properties = request.Properties,
            };
            return Task.FromResult(response);
        }

        public override Task<CheckResponse> Check(CheckRequest request, CancellationToken ct)
        {
            if (request.Type == parameter + ":index:VirtualMachine")
                //|| request.Type == parameter + ":index:Random" ||
                //request.Type == parameter + ":index:FailsOnDelete")
            {
                return Task.FromResult(new CheckResponse() { Inputs = request.NewInputs });
            }

            throw new Exception($"Unknown resource type '{request.Type}'");
        }

        public override Task<DiffResponse> Diff(DiffRequest request, CancellationToken ct)
        {
            if (request.Type == parameter + ":index:VirtualMachine") {
                var changes = !request.OldState["vmname"].Equals(request.NewInputs["vmname"]);
                return Task.FromResult(new DiffResponse {
                    Changes = changes,
                    Replaces = ["vmname"]
                });
            }

            throw new Exception($"Unknown resource type '{request.Type}'");
        }

        public override Task<UpdateResponse> Update(UpdateRequest request, CancellationToken ct)
        {
            return base.Update(request, ct);
        }

        public override Task Delete(DeleteRequest request, CancellationToken ct)
        {
            if (request.Type == parameter + ":index:FailsOnDelete")
            {
                throw new Exception("Delete always fails for the FailsOnDelete resource");
            }

            // get props
            if (!request.Properties["vmname"].TryGetString(out var vmName))
            {
                throw new Exception($"Missing required param 'vmname'");
            }

            // **********************
            // delete VM

            var vmms = new VMMS();
            
            vmms.DeleteVirtualMachine(vmName);

            // **********************

            return Task.CompletedTask;
        }

        public override Task<ConstructResponse> Construct(ConstructRequest request, CancellationToken ct)
        {
            return base.Construct(request, ct);
        }

        public override Task<CallResponse> Call(CallRequest request, CancellationToken ct)
        {
            return base.Call(request, ct);
        }
    }
}
