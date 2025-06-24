#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextToTMPConverter : MonoBehaviour
{
    [MenuItem("Tools/Convert Text To TMP")]
    public static void ConvertTextComponents()
    {
        int count = 0;
        Text[] texts = GameObject.FindObjectsOfType<Text>();

        foreach (Text oldText in texts)
        {
            GameObject go = oldText.gameObject;

            // 백업값 저장
            string content = oldText.text;
            Font font = oldText.font;
            int fontSize = oldText.fontSize;
            Color color = oldText.color;
            TextAnchor alignment = oldText.alignment;
            RectTransform rt = oldText.GetComponent<RectTransform>();

            // 기존 Text 제거
            DestroyImmediate(oldText);

            // TMP 컴포넌트 추가
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = content;
            tmp.fontSize = fontSize;
            tmp.color = color;
            tmp.alignment = ConvertAlignment(alignment);
            tmp.enableAutoSizing = false;

            // 기본 폰트 자산 없으면 TMP Essentials 먼저 임포트해야 함
            if (TMP_Settings.defaultFontAsset != null)
                tmp.font = TMP_Settings.defaultFontAsset;

            count++;
        }

        Debug.Log($"[TMP 변환기] Text → TMP로 {count}개 변환 완료!");
    }

    static TextAlignmentOptions ConvertAlignment(TextAnchor anchor)
    {
        return anchor switch
        {
            TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
            TextAnchor.UpperCenter => TextAlignmentOptions.Top,
            TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
            TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
            TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
            TextAnchor.MiddleRight => TextAlignmentOptions.Right,
            TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
            TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
            TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
            _ => TextAlignmentOptions.Center,
        };
    }
}
#endif