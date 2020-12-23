using System.Globalization;
using Microsoft.CognitiveServices.Speech;

namespace AnkiSharp.Helpers
{
    //"Need migrate to .Net 5"
    internal static class SynthetizerHelper
    {
        //internal static void CreateAudio(string path, string text, CultureInfo cultureInfo, SpeechAudioFormatInfo audioFormat)
        //{
        //    using (SpeechSynthesizer synth = new SpeechSynthesizer())
        //    {
        //        synth.SetOutputToWaveFile(path, audioFormat);

        //        PromptBuilder builder = new PromptBuilder(cultureInfo);
        //        builder.AppendText(text);
                
        //        synth.Speak(builder);
        //    }
        //}

    }
}
