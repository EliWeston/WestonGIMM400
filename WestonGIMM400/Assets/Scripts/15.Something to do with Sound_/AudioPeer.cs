using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//doesn't work without some audio source
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    //public GameObject _musicPlayerObject;
    //create a float array of the samples we can use elsewhere
    float[] _samples = new float[512];
    //float[] _samplesRight = new float[512];

    float[] _freqBand = new float[8];
    float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    //highest frequency value played
    public float[] _freqBandHighest = new float[8];
    //values between 0 and 1
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    //all frequencencies into amplitude
    public static float _amplitude, _amplitudeBuffer;
    float _amplitudeHighest;
    public float _audioProfile;



    void Start()
    {
        _audioSource = this.GetComponent<MusicPlayer>().musicSource;
        AudioProfile(_audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    //my understanding is that this adds value to the frequency channels that get low values initially, making the visuals smoother
    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = _audioProfile;
        }
    }

    void GetAmplitude()
    {
        float _currentAmplitude = 0;
        float _currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            _currentAmplitude += _audioBand[i];
            _currentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_currentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _currentAmplitude;
        }
        _amplitude = _currentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = _currentAmplitudeBuffer / _amplitudeHighest;
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    void GetSpectrumAudioSource()
    {
        //samples are waves taken from audio file we chose 512 our of 20,000
        //channel 0
        //****need to look up FFTWindow stuff****
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer()
    //softens the scale changes so the bars dont jump around as much
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        /*
         * 22050 hertz / 512 bands = 43 hertz per sample
         * 
         * 20- 60 hertz
         * 60 - 250 hertz
         * 250 - 500 hertz
         * 500 - 2000 hertz
         * 2000 - 4000 hertz
         * 4000 - 6000 hertz
         * 6000 - 200000 hertz
         * 
         * 0 - 2 = 86 hertz
         * 1 - 4 = 172 hertz - 87 - 258
         * 2 - 8 = 344 hertz - 259 - 602
         * 3 - 16 = 688 hertz - 603 - 1290
         * 4 - 32 = 1376 hertz - 1291 - 2666
         * 5 - 64 = 2752 hertz - 2667 - 5418
         * 6 - 128 = 5504 hertz - 5419 - 10922
         * 7 - 256 = 11008 hertz - 10923 - 21930
         *  510 samples
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            //average of samples combined
            float average = 0;
            //math to get accurate number of samples per bar, 8 times
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            //the last one needs to pick up two samples
            /*if (i == 7)
            {
                sampleCount += 2;
            }*/
            //put the samples in bands
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }

    }
}