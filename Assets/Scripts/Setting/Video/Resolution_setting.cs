using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Resolution_setting : MonoBehaviour
{
    public TMP_Dropdown Resolution_Dropdown;

    List<Resolution> resolutions;

    private void OnEnable()
    {
        SetResolutionOptions();
    }

    void SetResolutionOptions()
    {
        // 모든 해상도를 가져오고 16:9 비율의 해상도만 선택
        resolutions = new List<Resolution>(Screen.resolutions);

        // 해상도를 필터링해 16:9 비율 중 가장 높은 주사율을 가지는 해상도만 남김
        resolutions = resolutions
            .Where(x => Mathf.Approximately((float)x.width / x.height, 16f / 9)) // 16:9 비율 필터
            .GroupBy(x => new { x.width, x.height }) // 해상도로 그룹화
            .Select(g => g.OrderByDescending(x => x.refreshRateRatio.numerator).First()) // 각 그룹에서 최대 주사율 선택
            .ToList();

        // 옵션 목록을 설정
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            var resolution = resolutions[i];
            uint refreshRate = resolution.refreshRateRatio.numerator; // 주사율 가져오기
            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            // 현재 해상도와 일치하는 인덱스를 저장
            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height &&
                refreshRate == Screen.currentResolution.refreshRateRatio.numerator)
            {
                currentResolutionIndex = i;
            }
        }

        // 드롭다운 초기화 및 설정
        Resolution_Dropdown.ClearOptions();
        Resolution_Dropdown.AddOptions(options);

        //저장된 값이 모니터가 출력 할 수 있는 해상도랑 일치하면 할당
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            var _resolution = resolutions[PlayerPrefs.GetInt("resolutionIndex")];
            string _option = $"{_resolution.width} x {_resolution.height}";

            if (_option == PlayerPrefs.GetString("resolutionValue"))
            {
                currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            }
        }

        Resolution_Dropdown.value = currentResolutionIndex;
        Resolution_Dropdown.RefreshShownValue();

        // 드롭다운 값 변경 시 이벤트 연결
        Resolution_Dropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetResolution(int resolutionIndex)
    {
        // 선택된 해상도를 설정
        Resolution selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);

        PlayerPrefs.SetString("resolutionValue", $"{selectedResolution.width} x {selectedResolution.height}");
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }
}
