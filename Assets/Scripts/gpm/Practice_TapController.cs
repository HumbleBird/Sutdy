using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Practice_TapController : MonoBehaviour
{
    public class SampleTabData : ITabData
    {
        public string text;
    }

    public TabController tabController;
    public Tab tab;


    public void SampleFunc()
    {
        // tabController의 GetTab으로도 Tab을 받아올 수 있습니다.
        // tab = tabController.GetTab(0);

        SampleTabData data = new SampleTabData();
        data.text = "sampleText";

        // Tab에 Data를 넣습니다.
        tab.SetData(data);
    }

    public void SampleFunc2()
    {
        tab.onUpdateData.AddListener(DataSetting);
    }

    public Text buttonText;

    // 설정 함수를 정의합니다.
    public void DataSetting(ITabData updateData)
    {
        // 전달받은 ITabData를 캐스팅해 사용합니다.
        SampleTabData data = (SampleTabData)updateData;

        // Tab UI 업데이트
        buttonText.text = data.text;
    }

    public Text pageText;

    // 설정 함수를 정의합니다.
    public void PageDataSetting(Tab tab)
    {
        // onNotify 이벤트는 연결된 모든 TabPage에 전달됩니다.
        // 선택된 Tab일 경우만 업데이트합니다.
        if (tab.IsSelected() == true)
        {
            // GetData를 통해 전달받은 ITabData를 캐스팅해 사용합니다.
            SampleTabData data = (SampleTabData)tab.GetData();

            // TabPage UI 업데이트
            pageText.text = data.text;
        }
    }

    TabPage tabPage;
    public void TapPageSampleFunc()
    {
        tabPage.onNotify.AddListener(PageDataSetting);
    }
}
