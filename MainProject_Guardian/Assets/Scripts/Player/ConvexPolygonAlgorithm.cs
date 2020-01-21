using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ConvexPolygonAlgorithm : MonoBehaviour
{
    private List<float[]> pointList;
    private int maxArrayLength = 0;
    private const float twoPI = 2 * 3.14159265f;


    /// <summary>
    /// 미리 지정한 좌표를 통해 다각형의 꼭지점을 구성합니다.
    /// </summary>
    /// <param name="shapePt">다각형의 꼭지점이 될 point배열입니다.</param>


    //public ConvexPolygonAlgorithm(Point[] shapePt)
    //{
    //    ar = new ArrayList(shapePt.Length);

    //    foreach (Point pt in shapePt)
    //    {
    //        ar.Add(pt);
    //    }
    //}

    //입력 파라메터는, centerofEclipse : 타원의 중점, eclipseSize는 타원의 크기,

    //                        howmany : 타원상의 점을 몇개나 추출해 낼것이냐

    //                                        만약 이 좌표들로 그림을 그린다면 howmany가 높을수록

    //                                         매끄러운 타원에 근접하게 될것이다.



    public ConvexPolygonAlgorithm(float centerofEllipseX, float centerofEllipseY, float ellipseSizeX, float ellipseSizeY, int howmany /* ,int slope */)
    {
        pointList = new List<float[]>();

        float W = ellipseSizeX / 2;
        float H = ellipseSizeY / 2;

        for (float radian = 0; radian <= twoPI; radian += twoPI / howmany)
        {

            //float X = Convert.ToInt32(centerofEllipseX + W * Math.Cos(radian) * Math.Cos(30) - H * Math.Sin(radian) * Math.Sin(30));
            //float Y = Convert.ToInt32(centerofEllipseY + W * Math.Cos(radian) * Math.Sin(30) + H * Math.Sin(radian) * Math.Cos(30));
            float X = centerofEllipseX + W * Mathf.Cos(radian);
            float Y = centerofEllipseY + H * Mathf.Sin(radian);

            float[] point = {X, Y};
            pointList.Add(point);
        }

        //매개변수화(타원의 기울기 지정 가능 A와B는 각 좌표의 중점X,Y, slope는 타원의 기울기)
        //temp.X = Convert.ToInt32(centerofEclipse.X + A * Math.Cos(radian) * Math.Cos(slope) - B * Math.Sin(radian) * Math.Sin(slope));
        //temp.Y = Convert.ToInt32(centerofEclipse.Y + A * Math.Cos(radian) * Math.Sin(slope) + B * Math.Sin(radian) * Math.Cos(slope));
    }
    public List<float[]> returnPts()
    {
        return pointList;
    }
}