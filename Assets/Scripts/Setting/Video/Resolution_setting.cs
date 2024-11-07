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
        // ��� �ػ󵵸� �������� 16:9 ������ �ػ󵵸� ����
        resolutions = new List<Resolution>(Screen.resolutions);

        // �ػ󵵸� ���͸��� 16:9 ���� �� ���� ���� �ֻ����� ������ �ػ󵵸� ����
        resolutions = resolutions
            .Where(x => Mathf.Approximately((float)x.width / x.height, 16f / 9)) // 16:9 ���� ����
            .GroupBy(x => new { x.width, x.height }) // �ػ󵵷� �׷�ȭ
            .Select(g => g.OrderByDescending(x => x.refreshRateRatio.numerator).First()) // �� �׷쿡�� �ִ� �ֻ��� ����
            .ToList();

        // �ɼ� ����� ����
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            var resolution = resolutions[i];
            uint refreshRate = resolution.refreshRateRatio.numerator; // �ֻ��� ��������
            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            // ���� �ػ󵵿� ��ġ�ϴ� �ε����� ����
            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height &&
                refreshRate == Screen.currentResolution.refreshRateRatio.numerator)
            {
                currentResolutionIndex = i;
            }
        }

        // ��Ӵٿ� �ʱ�ȭ �� ����
        Resolution_Dropdown.ClearOptions();
        Resolution_Dropdown.AddOptions(options);

        //����� ���� ����Ͱ� ��� �� �� �ִ� �ػ󵵶� ��ġ�ϸ� �Ҵ�
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

        // ��Ӵٿ� �� ���� �� �̺�Ʈ ����
        Resolution_Dropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetResolution(int resolutionIndex)
    {
        // ���õ� �ػ󵵸� ����
        Resolution selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);

        PlayerPrefs.SetString("resolutionValue", $"{selectedResolution.width} x {selectedResolution.height}");
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }
}
