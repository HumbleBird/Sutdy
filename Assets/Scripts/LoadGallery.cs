using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadGallery : MonoBehaviour
{
    public RawImage img;

    public void Start()
    {
        Debug.Log(Application.persistentDataPath);

        string imagePath = Application.persistentDataPath + "/Image/저장.png";
        if (File.Exists(imagePath))
        {
            var temp = File.ReadAllBytes(imagePath);
            SetImage(temp);
        }
    }

    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) => 
        { 
            FileInfo selected = new FileInfo(file);

            // 용량 제한
            if (selected.Length > 50000000) // 5천만 바이트
                return;

            // 불러오기
            if(!string.IsNullOrEmpty(file))
            {
                // 불러와리.
                StartCoroutine(LoadImage(file));
            }


        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string name = "저장";
        string savePath = Application.persistentDataPath + "/Image/";

        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllBytes(savePath + name + ".png", fileData);

        var temp = File.ReadAllBytes(savePath + name + ".png");

        SetImage(temp);
    }

    void SetImage(byte[] temp)
    {
        
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(temp);

        img.texture = tex;
        img.SetNativeSize();
        ImageSizeSetting(img, 800, 800);
    }

    void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if( x / y > imgX/ imgY) // 이미지의 세로길이가 더 길다
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y); 
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgY * (y/imgY));
        } 
        else // 가로길이가 더 길다.
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgY * (x / imgX));
        }
    }
}
