namespace Botticelli.Talks
{
    public interface ISpeaker
    {
        /// <summary>
        /// Speaking
        /// </summary>
        /// <param name="markedText">SSML-marked text</param>
        /// <param name="lang">Language</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public Task<byte[]> Speak(string markedText, 
                                  string voice, 
                                  string lang, 
                                  CancellationToken token);
        public Task<byte[]> Speak(string markedText, CancellationToken token);

    }
}