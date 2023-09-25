using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botticelli.Audio.Exceptions
{
    public class AudioConvertorException : Exception
    {
        public AudioConvertorException(string message, Exception ex) : base(message, ex) { }
    }
}
