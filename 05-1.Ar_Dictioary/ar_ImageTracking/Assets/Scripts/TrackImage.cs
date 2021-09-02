using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class TrackImage : MonoBehaviour
{
    public float _timer;

    public ARTrackedImageManager manager;

    public List<GameObject> objList = new List<GameObject>();                   //list type = GameObject

    Dictionary<string, GameObject> _prefabfDic = new Dictionary<string, GameObject>();  //키, 벨류 으로 구성된 딕셔너리


    private List<ARTrackedImage> _trackedImg = new List<ARTrackedImage>();
    private List<float> _tractedTimer = new List<float>();

    void Awake()
    {
        foreach (GameObject obj in objList)
        {
            string name = obj.name;                                         //키값 = 이름
            _prefabfDic.Add(name, obj);                                         //이름 , 실제 오브젝트
        }
    }

    private void Update()
    {
        if(_trackedImg.Count >0)
        {
            List<ARTrackedImage> tNumList = new List<ARTrackedImage>();
            for(var i=0; i<_trackedImg.Count; i++)
            {
                if(_trackedImg[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    if(_tractedTimer[i] > _timer)
                    {
                        string name = _trackedImg[i].referenceImage.name;
                        GameObject tObj = _prefabfDic[name];
                        tObj.SetActive(false);
                        tNumList.Add(_trackedImg[i]);
                    }
                    else
                    {
                        _tractedTimer[i] += Time.deltaTime;
                    }
                }
            }

            if(tNumList.Count > 0)
            {
                for(var i = 0; i< tNumList.Count; i++)
                {
                    int num = _trackedImg.IndexOf(tNumList[i]);
                    _trackedImg.Remove(_trackedImg[num]);
                    _tractedTimer.Remove(_tractedTimer[num]);
                }
            }
        }
    }

    private void OnEnable()
    {
        manager.trackedImagesChanged += ImageChanged;
    }
  
    private void OnDisable()
    {
        manager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            if(!_trackedImg.Contains(trackedImage))
            {
                _trackedImg.Add(trackedImage);
                _tractedTimer.Add(0);
            }
            
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (!_trackedImg.Contains(trackedImage))
            {
                _trackedImg.Add(trackedImage);
                _tractedTimer.Add(0);
            }
            else
            {
                int num = _trackedImg.IndexOf(trackedImage);
                _tractedTimer[num] = 0;
            }
            UpdateImage(trackedImage);
        }

    }
    
    private void UpdateImage(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        GameObject obj = _prefabfDic[name];
        obj.transform.position = img.transform.position;            //프리펩의 위치/회전값을 인식한 이미지의 위치 회전값에 맞추기
        obj.transform.rotation = img.transform.rotation;
        obj.SetActive(true);                                        //프리펩을 활성화시킴.
        
    }

  /*public float rotationSpeed = 30f;

    void FixedUpdate()
    {
        GameObject obj = _prefabfDic[name];
        //transform.Rotate( 0f,-rotationSpeed * Time.deltaTime, 0f);
        transform.RotateAround(objList.po, Vector3.up, rotationSpeed*Time.deltaTime);
    }*/
}
