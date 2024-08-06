using Gpm.Adapter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sample_Adapter : MonoBehaviour
{
    public TMP_InputField input;
    //public TextMeshProUGUI idpName;

    private void SampleIsSucces(AdapterError error)
    {
        if (GpmAdapter.IsSuccess(error) == true)
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log(string.Format("failure. error:{0}", error));
        }
    }

    public void SampleLogin()
    {
        Dictionary<string, object> additionalInfo;

        string idpName = input.text;

        switch (idpName.ToString())
        {
            case GpmAdapterType.IdP.FACEBOOK:
                {
                    var facebookPermissionList = new List<string> { "public_profile", "email" };
                    additionalInfo = new Dictionary<string, object>();
                    additionalInfo.Add("facebook_permissions", facebookPermissionList);
                    break;
                }
            case GpmAdapterType.IdP.GPGS:
            default:
                {
                    additionalInfo = null;
                    break;
                }
        }

        GpmAdapter.IdP.Login(GpmAdapterType.IdP.FACEBOOK, additionalInfo, (error) =>
        {
            if (GpmAdapter.IsSuccess(error) == true)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log(string.Format("failure. error:{0}", error));
            }

            SampleIsSucces(error);
        });
    }

    public void SampleLogout()
    {
        GpmAdapter.IdP.Logout(GpmAdapterType.IdP.FACEBOOK, (error) =>
        {
            if (GpmAdapter.IsSuccess(error) == true)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log(string.Format("failure. error:{0}", error));
            }
        });
    }

    public void SampleLogoutAll()
    {
        GpmAdapter.IdP.LogoutAll((error) =>
        {
            if (GpmAdapter.IsSuccess(error) == true)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log(string.Format("failure. error:{0}", error));
            }
        });
    }

    public void SampleGetAuthInfo()
    {
        GpmAdapter.IdP.GetAuthInfo(GpmAdapterType.IdP.FACEBOOK, (facebookAuthInfo) =>
        {
            Debug.Log(string.Format("authInfo:{0}", facebookAuthInfo));
        });
    }

    public void SampleGetProfile()
    {
        GpmAdapter.IdP.GetProfile(GpmAdapterType.IdP.FACEBOOK, (facebookProfile) =>
        {
            if (facebookProfile == null)
            {
                Debug.Log("Facebook profile is null.");
            }
            else
            {
                foreach (KeyValuePair<string, object> kvp in facebookProfile)
                {
                    Debug.Log(string.Format("{0}:{1}\n", kvp.Key, kvp.Value));
                }
            }
        });
    }

    public void SampleGetLoggedInIdpList()
    {
        var loggedInIdpList = GpmAdapter.IdP.GetLoggedInIdPList();
        foreach (var loggedInIdp in loggedInIdpList)
        {
            Debug.Log(string.Format("loggedInIdp:{0}", loggedInIdp));
        }
    }

    public void SampleGetUserId()
    {
        var facebookUserId = GpmAdapter.IdP.GetUserId(GpmAdapterType.IdP.FACEBOOK);
        Debug.Log(string.Format("facebookUserId:{0}", facebookUserId));
    }
}
