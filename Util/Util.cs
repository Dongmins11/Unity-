using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    // �̸� ���ڿ��� �ڽ� ��ü ã��
    // ���� 
    // 1. �̸��� ���� ��ü�� ��������� ������ ���� ���� ��ü�� ��ȯ�� (�ǵ��� �κ�)
    // 2. Contains�� �ƴ϶� Equal�� ��� ������ �̸� ��Ȯ�� Ǯ�������� �Է������ ��
    // 3. null �˻� �� ���ּ���

    public static Transform FindChildWithName(Transform _Parent, String _strName)
    {
        Transform[] Children;
        Children = _Parent.GetComponentsInChildren<Transform>();

        foreach (Transform Child in Children)
        {
            if (Child.name.Equals(_strName))
                return Child;
        }

        return null;
    }

    public static void ObjectCloneNameRemove(GameObject go)
    {
        int index = go.name.IndexOf("(Clone)");

        if (0 < index)
            go.name = go.name.Substring(0, index);
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T Component = go.GetComponent<T>();

        if (null == Component)
            Component = go.AddComponent<T>();

        return Component;
    }

    public static GameObject FindChild(GameObject _GameObject, string _name = null, bool Recusive = false)
    {
        Transform transform = FindChild<Transform>(_GameObject, _name, Recusive);

        if (null == transform)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject _GameObject, string _name = null, bool Recusive = false) where T : UnityEngine.Object
    {
        if (null == _GameObject)
            return null;

        if (false == Recusive)
        {
            for (int i = 0; i < _GameObject.transform.childCount; ++i)
            {
                Transform transform = _GameObject.transform.GetChild(i);

                if (string.IsNullOrEmpty(_name) || transform.name == _name)
                {
                    T component = transform.GetComponent<T>();

                    if (null != component)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in _GameObject.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                    return component;
            }
        }

        return null;
    }

    public static int Get_MiddleIndex(int _iStartIndex, int _iEndIndex)
    {
        int iResult = _iStartIndex + _iEndIndex;

        if (0 == (iResult & 1))
            iResult /= 2;
        else
            iResult = (iResult / 2) + 1;

        return iResult;
    }

    public static bool FindKeyAction(Action _MyAction, Action _FindFunction)
    {
        if (null == _MyAction)
            return false;

        Delegate[] TempDelegates = _MyAction.GetInvocationList();

        foreach (var iter in TempDelegates)
        {
            if (_MyAction.Method.Name == iter.Method.Name)
                return true;
        }

        return false;
    }

    public static void MoveToMaterial(Material _Material, float _fOffset)
    {
        _Material.SetTextureOffset("_BaseMap", new Vector2(_fOffset, 0.0f));
    }

    public static void Billbord_UI(Transform _MyTrasnform, Transform _CamTrans, bool _bIsYActive = false)
    {
        if (null == _CamTrans)
            return;

        Vector3 Billboard_Direction = _MyTrasnform.position - _CamTrans.transform.position;

        if (false == _bIsYActive)
            Billboard_Direction.y = 0.0f;
            
        Billboard_Direction.Normalize();

        Quaternion LookRot = Quaternion.LookRotation(Billboard_Direction);

        _MyTrasnform.rotation = LookRot;
    }

}
