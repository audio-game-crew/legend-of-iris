Shader "Depth/Fog" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:mycolor vertex:myvert

		sampler2D _MainTex;
		uniform half4 unity_FogColor;
		uniform half4 unity_FogStart;
		uniform half4 unity_FogEnd;

		struct Input {
			half fog;
		};

		void myvert (inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input,data);
			float pos = length(mul (UNITY_MATRIX_MV, v.vertex).xyz);

			float diff = unity_FogEnd.x - unity_FogStart.x;
			float invDiff = 1.0f / diff;
			data.fog = clamp ((unity_FogEnd.x - pos) * invDiff, 0.0, 1.0);
		}
		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			fixed3 fogColor = unity_FogColor.rgb;
			#ifdef UNITY_PASS_FORWARDADD
			fogColor = 0;
			#endif
			color.rgb = lerp (fogColor, color.rgb * 0, IN.fog);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = 0;
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}