using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace CalculatorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        [HttpGet("Calculate/{output}/{vendor}/{material}/{configuration}/{print}/{zipper}/{thickness}/{width}/{length}/{gusset}/{quantity}")]
        public string Calculate(string output, string vendor, string material, string configuration, string print, string zipper, int thickness, float width, float length, float gusset, int quantity)
        {
            //NOTE: business rules are in caclulator javascript
            if (TryCalculate(output, vendor, material, configuration, print, zipper, thickness, width, length, gusset, quantity, out string result)) return result;
            else return "Error encountered in python script: " + result;
        }

        [HttpGet("Calculate/Test")]
        public string CalculateTest()
        {
            var response = new StringBuilder();
            var allSucceeded = true;
            string vendor = "Glen";
            string material = "PET/PE";
            string configuration = "3-Seal";
            string print = "Plate";
            string zipper = "Yes";
            int thickness = 110;
            float width = 76;
            float length = 127;
            float gusset = 0;
            int quantity = 100000;

            response.AppendLine();
            response.AppendLine();
            response.AppendLine("Testing price:");
            if (TryCalculate("price", vendor, material, configuration, print, zipper, thickness, width, length, gusset, quantity, out string result))
            {
                response.AppendLine("success!");
                response.AppendLine("value: " + result);
            }
            else
            {
                response.AppendLine("failure!");
                response.AppendLine("Python error: " + result);
                allSucceeded = false;
            }
            response.AppendLine();
            response.AppendLine();
            response.AppendLine("Testing price+tax:");
            if (TryCalculate("price+tax", vendor, material, configuration, print, zipper, thickness, width, length, gusset, quantity, out result))
            {
                response.AppendLine("success!");
                response.AppendLine("value: " + result);
            }
            else
            {
                response.AppendLine("failure!");
                response.AppendLine("Python error: " + result);
                allSucceeded = false;
            }
            response.AppendLine();
            response.AppendLine();
            response.AppendLine("Testing price+tax+freight:");
            if (TryCalculate("price+tax+freight", vendor, material, configuration, print, zipper, thickness, width, length, gusset, quantity, out result))
            {
                response.AppendLine("success!");
                response.AppendLine("value: " + result);
            }
            else
            {
                response.AppendLine("failure!");
                response.AppendLine("Python error: " + result);
                allSucceeded = false;
            }
            response.AppendLine();
            response.AppendLine();
            response.AppendLine("Testing weight:");
            if (TryCalculate("weight", vendor, material, configuration, print, zipper, thickness, width, length, gusset, quantity, out result))
            {
                response.AppendLine("success!");
                response.AppendLine("value: " + result);
            }
            else
            {
                response.AppendLine("failure!");
                response.AppendLine("Python error: " + result);
                allSucceeded = false;
            }
            response.AppendLine();
            response.AppendLine();
            if (allSucceeded) return response.ToString();
            else throw new PythonException(response.ToString());
        }

        [HttpGet("Python/Pip/Version")]
        public string GetPythonPipVersion()
        {
            return PythonInterop.Execute("-m pip --version");
        }

        [HttpGet("Python/Version")]
        public string GetPythonVersion()
        {
            return PythonInterop.Execute("--version");
        }

        [HttpGet("Preview")]
        public ContentResult Preview()
        {
            var file = System.IO.File.ReadAllText("preview.html");
            return Content(file, "text/html", Encoding.UTF8);
        }

        [HttpGet("Python/Integration")]
        public string TestPythonIntegration()
        {
            var args = string.Format("IntegrationTest.py");
            return PythonInterop.Execute(args);
        }

        [HttpGet("Service/Running")]
        public string TestServiceLive()
        {
            return "Service is live!";
        }

        [HttpGet("Python/Pip/Upgrade")]
        public string UpgradePythonPipVersion()
        {
            return PythonInterop.Execute("-m pip install --upgrade pip");
        }

        private string InstallEnvironmentPackages()
        {
            var results = new StringBuilder();
            using (var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Python\\environment.yml"))
            {
                var yaml = new YamlStream();
                yaml.Load(reader);
                var root = yaml.Documents[0].RootNode as YamlMappingNode;
                var dep = root.Children["dependencies"] as YamlSequenceNode;
                var pip = dep.Children.FirstOrDefault(ch => ch is YamlMappingNode) as YamlMappingNode;
                var packages = (pip.Children.Values.First() as YamlSequenceNode).Cast<YamlScalarNode>().Select(p => p.Value);
                foreach (var package in packages)
                {
                    var r = PythonInterop.Execute($"-m pip install \"{package}\"");
                    results.Append(r + "\n");
                }
            }
            return results.ToString();
        }

        private bool TryCalculate(string outputType, string vendor, string material, string configuration, string print, string zipper, int thickness, float width, float length, float gusset, int quantity, out string message)
        {
            //var args = "Glen PET/PE 3-Seal Plate Yes 110 76 127 0 100000";
            const string deliminator = "|||RESULT|||";
            vendor = vendor.Replace("-", "/").Replace(",", "/").Replace("_", "/").Replace(" ", "/");
            material = material.Replace("-", "/").Replace(",", "/").Replace("_", "/").Replace(" ", "/");
            var args = $"{outputType} {vendor} {material} {configuration} {print} {zipper} {thickness} {width} {length} {gusset} {quantity}";
            var result = PythonInterop.Execute("shim.py " + args);
            var indexes = result.IndexesOf(deliminator).ToArray();
            if (indexes.Length == 2)
            {
                var start = indexes[0] + deliminator.Length;
                message = result.Substring(start, indexes[1] - start).Trim('\r', '\n', ' ');
                return true;
            }
            else
            {
                message = result;
                return false;
            }
        }
    }

    [Serializable]
    internal class PythonException : Exception
    {
        public override string StackTrace { get { return "[From Python]"; } }
        public PythonException()
        {
        }

        public PythonException(string message) : base(message)
        {
        }

        public PythonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PythonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}