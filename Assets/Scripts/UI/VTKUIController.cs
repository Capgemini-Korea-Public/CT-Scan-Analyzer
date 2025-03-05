using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VTKUIController : MonoBehaviour
{
    [SerializeField] private VTKSceneObject vtkSceneObject;
    [SerializeField] private TMP_Dropdown orientationDropdown;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValText;

    private void Start()
    {
        if (vtkSceneObject != null)
            Init((float)vtkSceneObject.Slice);

        slider.onValueChanged.AddListener(OnSliceChanged);
        orientationDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void Init(float value)
    {
        slider.value = value;
        sliderValText.text = value.ToString("F2");

        orientationDropdown.ClearOptions();
        List<string> options = new List<string> { "I", "J", "K" };
        orientationDropdown.AddOptions(options);

        orientationDropdown.value = (int)vtkSceneObject.Orientation;
        orientationDropdown.RefreshShownValue();
    }

    void OnSliceChanged(float value)
    {
        if (vtkSceneObject != null)
            vtkSceneObject.Slice = value;
        sliderValText.text = value.ToString("F2");
    }

    void OnDropdownChanged(int value)
    {
        if (vtkSceneObject != null)
            vtkSceneObject.Orientation = (VTKSceneObject.SliceOrientation)value;

        slider.value = 0;
        sliderValText.text = 0.ToString("F2");
    }
}
