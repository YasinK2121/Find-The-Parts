// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GhostShader"
{
	Properties
	{
		_GhostColor("GhostColor", Color) = (1,0,0.01176471,1)
		_OutlineScale("OutlineScale", Float) = 0.01
		_AlphaPower("AlphaPower", Float) = 3
		_Float3("Float 3", Float) = -0.2
		_Float4("Float 4", Float) = 1.25
		_Float5("Float 5", Float) = 0.5
		_Float6("Float 6", Float) = 0.5
		_Float7("Float 7", Float) = 1
		_Float8("Float 8", Float) = -0.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
			ZWrite On
		}

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0"}
		ZWrite On
		ZTest Always
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog alpha:fade  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input
		{
			float3 worldPos;
			half3 worldNormal;
			INTERNAL_DATA
		};
		uniform half _OutlineScale;
		uniform half4 _GhostColor;
		uniform half _AlphaPower;
		uniform half _Float8;
		uniform half _Float7;
		uniform half _Float6;
		uniform half _Float3;
		uniform half _Float4;
		uniform half _Float5;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = _OutlineScale;
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			half3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			half3 ase_normWorldNormal = normalize( ase_worldNormal );
			half fresnelNdotV134 = dot( ase_normWorldNormal, ase_worldViewDir );
			half fresnelNode134 = ( _Float8 + _Float7 * pow( 1.0 - fresnelNdotV134, _Float6 ) );
			half fresnelNdotV124 = dot( ase_normWorldNormal, ase_worldViewDir );
			half fresnelNode124 = ( _Float3 + _Float4 * pow( 1.0 - fresnelNdotV124, _Float5 ) );
			half clampResult130 = clamp( min( fresnelNode134 , ( 1.0 - fresnelNode124 ) ) , 0.0 , 1.0 );
			o.Emission = _GhostColor.rgb;
			o.Alpha = ( _AlphaPower * clampResult130 );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+1" }
		Cull Back
		ZWrite On
		ZTest LEqual
		Blend Zero One
		
		CGPROGRAM
		#pragma target 2.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17802
1960;64;1657;894;696.5759;1265.83;1.583648;True;False
Node;AmplifyShaderEditor.RangedFloatNode;128;-32.79514,-206.2495;Half;False;Property;_Float4;Float 4;4;0;Create;True;0;0;False;0;1.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-32.79514,-108.7494;Half;False;Property;_Float5;Float 5;5;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-43.19503,-302.4494;Half;False;Property;_Float3;Float 3;3;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;53.84647,75.62952;Half;False;Property;_Float6;Float 6;6;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;124;211.6049,-243.9494;Inherit;True;Standard;TangentNormal;ViewDir;True;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;59.25358,161.9666;Half;False;Property;_Float7;Float 7;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;137;57.5048,251.7665;Half;False;Property;_Float8;Float 8;8;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;134;359.3291,97.97546;Inherit;True;Standard;TangentNormal;ViewDir;True;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;125;522.3052,-245.2495;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;139;716.3467,-303.0724;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;130;921.8007,-314.3524;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;382.5524,-524.8203;Half;False;Property;_AlphaPower;AlphaPower;2;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;95;-124.2811,-630.6235;Half;False;Property;_GhostColor;GhostColor;0;0;Create;True;0;0;False;0;1,0,0.01176471,1;0,0.297,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;104;360.1668,-401.1414;Half;False;Property;_OutlineScale;OutlineScale;1;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;576.9046,-515.6503;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;103;805.4467,-662.6535;Inherit;False;0;True;Transparent;1;7;Back;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1162.545,-944.3604;Half;False;True;-1;0;ASEMaterialInspector;0;0;Standard;GhostShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;1;False;-1;3;False;-1;False;0;False;-1;0;False;-1;True;0;Opaque;0.5;True;True;1;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;1;0;False;-1;1;False;-1;0;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;124;1;127;0
WireConnection;124;2;128;0
WireConnection;124;3;129;0
WireConnection;134;1;137;0
WireConnection;134;2;133;0
WireConnection;134;3;132;0
WireConnection;125;0;124;0
WireConnection;139;0;134;0
WireConnection;139;1;125;0
WireConnection;130;0;139;0
WireConnection;126;0;105;0
WireConnection;126;1;130;0
WireConnection;103;0;95;0
WireConnection;103;2;126;0
WireConnection;103;1;104;0
WireConnection;0;11;103;0
ASEEND*/
//CHKSM=43E11D6E7F35DFA794522CAECF727578DDB18125