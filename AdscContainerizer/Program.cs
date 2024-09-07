using Mono.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdscContainerizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string snd = string.Empty, output = string.Empty, psxavenc = string.Empty;
            int sampleRate = 0, interleave = 0;
            OptionSet options = new()
            {
                { "s|snd|sound=", "Input sound", s => snd = s },
                { "e|psxavenc=", "Path to psxavenc", e => psxavenc = e },
                { "o|output=", "Output ADSC", o => output = o },
                { "f|sample-rate=", "Sample rate", f => sampleRate = int.Parse(f) },
                { "i|interleave=", "Interleave offset", i => interleave = int.Parse(i) },
            };
            options.Parse(args);

            string tmpFile = Path.Combine(Path.GetTempPath(), "tmp.snd");
            ProcessStartInfo psi = new(psxavenc, $"-t spui -f {sampleRate} -c 2 -i {interleave} {snd} {tmpFile}");
            Process.Start(psi)?.WaitForExit();

            byte[] sndFile = File.ReadAllBytes(tmpFile);
            File.Delete(tmpFile);
            if (interleave < 0x800)
            {
                List<byte> trueFile = [];
                for (int i = 0; i < sndFile.Length; i += 0x800)
                {
                    trueFile.AddRange(sndFile.Skip(i).Take(interleave));
                }
                sndFile = [.. trueFile];
            }
            sndFile[0] = 0x00;
            sndFile[interleave] = 0x00;
            List<byte> header = [];
            header.AddRange(Encoding.ASCII.GetBytes("ADSC"));
            header.AddRange(BitConverter.GetBytes(1));
            header.AddRange(Encoding.ASCII.GetBytes("SShd"));
            header.AddRange(BitConverter.GetBytes(0x18));
            header.AddRange(BitConverter.GetBytes(0x10));
            header.AddRange(BitConverter.GetBytes(sampleRate));
            header.AddRange(BitConverter.GetBytes(2));
            header.AddRange(BitConverter.GetBytes(interleave));
            header.AddRange([0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]);
            header.AddRange(Encoding.ASCII.GetBytes("SSbd"));
            header.AddRange(BitConverter.GetBytes(sndFile.Length));
            header.AddRange(BitConverter.GetBytes(0x1000));
            header.AddRange(new byte[0xFCC]);

            File.WriteAllBytes(output, [.. header, .. sndFile]);
        }
    }
}
