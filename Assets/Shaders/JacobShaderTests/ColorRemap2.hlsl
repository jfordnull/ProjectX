#ifndef MYHLSLINCLUDE_EXCLUDED
#define MYHLSLINCLUDE_EXCLUDED

#define H 255

void ColorRemap_float(float3 Col, out float3 Out)
{
    if (Col.r > .99)
    {
        //Col = half3(228. / H, 50. / H, 84. / H);
    }
    else if (Col.r > 0.80)
    {
        //Col = half3(212. / H, 226. / H, 204. / H);
        Col = float3(.83, .87, .8);
    }
    else if (Col.r <= 0.80 && Col.r > 0.60)
    {
        Col = half3(132. / H, 174. / H, 164. / H);
    }
    else if (Col.r <= 0.60 && Col.r > 0.40)
    {
        Col = half3(60. / H, 86. / H, 108. / H);
    }
    //else if (Col.r <= 0.40 && Col.r > 0.20)
    //{
    //    Col = half3(48. / H, 98. / H, 48. / H);
    //}
    else
    {
        Col = half3(23. / H, 21. / H, 46. / H);
    }
    
    Out = Col;
}
#endif 