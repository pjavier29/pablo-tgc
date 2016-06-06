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
float lightIntensityRelitive; //Intensidad de la luz, recibimos un numero entre 0 y 1
float colorCieloRojo; //Enviamos un flotante para calcular el color del cielo al amanecer o anochecer
float colorCieloVerde; //Enviamos un flotante para calcular el color del cielo al amanecer o anochecer


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
   
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;
   
   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}



//Pixel Shader
float4 ps_main( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{      
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D( diffuseMap, Texcoord );
	float4 colorCielo = float4(colorCieloRojo, colorCieloVerde, 0, 0);
	// Lo multiplico por la intensidad y le sumo el color
	return (fvBaseColor + colorCielo) * lightIntensityRelitive;
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

