namespace Botticelli.Talks
{
    public interface ISpeaker
    {
        public Task<byte[]> Speak(string markedText, string language);
    }
}