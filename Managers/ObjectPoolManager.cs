using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager
{
    Transform m_Root = null;

    Dictionary<string, Queue<GameObject>> m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();

    Dictionary<string, GameObject> m_PoolObject = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> m_PoolParent = new Dictionary<string, GameObject>();

    GameObject CreateObject(GameObject _Prefab)
    {
        return UnityEngine.Object.Instantiate(_Prefab, new Vector3(0f,0f,0f), Quaternion.identity);
    }

    void InitPooling()
    {
        if (null == m_Root)
        {
            m_Root = new GameObject("@PoolRoot").transform;
            UnityEngine.Object.DontDestroyOnLoad(m_Root);
        }
    }

    bool FindPoolQueue(string _strName, out Queue<GameObject> _Queue)
    {
        if (!m_PoolDictionary.TryGetValue(_strName, out _Queue))
            return false;

        return true;
    }


    //Ǯ�� ������Ű������ �뵵�� �Լ�
    public void CreatePool(string _strName, GameObject _Prefab, int _isize = 5)
    {
        //Pool������Ʈ�� �ֻ��� �θ� �ʱ�ȭ �� ���� ���� �Ҵ�
        InitPooling();

        //���ӰԵ��� Key���� �̹� �ִٸ� return
        if (m_PoolDictionary.ContainsKey(_strName))
            return;

        //���Ӱ� ������ Pool��ü�� ���� Queue
        Queue<GameObject> TempPool = new Queue<GameObject>();

        //Pool��ü���� �θ� ������Ʈ(�̸� ���� �����ϱ�����)
        GameObject ParentObject = new GameObject(_strName + "_Pool");

        //�ܺο��� ���� �Ű����� ���� �ݺ��ؼ� ������Ű������
        for (int i = 0; i < _isize; ++i)
        {
            //�������� �޾Ƽ� ���Ӱ� �ν��Ͻ�ȭ
            GameObject InputObject = CreateObject(_Prefab);

            //������Ʈ �ڿ� (1) ~ (n) �ٿ��ִ°� ����� ���� �Լ�
            Util.ObjectCloneNameRemove(InputObject);

            //������ ������Ʈ ��Ȱ��ȭ
            InputObject.SetActive(false);

            //�θ� ������Ʈ�� ������ �α�����
            InputObject.transform.SetParent(ParentObject.transform);

            //Poll Queue�� ����
            TempPool.Enqueue(InputObject);
        }

        //�θ� ������Ʈ�� Root������Ʈ ������ �α�����
        ParentObject.transform.SetParent(m_Root);

        //�θ� ������Ʈ�� �����ϱ����� �θ� ������
        m_PoolParent.Add(_strName, ParentObject);

        //Get�� �� �� ���� Pool���� ������Ʈ�� ������ ���� ������ ������ ����
        m_PoolObject.Add(_strName, _Prefab);

        //Pool��ü�� Dictionary ������ ���Խ��� ����
        m_PoolDictionary.Add(_strName, TempPool);

    }

    public void CreatePool(string _strName, GameObject _Prefab, Action<GameObject> _Funiton, int _isize = 5)
    {
        InitPooling();

        if (m_PoolDictionary.ContainsKey(_strName))
            return;

        Queue<GameObject> TempPool = new Queue<GameObject>();

        GameObject ParentObject = new GameObject(_strName + "_Pool");

        for (int i = 0; i < _isize; ++i)
        {
            GameObject InputObject = CreateObject(_Prefab);

            Util.ObjectCloneNameRemove(InputObject);

            _Funiton?.Invoke(InputObject);

            InputObject.SetActive(false);
            InputObject.transform.SetParent(ParentObject.transform);

            TempPool.Enqueue(InputObject);
        }

        ParentObject.transform.SetParent(m_Root);

        m_PoolParent.Add(_strName, ParentObject);

        m_PoolObject.Add(_strName, _Prefab);

        m_PoolDictionary.Add(_strName, TempPool);

    }

    public int FindObjectCountInQueue(string _strName)
    {
        Queue<GameObject> tempQueue;

        if (false == FindPoolQueue(_strName, out tempQueue))
            return -1;

        return tempQueue.Count;
    }

    public GameObject Get_Object(string _strName)
    {
        Queue<GameObject> TempQueue;

        if (false == FindPoolQueue(_strName, out TempQueue))
            return null;

        if (0 < TempQueue.Count)
        {
            GameObject PoolObject = TempQueue.Dequeue();
            PoolObject.SetActive(true);

            if (PoolObject.CompareTag("Monster"))
                PoolObject.transform.SetParent(null);
            else
                PoolObject.transform.SetParent(m_Root);

            return PoolObject;
        }
        else
        {
            GameObject NewObject = null;

            if (!m_PoolObject.TryGetValue(_strName, out NewObject))
            {
                return null;
            }

            GameObject TempObject = CreateObject(NewObject);

            TempObject.SetActive(true);
            TempObject.transform.SetParent(m_Root);

            return TempObject;
        }
    }


    public void RelaseObject(string _strName, GameObject _RetrunObject)
    {
        Queue<GameObject> TempQueue;
        GameObject ParentObject;

        if (false == FindPoolQueue(_strName, out TempQueue))
        {
            if (null != _RetrunObject)
                UnityEngine.Object.Destroy(_RetrunObject);

            return;
        }

        m_PoolParent.TryGetValue(_strName, out ParentObject);

        _RetrunObject.SetActive(false);

        _RetrunObject.transform.SetParent(ParentObject.transform);

        TempQueue.Enqueue(_RetrunObject);

    }

    public void DestroyParent(string _strName)
    {
        GameObject TempParent = null;
        Queue<GameObject> Pool = null;

        if (!m_PoolParent.TryGetValue(_strName, out TempParent))
            return;

        m_PoolObject.Remove(_strName);

        m_PoolDictionary.TryGetValue(_strName, out Pool);

        foreach (GameObject iter in Pool)
        {
            iter.transform.SetParent(null);

            UnityEngine.Object.Destroy(iter);
        }

        m_PoolParent.Clear();

        UnityEngine.Object.Destroy(TempParent);
    }

    public void AllClear()
    {
    }
}
