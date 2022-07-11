using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SoundType;
public class Sound : MonoBehaviour
{
    //�⺻������ ������ ������ͼ�. �׷����� eff�� bgm�� ������ ���� ��
    //���� ���� �׷��� �ν����Ϳ��� volume ��Ŭ�� -> expose
    //exposed parameters ���� ���� �Ķ���� �̸��� �ٲ��� ��.
    [SerializeField]
    AudioMixer audioMixer = null;
    //���� ������ �����̴�.
    [Header("���� ������ �����̴�")]
    [SerializeField]
    Slider bgmSlider = null;
    [SerializeField]
    Slider effSlider = null;
    [SerializeField]
    Slider masterSlider = null;
    [Header("�����ų Ŭ��")]
    [SerializeField]
    private List<AudioClip> effAudioClips = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> bgmAudioClips = new List<AudioClip>();
    [Header("����� �ͼ� �׷�")]
    [SerializeField]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup effMixerGroup;

    private List<AudioSource> SoundsEff = new List<AudioSource>();
    private List<AudioSource> SoundsBgm = new List<AudioSource>();

    //exposed parameters�� �Ķ���͵��� �̸�.
    string bgm_Group = "BGM";
    string eff_Group = "EFF";
    string master_Group = "MASTER";

    private AudioSource lastPlayBgm;
    private void Awake()
    {
        SetSource();
        SetAddListener();
    }
    private void Update()
    {
        //��� ����
        if (Input.GetKeyDown(KeyCode.E))
            PlayEff(SoundType.EffType.Example);
        if (Input.GetKeyDown(KeyCode.R))
            PlayBgm(SoundType.BgmType.Example);
    }
    private void SetAddListener()
    {
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        effSlider.onValueChanged.AddListener(SetEffVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    //������ �����ϴ� �Լ�. 
    private void SetBgmVolume(float volume)
    {
        if (volume == 0) //������ 0�̸�  log����� 1�� ó���Ǳ� ������ ũ�� ��� ���� �߻�. ����ó��
        {
            audioMixer.SetFloat(bgm_Group, -80);
            return;
        }
        audioMixer.SetFloat(bgm_Group, Mathf.Log10(volume) * 20);
        //�����̴��� ���� ������������ ����� �ͼ��� �α� �������̱� ������ ��ȯ������ ��ģ��.
    }
    private void SetEffVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat(eff_Group, -80);
            return;
        }
        audioMixer.SetFloat(eff_Group, Mathf.Log10(volume) * 20);
    }
    private void SetMasterVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat(master_Group, -80);
            return;
        }
        audioMixer.SetFloat(master_Group, Mathf.Log10(volume) * 20);
    }
    private void SetSource()
    {
        int i = 1;
        foreach (AudioClip clip in effAudioClips)
        {
            var obj = new GameObject().AddComponent<AudioSource>();
            obj.name = "Eff " + i;
            obj.playOnAwake = false;
            obj.outputAudioMixerGroup = effMixerGroup;
            obj.clip = clip;
            SoundsEff.Add(obj);
            obj.transform.SetParent(this.transform);
            i++;
        }
        i = 1;
        foreach (AudioClip clip in bgmAudioClips)
        {
            var obj = new GameObject().AddComponent<AudioSource>();
            obj.name = "Bgm " + i;
            obj.playOnAwake = false;
            obj.clip = clip;
            obj.loop = true;
            obj.outputAudioMixerGroup = bgmMixerGroup;

            SoundsBgm.Add(obj);
            obj.transform.SetParent(this.transform);
            i++;
        }

    }
    public void PlayBgm(SoundType.BgmType value)
    {
        Debug.Log("play Bgm");
        lastPlayBgm?.Stop();
        SoundsBgm[(int)value].Play();
        lastPlayBgm = SoundsBgm[(int)value];
    }
    public void PlayEff(SoundType.EffType value)
    {
        Debug.Log("play eff");
        SoundsEff[(int)value].Play();
    }
}
