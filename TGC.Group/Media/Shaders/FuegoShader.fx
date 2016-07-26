// ---------------------------------------------------------
// Ejemplo shader Minimo:
// ---------------------------------------------------------

/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

float time = 0;
float altura = 0;


/**************************************************************************************/
/* RenderScene */
/**************************************************************************************/

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float4 Color : COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position :        POSITION0;
   float2 Texcoord :        TEXCOORD0;
   float4 Color :			COLOR0;
};


// Ejemplo de un vertex shader que anima la posicion de los vertices 
// ------------------------------------------------------------------
VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;
   float y = Input.Position.y;
   float valor = y > altura
	       ? ((y + 200) / 142)
	       : 0;

   Input.Position.x += sin((time*valor*valor)*5)*valor*sign(Input.Position.x);
   Input.Position.z += cos((time*valor*valor)*5)*valor*sign(Input.Position.z);
   Input.Position.y += (sin(time * 20) + 5) * valor;
   
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;
   
   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}

VS_OUTPUT vs_main2( VS_INPUT Input )
{
   VS_OUTPUT Output;

   float y = Input.Position.y;
   float valor = y > altura
	       ? ((y + 200) / 142)
	       : 0;

   float4 colorAumentado = y > altura
	                 ? float4(1, 1, 0, 0)
	                 : float4(0, 0, 0, 0);

   Input.Position.x += sin((time*valor*valor)*5)*valor*sign(Input.Position.x);
   Input.Position.z += cos((time*valor*valor)*5)*valor*sign(Input.Position.z);
   Input.Position.y += (sin(time * 20) + 10) * valor;
   
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;
   
   //Propago el color x vertice
   Output.Color = Input.Color + colorAumentado;

   return( Output );
   
}


//Pixel Shader
float4 ps_main( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{      
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D( diffuseMap, Texcoord );
	
	// combino color y textura
	// en este ejemplo combino un 80% el color de la textura y un 20%el del vertice
	return 0.8*fvBaseColor + 0.2*Color;
}


// ------------------------------------------------------------------
technique RenderScene
{
   pass Pass_0
   {
	  VertexShader = compile vs_3_0 vs_main();
	  PixelShader = compile ps_3_0 ps_main();
   }

}

technique RenderScene2
{
   pass Pass_0
   {
	  VertexShader = compile vs_3_0 vs_main2();
	  PixelShader = compile ps_3_0 ps_main();
   }

}
