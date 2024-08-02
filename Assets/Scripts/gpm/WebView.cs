using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.WebView;
using static Gpm.WebView.GpmWebViewCallback;
using static Gpm.WebView.GpmWebViewRequest;
using static UnityEngine.Application;
using UnityEngine.Profiling;

public class WebView : MonoBehaviour
{
    public string url ="https://m.naver.com/";
    private int width = 1008;
    private int height = 567;
    private int x = 0;
    private int y = 0;

    public void ShowWebsite()
    {
        GpmWebView.ShowUrl(url,
        GetConfiguration(),
        // See the end of the code example
        OnCallback,
        new List<string>()
        {
            "USER_ CUSTOM_SCHEME"
        });
    }

    public void GoBack()
    {
        if(GpmWebView.CanGoBack())
        {
            GpmWebView.GoBack();
        }
    }

    public void GoForward()
    {
        if (GpmWebView.CanGoForward())
        {
            GpmWebView.GoForward();
        }
    }


    private void OnCallback(
        GpmWebViewCallback.CallbackType callbackType,
        string data,
        GpmWebViewError error)
    {
        Debug.Log("OnCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("PageStarted Url : {0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                if (error == null)
                {
                    if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
                    {
                        Debug.Log(string.Format("scheme:{0}", data));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
                }
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
#if UNITY_ANDROID
            case GpmWebViewCallback.CallbackType.BackButtonClose:
                Debug.Log("BackButtonClose");
                break;
#endif
        }
    }

    // App에서 Android Chrome 또는 iOS Safari 브라우저를 표시합니다.
    // GpmWebViewSafeBrowsing class를 사용합니다.
    public void OpenSafeBrowsing()
    {
        string sampleUrl = "";
        GpmWebViewSafeBrowsing.ShowSafeBrowsing(sampleUrl,
            new GpmWebViewRequest.ConfigurationSafeBrowsing()
            {
                navigationBarColor = "#4B96E6",
                navigationTextColor = "#FFFFFF"
            },
            OnCallback);
    }

    private GpmWebViewRequest.Configuration GetConfiguration()
    {
        return new Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            backgroundColor = "#FFFFFF",
            isNavigationBarVisible = false,
            navigationBarColor = "#00000000",
            title = "",
            isBackButtonVisible = true,
            isForwardButtonVisible = false,
            isCloseButtonVisible = false,
            supportMultipleWindows = true,
            position = new Position
            {
                hasValue = true,
                x = this.x,
                y = this.y
            },
            size = new Size
            {
                hasValue = true,
                width = this.width,
                height = this.height
            },
        };
    }

}
