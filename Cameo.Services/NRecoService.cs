using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cameo.Services
{
    public static class NRecoService
    {
        public static void ConvertMedia(string sourceFile, string destinationFile, string format)
        {
            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia(sourceFile, null, destinationFile, format, new ConvertSettings()
            {
                VideoFrameRate = 30,
                VideoFrameSize = FrameSize.vga640x480,
                AudioSampleRate = 44100,
            });
        }
    }
}
