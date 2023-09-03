#ifndef MYHLSLINCLUDE_EXCLUDED
#define MYHLSLINCLUDE_EXCLUDED

#define H 255

void ColorRemap_float(float3 Col, float3 Col1, float3 Col2, float3 Col3,
float3 Col4, float3 Col5, float3 Col6, float3 Col7, float3 Col8, float3 Col9,
float3 Col10, out float3 Out)
{
    if (Col.r > 0.90)
    {
        //Col = half3(211. / H, 216. / H, 217. / H);
        Col = Col1;
    }

    else if (Col.r > 0.90)
    {
        //Col = half3(211. / H, 216. / H, 217. / H);
        Col = Col1;
    }
    else if (Col.r <= 0.90 && Col.r > 0.80)
    {
        //Col = half3(197. / H, 210. / H, 206. / H);
        Col = Col2;
    }
    else if (Col.r <= 0.80 && Col.r > 0.70)
    {
        //Col = half3(179. / H, 198. / H, 180. / H);
        Col = Col3;
    }
    else if (Col.r <= 0.70 && Col.r > 0.60)
    {
        //Col = half3(142. / H, 178. / H, 154. / H);
        Col = Col4;
    }
    else if (Col.r <= 0.60 && Col.r > 0.50)
    {
        //Col = half3(123. / H, 173. / H, 159. / H);
        Col = Col5;
    }
    else if (Col.r <= 0.50 && Col.r > 0.40)
    {
        //Col = half3(106. / H, 147. / H, 149. / H);
        Col = Col6;
    }
    else if (Col.r <= 0.40 && Col.r > 0.30)
    {
        //Col = half3(93. / H, 118. / H, 128. / H);
        Col = Col7;
    }
    else if (Col.r <= 0.30 && Col.r > 0.20)
    {
        //Col = half3(84. / H, 94. / H, 114. / H);
        Col = Col8;
    }
    else if (Col.r <= 0.20 && Col.r > 0.10)
    {
        //Col = half3(70. / H, 68. / H, 89. / H);
        Col = Col9;
    }
    else
    {
        //Col = half3(55. / H, 47. / H, 58. / H);
        Col = Col10;
    }
    
    Out = Col;
}
#endif 