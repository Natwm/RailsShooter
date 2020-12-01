// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/Outline_Normal"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0.2551723,1,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.03
		_Albedo("Albedo", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_NormalStrenght("NormalStrenght", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input {
			half filler;
		};
		half4 _ASEOutlineColor;
		half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform half _NormalStrenght;
		uniform sampler2D _Albedo;
		uniform half4 _Albedo_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			half2 temp_output_2_0_g1 = uv_Texture0;
			half2 break6_g1 = temp_output_2_0_g1;
			half temp_output_25_0_g1 = ( pow( 0.5 , 3.0 ) * 0.1 );
			half2 appendResult8_g1 = (half2(( break6_g1.x + temp_output_25_0_g1 ) , break6_g1.y));
			half4 tex2DNode14_g1 = tex2D( _Texture0, temp_output_2_0_g1 );
			half temp_output_4_0_g1 = _NormalStrenght;
			half3 appendResult13_g1 = (half3(1.0 , 0.0 , ( ( tex2D( _Texture0, appendResult8_g1 ).g - tex2DNode14_g1.g ) * temp_output_4_0_g1 )));
			half2 appendResult9_g1 = (half2(break6_g1.x , ( break6_g1.y + temp_output_25_0_g1 )));
			half3 appendResult16_g1 = (half3(0.0 , 1.0 , ( ( tex2D( _Texture0, appendResult9_g1 ).g - tex2DNode14_g1.g ) * temp_output_4_0_g1 )));
			half3 normalizeResult22_g1 = normalize( cross( appendResult13_g1 , appendResult16_g1 ) );
			o.Normal = normalizeResult22_g1;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18500
0;0;1536;803;848.3586;-185.0935;1;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;3;-585.7155,472.4344;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;5;-542.3586,697.0935;Inherit;False;Property;_NormalStrenght;NormalStrenght;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-241.5004,282.2002;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;4;-206.3586,511.5935;Inherit;False;NormalCreate;1;;1;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;147.2,339.6;Half;False;True;-1;2;;0;0;Standard;ASESampleShaders/Outline_Normal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;True;0.03;0,0.2551723,1,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;1;3;0
WireConnection;4;4;5;0
WireConnection;0;0;1;0
WireConnection;0;1;4;0
ASEEND*/
//CHKSM=1F416823062519D6ED5B9D3857C963DA9B1FABE4