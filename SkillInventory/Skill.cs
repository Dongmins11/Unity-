using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public enum Skill_Type
    {
        MagneCatch,
        RemoteBomb,
        IceMaker,
        TimeLock,
        End_Type
    }
    

    public Skill(Skill_Type _eType, string _strSpriteName, string _strRenderPath)
    {
        My_Type = _eType;
        My_Sprite = GameManager.Resources.Load<Sprite>(StrDefalutPath + _strSpriteName);
        My_QuickRenderSpiret = GameManager.Resources.Load<Sprite>(StrSkillPath + _strRenderPath);

        //if (null == My_Sprite)
            //Debug.Log("Failed to Skill Class Find Load");

        switch (My_Type)
        {
            case Skill_Type.MagneCatch:
                My_Name = "���׳� ĳġ";
                break;
            case Skill_Type.RemoteBomb:
                My_Name = "������ ��ź";
                break;
            case Skill_Type.IceMaker:
                My_Name = "���̽� ����Ŀ";
                break;
            case Skill_Type.TimeLock:
                My_Name = "Ÿ�� ��";
                break;
        }

    }

    private string StrDefalutPath = "Textures/UI/InGameUI_Texture/SkillInven/";
    private string StrSkillPath = "Textures/UI/Skill_UI/";

    public Skill_Type My_Type { get; set; } = Skill_Type.End_Type;

    public Sprite My_Sprite { get; private set; }

    public Sprite My_QuickRenderSpiret { get; private set; }

    public string My_Name;
}
