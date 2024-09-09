using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // 로고
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    // 타이틀
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgresssTxt;

    private AsyncOperation m_AsyncOperation;

    private void Awake()
    {
        LogoAnim.gameObject.SetActive(true);
        Title.SetActive(false);
    }

    private void Start()
    {
        // 유저 데이터 로드
        UserDataManager.Instance.LoadUserData();

        // 저장된 유저 데이터가 없으면 기본값으로 세팅 후 저장
        if(!UserDataManager.Instance.ExistsSavedData)
        {
            UserDataManager.Instance.SetDefaultUserData();
            UserDataManager.Instance.SaveUserData();
        }

        StartCoroutine(LoadGameCo());
    }

    private IEnumerator LoadGameCo()
    {
        Logger.Log($"{GetType()}::LoadGameCo");

        LogoAnim.Play();
        yield return new WaitForSeconds(LogoAnim.clip.length);

        LogoAnim.gameObject.SetActive(false);
        Title.SetActive(true);

        m_AsyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);
        if(m_AsyncOperation == null)
        {
            Logger.Log("Lobby async loading error.");
            yield break;
        }

        m_AsyncOperation.allowSceneActivation = false;

        LoadingSlider.value = 0.5f;
        LoadingProgresssTxt.text = $"{((int)LoadingSlider.value * 100)}%";
        yield return new WaitForSeconds(0.5f);

        while(m_AsyncOperation.isDone) // 로딩 진행중
        {
            LoadingSlider.value = m_AsyncOperation.progress < 0.5f ? 0.5f : m_AsyncOperation.progress;
            LoadingProgresssTxt.text = $"{((int)LoadingSlider.value * 100)}%";

            // 씬 로딩이 완료 되었다면 로비로 전환하고 코루틴 종료
            if (m_AsyncOperation.progress >= 0.9f) // 유니티에서 이렇게 만듦. progress가 0.9에서 멈춤
            {
                m_AsyncOperation.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }

        // 씬 로딩이 완료 되었다면 로비로 전환하고 코루틴 종료
        if (m_AsyncOperation.progress >= 0.9f) // 유니티에서 이렇게 만듦. progress가 0.9에서 멈춤
        {
            LoadingSlider.value = m_AsyncOperation.progress < 0.5f ? 0.5f : m_AsyncOperation.progress;
            LoadingProgresssTxt.text = $"{((int)LoadingSlider.value * 100)}%";
            m_AsyncOperation.allowSceneActivation = true;
            yield break;
        }
    }
}
