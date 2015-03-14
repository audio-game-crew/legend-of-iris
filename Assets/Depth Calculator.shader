Shader "Depth/Depth Renderer" {
Properties {
		_DepthStart ("Start", float) = 0
		_DepthEnd ("End", float) = 100
	}
	SubShader {
		Cull Back
		Lighting Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
 
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert finalcolor:mycolor
 
		float _DepthStart;
		float _DepthEnd;
 
		struct Input {
			float3 depth;
		};
 
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			float3 foo = mul(UNITY_MATRIX_MVP, v.vertex);
			o.depth = clamp((foo.z - _DepthStart) / (_DepthEnd - _DepthStart), 0, 1);
		}

		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			color.rgb = float3(1,1,1) * IN.depth;
		}
 
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = 1;
			o.Alpha = 1;
		}
		ENDCG
	}
 
	Fallback "VertexLit"
}