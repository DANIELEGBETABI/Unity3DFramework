﻿#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class SpaceEditorStyles
{
    private static GUISkin _skin;
    public static GUISkin Skin
    {
        get
        {
            if(_skin == null)
                _skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Gizmos/SpaceEditorGUI.guiskin");
            return _skin;
        }
    }
    
    private static GUIStyle _lightBackground;
    public static GUIStyle LightBackground
    {
        get
        {
            if (_lightBackground == null)
                _lightBackground = (GUIStyle)"ProjectBrowserPreviewBg";
            return _lightBackground;
        }
    }

    private static GUIStyle _darkBackground;
    public static GUIStyle DarkBackground
    {
        get
        {
            if (_darkBackground == null)
                _darkBackground = (GUIStyle)"ObjectPickerPreviewBackground";
            return _darkBackground;
        }
    }

    private static GUIStyle _largebuttonRight;
    public static GUIStyle LargeButtonRight
    {
        get
        {
            if (_largebuttonRight == null)
                _largebuttonRight = (GUIStyle)"LargeButtonRight";
            return _largebuttonRight;
        }
    }

    private static GUIStyle _largebuttonMid;
    public static GUIStyle LargeButtonMid
    {
        get
        {
            if (_largebuttonMid == null)
                _largebuttonMid = (GUIStyle)"LargeButtonMid";
            return _largebuttonMid;
        }
    }

    private static GUIStyle _largeButtonLeft;
    public static GUIStyle LargeButtonLeft
    {
        get
        {
            if (_largeButtonLeft == null)
                _largeButtonLeft = (GUIStyle)"LargeButtonLeft";
            return _largeButtonLeft;
        }
    }

    private static GUIStyle _buttonRight;
    public static GUIStyle ButtonRight
    {
        get
        {
            if (_buttonRight == null)
                _buttonRight = (GUIStyle)"ButtonRight";
            return _buttonRight;
        }
    }

    private static GUIStyle _buttonMid;
    public static GUIStyle ButtonMid
    {
        get
        {
            if (_buttonMid == null)
                _buttonMid = (GUIStyle)"ButtonMid";
            return _buttonMid;
        }
    }

    private static GUIStyle _buttonLeft;
    public static GUIStyle ButtonLeft
    {
        get
        {
            if (_buttonLeft == null)
                _buttonLeft = (GUIStyle) "ButtonLeft";
            return _buttonLeft;
        }
    }

    private static GUIStyle _timeLineThumbLine;
    public static GUIStyle TimeLineThumbLine
    {
        get
        {
            if (_timeLineThumbLine == null)
                _timeLineThumbLine = Skin.customStyles.First(s => s.name == "TimelineThumbLine");;
            return _timeLineThumbLine;
        }
    }

    private static GUIStyle _timeLineThumb;
    public static GUIStyle TimeLineThumb
    {
        get
        {
            if (_timeLineThumb == null)
                _timeLineThumb = Skin.customStyles.First(s => s.name == "TimelineThumb");// (GUIStyle)"MeBlendPosition";
            return _timeLineThumb;
        }
    }

    private static GUIStyle _timeLineBackground;
    public static GUIStyle TimeLineBackground
    {
        get
        {
            if (_timeLineBackground == null)
                _timeLineBackground = (GUIStyle)"ShurikenEffectBg";
            return _timeLineBackground;
        }
    }

    private static GUIStyle _bar;
    public static GUIStyle Bar
    {
        get
        {
            if (_bar == null)
                _bar = Skin.customStyles.First(s => s.name == "Bar");
            return _bar;
        }
    }

    private static GUIStyle _frameRangeThumb;
    public static GUIStyle FrameRangeThumb
    {
        get
        {
            if (_frameRangeThumb == null)
                _frameRangeThumb = Skin.customStyles.First(s => s.name == "TimelineBar");
            return _frameRangeThumb;
        }
    }

    private static GUIStyle _timeRangeThumb;
    public static GUIStyle TimeRangeThumb
    {
        get
        {
            if (_timeRangeThumb == null)
                _timeRangeThumb = Skin.customStyles.First(s => s.name == "TimeRangeThumb");
            return _timeRangeThumb;
        }
    }

    private static GUIStyle _timeRangeBackground;
    public static GUIStyle TimeRangeBackground
    {
        get
        {
            if(_timeRangeBackground == null)
                _timeRangeBackground = (GUIStyle)"ShurikenEffectBg";
            return _timeRangeBackground;
        }
    }

    private static Texture2D _activeBackground;
    public static Texture2D ActiveBackground
    {
        get
        {
            if (_activeBackground == null)
            {
                var color = EditorGUIUtility.isProSkin ? new Color(61 / 255.0f, 96 / 255.0f, 145 / 255.0f) : new Color(0.33f, 0.66f, 1f, 0.66f);
                _activeBackground = new Texture2D(1, 1);
                _activeBackground.SetPixel(0, 0, color);
                _activeBackground.Apply();
            }
            return _activeBackground;
        }
    }
}
#endif