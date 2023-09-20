using System.Runtime.InteropServices;

namespace Botticelli.Audio;

public interface IAnalyzer
{
    public AudioInfo Analyze(string filePath);

    public AudioInfo Analyze(Stream input);
}