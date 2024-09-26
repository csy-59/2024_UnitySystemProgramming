using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsUI : BaseUI
{
    public TextMeshProUGUI GameVersionTxt;

    public GameObject SoundOnToggle;
    public GameObject SoundOffToggle;

    private const string PRIVACY_POLICY_URL = "https://www.inflearn.com/";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetGameVersion();

        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingData>();
        if (userSettingsData != null)
        {
            SetSoundSetting(userSettingsData.Sound);
        }
    }


    private void SetGameVersion()
    {
        GameVersionTxt.text = $"Version:{Application.version}";
    }

    private void SetSoundSetting(bool sound)
    {
        SoundOnToggle.SetActive(sound);
        SoundOffToggle.SetActive(sound == false);
    }

    public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        if (userSettingData != null)
        {
            userSettingData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            SetSoundSetting(userSettingData.Sound);
        }
    }

    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOffToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        if (userSettingData != null)
        {
            userSettingData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.UnMute();
            SetSoundSetting(userSettingData.Sound);
        }
    }

    public void OnClickPrivacyPolicyURL()
    {
        Logger.Log($"{GetType()}::OnClickPrivacyPolicyURL");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        Application.OpenURL(PRIVACY_POLICY_URL);
    }
}
