matrix World;
matrix View;
matrix Projetion;

float3 AmbientColor = float3(0.15f, 0.15f, 0.15f);
float3 DiffuseColor = float3(1,1,1);
float3 LightColor = float3(1,1,1);

float3 Direction = float3(0, 1, 0);

Texture2D ModelTexture;
SamplerState TextureSample
{

};

//vertex function input
struct VertexShaderInput
{
	float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 Normal : NORMAL0;
};

//pixel function input
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 UV : TEXCOORDD0;
    float3 Normal : TEXCOORD1;
};

//vertex shader function
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 world = mul(input.Position, World);
    float4 view = mul(world, View);
    float4 proj = mul(view, Projetion);

    output.Position = proj;
    output.UV = input.UV;

    float3 normalWorld = mul(input.Normal, World);
    //normal / length(normal)
    output.Normal = normalize(normalWorld);

    return output;

}

//pixel sahder function
float4 MainPS(VertexShaderOutput input) : COLOR
{
    //starting color 
    float3 color = DiffuseColor;
    float3 lightDirection = normalize(Direction);
    //color if no ligjt recieved
    float3 lightColor = AmbientColor;
    float3 textureColor = ModelTexture.Sample(TextureSample, input.UV);
    //reflectance angle
    float3 angle = saturate(dot(input.Normal, lightDirection));
    lightColor += saturate(angle * color * textureColor * LightColor);

    return float4(lightColor, 1);

}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};