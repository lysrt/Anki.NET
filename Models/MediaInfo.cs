using System;
using System.Globalization;

namespace AnkiSharp
{
    public class MediaInfo
    {
        public CultureInfo cultureInfo;
        public string field;
        public string extension = ".wav";
            
        //"Need migrate to .Net 5"
        //public SpeechAudioFormatInfo audioFormat = new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
    }
}
