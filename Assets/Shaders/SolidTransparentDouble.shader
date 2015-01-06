Shader "Custom/TranspUnlit_2Faced" {
     Properties {
         _Color("Color & Transparency", Color) = (0, 0, 0, 0.5)
     }
     SubShader {
         Lighting Off
         ZWrite On
         Cull Off
         Blend SrcAlpha OneMinusSrcAlpha
         Tags {"Queue" = "Transparent"}
         Color[_Color]
         Pass {
         }
     } 
     FallBack "Unlit/Transparent"
 }