using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtension
{
    private static Dictionary<Button, Color[ ]> dicTextColor = new Dictionary<Button, Color[ ]>( );
    private static Dictionary<Image, Color[ ]> dicTextColorByImage = new Dictionary<Image, Color[ ]>( );

    // 버튼 GrayScale 적용
    public static void SetInteractalbeWithGrayscale(this Button btn, bool interactable)
    {
        btn.interactable = interactable;

        Image img = btn.GetComponent<Image>( );
        if (img != null)
        {
            img.material = new Material(Shader.Find("Grayscale"));
            img.material.SetFloat("_GrayscaleAmount", interactable == false ? 1 : 0);
        }
    }
}