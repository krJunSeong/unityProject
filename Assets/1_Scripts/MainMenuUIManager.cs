using UnityEngine;
using UnityEngine.UI;
public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] Toggle to_isBgmMute;
    [SerializeField] Toggle to_isSfxMute;
    [SerializeField] Slider sl_bgmVolume;
    [SerializeField] Slider sl_sfxVolume;
    [SerializeField] Dropdown dp_screenMode;

    public enum ScreenMode { FULLSCREEN, WINDOW, FULLSCREENWINDOW};

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /*
    사운드매니저에서 각 옵션의 값을 받아오는 함수
    */public void GetOptionValue()
    {
       sl_bgmVolume.value = SoundManager.Instance.GetBgmVolume();
       sl_sfxVolume.value = SoundManager.Instance.GetSfxVolume();
       to_isBgmMute.isOn = !SoundManager.Instance.GetBgmMute();
       to_isSfxMute.isOn = !SoundManager.Instance.GetSfxMute();
    }
    public void SetVolume()
    {
        SoundManager.Instance.SetSfxVolume(sl_sfxVolume.value, !to_isSfxMute.isOn);
        SoundManager.Instance.SetBgmVolume(sl_bgmVolume.value, !to_isBgmMute.isOn);
    }

    public void SetScreenOption()
    {   //더 늘어날 시에는 배열 생성해서 구조체 만들어서 넣고, 그걸 value값으로 컨트롤할 것
        ScreenMode screenSize = (ScreenMode)dp_screenMode.value;
        switch (screenSize)
        {
            case ScreenMode.FULLSCREEN:
                Screen.fullScreen = true;
                break;

            case ScreenMode.WINDOW:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(1280, 720, false);
                break;

            case ScreenMode.FULLSCREENWINDOW:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                break;
        }
    }
}
