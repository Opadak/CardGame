using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PRS
{
    //카드 위치로 다시 오게끔
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Utils 
{
    public static Quaternion QI => Quaternion.identity;
    //람다 함수 형식
    // 즉석에서 구현했다.
    //QI는 매개변수가 되고 입력이 한개면 타입을 안 붙여줘도 알아서 컴파일러가 입력해준다.
    //[] => {}; 가 정 표현인데 타입과 소괄호 대괄호는 생략가능하다.

    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
            return result;
        }
    }

}
