#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern float time;
Texture2D SpriteTexture;
sampler s0;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(s0, input.TextureCoordinates);
	// color = 1-color;
	float2 uv = input.TextureCoordinates ;
	// float2(640, 360); 
    float3 col = 0.5 + 0.5*cos(time+uv.xyx+float3(0,2,4));
	color.rgb = col.rgb;
	// color.a = 1 - col.a;
	return color;

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
