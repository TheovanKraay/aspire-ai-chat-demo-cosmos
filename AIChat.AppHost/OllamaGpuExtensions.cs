using Aspire.Hosting;
using CommunityToolkit.Aspire.Hosting.Ollama;

namespace AIChat.AppHost;

public static class OllamaGpuExtensions
{
    /// <summary>
    /// Enables GPU support for the Ollama container if the specified GPU vendor is supported.
    /// </summary>
    /// <param name="builder">The Ollama resource builder.</param>
    /// <param name="vendor">The GPU vendor to check for (defaults to Nvidia).</param>
    /// <returns>The original builder with GPU support added if available.</returns>
    public static IResourceBuilder<OllamaResource> WithGPUIfSupported(
        this IResourceBuilder<OllamaResource> builder,
        OllamaGpuVendor vendor = OllamaGpuVendor.Nvidia)
    {
        return vendor switch
        {
            OllamaGpuVendor.Nvidia when SystemSupportsNvidiaGpu() =>
                builder.WithGPUSupport(OllamaGpuVendor.Nvidia),

            OllamaGpuVendor.AMD when SystemSupportsAmdGpu() =>
                builder.WithGPUSupport(OllamaGpuVendor.AMD),

            _ => builder // unsupported or not detected
        };
    }

    private static bool SystemSupportsNvidiaGpu()
    {
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "nvidia-smi",
                Arguments = "-L",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null) return false;

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static bool SystemSupportsAmdGpu()
    {
        try
        {
            // Very naive AMD detection for now; can be expanded with rocminfo checks
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "bash",
                Arguments = "-c \"lspci | grep -i amd\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null) return false;

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.Contains("AMD", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
