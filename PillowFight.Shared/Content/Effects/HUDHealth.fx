#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern float Height;
extern float Position;

extern float Health[4];
Texture2D SpriteTexture;
Texture2D SpriteTexture2;
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
    float2 uv = input.TextureCoordinates.xy;
    //if ((Height * 32 + Position)/360 < uv.y)
    if (Height < uv.y)
    {
        color.g = (color.r + color.g + color.b) / 3;
        color.rb = 0;
    }
    else
    {
        color.r = (color.r + color.g + color.b) / 3;
        color.gb = 0;
    }
	
    return color;
//	return tex2D(S2priteTextureSampler,input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};