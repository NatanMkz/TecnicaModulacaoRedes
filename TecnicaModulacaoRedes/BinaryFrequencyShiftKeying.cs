using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace TecnicaModulacaoRedes
{
    public class BinaryFrequencyShiftKeying
    {
        private int frequencySample;
        private double time;
        private double frequency1;
        private double frequency2;

        public BinaryFrequencyShiftKeying(int frequencySample, double time, double frequency1, double frequency2)
        {
            this.frequencySample = frequencySample;
            this.time = time;
            this.frequency1 = frequency1;
            this.frequency2 = frequency2;
        }

        private Tuple<List<double>, List<double>> Sin(double frequency)
        {
            var time = Enumerable.Range(0, frequencySample).Select(i => this.time + (double)i / frequencySample * 0.1).ToList();
            var signal = time.Select(t => Math.Sin(2 * Math.PI * frequency * t)).ToList();
            return Tuple.Create(time, signal);
        }

        public Modulation Modulate(string message)
        {
            var binaryMessage = StringToBinary(message);
            var modulation = new Modulation();
            modulation.Frequencies = binaryMessage.Select(bit => bit == '1' ? frequency1 : frequency2).ToList();
            modulation.Signals = new List<double>();
            for (int i = 0; i < modulation.Frequencies.Count; i++)
            {
                var (time, signal) = Sin(modulation.Frequencies[i]);
                time = time.Select(t => t + i * 0.1).ToList();
                modulation.Signals.AddRange(signal);
            }
            return modulation;
        }

        public string Demodulate(Modulation modulatedSignal)
        {
            var demodulatedMessage = "0b";
            for (int i = 0; i < modulatedSignal.Frequencies.Count; i++)
            {
                var start = i * frequencySample;
                var end = (i + 1) * frequencySample;
                var signals = modulatedSignal.Signals.GetRange(start, frequencySample);
                var time = 1.0 / frequencySample;
                var signalSize = signals.Count;
                var fftFrequencies = Enumerable.Range(0, signalSize).Select(j => (double)j / (signalSize * time)).ToList();
                var frequencies = fftFrequencies.Take(signalSize / 2).Select(f => f * 100).ToList();
                var amplitudes = signals.Select(s => Complex.Abs(new Complex(s, 0))).Take(signalSize / 2).Select(a => a / signalSize).ToList();
                demodulatedMessage += frequencies[amplitudes.IndexOf(amplitudes.Max())] / 10 == frequency2 ? '0' : '1';
            }
            long bit = Convert.ToInt64(demodulatedMessage.Substring(2), 2);
            return BinaryToString(bit);
        }

        public static string StringToBinary(string str)
        {
            var bytes = Encoding.BigEndianUnicode.GetBytes(str);
            return Convert.ToString(BitConverter.ToInt32(bytes, 0), 2);
        }

        public static string BinaryToString(long num)
        {
            var bytes = BitConverter.GetBytes(num);
            int numBytes = (int)Math.Ceiling(Convert.ToDecimal(num) / 8);
            //Array.Resize(ref bytes, numBytes);
            //Array.Reverse(bytes);
            var sb = new StringBuilder(bytes.Length);
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                byte b = bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    sb.Append(((b >> j) & 1) == 1 ? '1' : '0');
                }
            }
            int bit = Convert.ToInt32(sb.ToString().Substring(2), 2);

            var byteConvertido = BitConverter.GetBytes(num);
            //return sb.ToString();

            string ret = Encoding.BigEndianUnicode.GetString(byteConvertido);
            //Console.WriteLine(ret);
            //string ret = Convert.ToBase64String(bytes);
            return ret;
        }
    }
}

