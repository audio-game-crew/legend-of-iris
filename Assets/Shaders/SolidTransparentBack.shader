Shader "Custom/TranspUnlit_BackFaced" {
     Properties {
         _Color("Color & Transparency", Color) = (0, 0, 0, 0.5)
     }
     SubShader {
         Lighting Off
         ZWrite On
         Cull Front
		 ZTest LEqual
         Blend SrcAlpha OneMinusSrcAlpha
         Tags {"Queue" = "Transparent"}
         Color[_Color]
         Pass {
         }
     } 
     FallBack "Unlit/Transparent"
 }