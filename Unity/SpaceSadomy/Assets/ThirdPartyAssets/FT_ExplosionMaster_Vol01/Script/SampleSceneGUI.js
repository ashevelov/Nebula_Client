var mySkin : GUISkin;
var effect01 : GameObject;
var effect02 : GameObject;
var effect03 : GameObject;
var effect04 : GameObject;
var effect05 : GameObject;
var effect06 : GameObject;
var effect07 : GameObject;
var effect08 : GameObject;
var effect09 : GameObject;
var effect10 : GameObject;
var effect11 : GameObject;
var effect12 : GameObject;
var effect13 : GameObject;
var effect14 : GameObject;
var effect15 : GameObject;
var effect16 : GameObject;
var effect17 : GameObject;
var effect18 : GameObject;
var effect19 : GameObject;
var effect20 : GameObject;
var effect21 : GameObject;
var effect22 : GameObject;
var effect23 : GameObject;
var effect24 : GameObject;
var effect25 : GameObject;
var effect26 : GameObject;
var effect27 : GameObject;
var effect28 : GameObject;
var effect29 : GameObject;
var effect30 : GameObject;
var effect31 : GameObject;
var effect32 : GameObject;
var effect33 : GameObject;
var effect34 : GameObject;
var effect35 : GameObject;
var effect36 : GameObject;
var effect37 : GameObject;
var effect38 : GameObject;
var effect39 : GameObject;
var effect40 : GameObject;
var effect41 : GameObject;
var effect42 : GameObject;

function OnGUI ()
{
	GUI.skin = mySkin;
	
	GUI.Label (Rect (70,10,300,20), "FT ExplosionMaster Volume01");

	if(GUI.Button (Rect (10,40,20,20), GUIContent ("", "AirExplosion01")))
	{	Instantiate(effect01, new Vector3(0, 3, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (40,40,20,20), GUIContent ("", "AirExplosion02")))
	{	Instantiate(effect02, new Vector3(0, 3, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (70,40,20,20), GUIContent ("", "AirExplosion_Smoke01")))
	{	Instantiate(effect03, new Vector3(0, 3, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (100,40,20,20), GUIContent ("", "Explosion01")))
	{	Instantiate(effect04, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (130,40,20,20), GUIContent ("", "Explosion02")))
	{	Instantiate(effect05, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (160,40,20,20), GUIContent ("", "Explosion03")))
	{	Instantiate(effect06, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (190,40,20,20), GUIContent ("", "Explosion04")))
	{	Instantiate(effect07, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	
	if(GUI.Button (Rect (10,70,20,20), GUIContent ("", "Explosion05")))
	{	Instantiate(effect08, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (40,70,20,20), GUIContent ("", "Explosion06")))
	{	Instantiate(effect09, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (70,70,20,20), GUIContent ("", "Explosion07")))
	{	Instantiate(effect10, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (100,70,20,20), GUIContent ("", "Explosion_Side01")))
	{	Instantiate(effect11, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (130,70,20,20), GUIContent ("", "Explosion_Side02")))
	{	Instantiate(effect12, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (160,70,20,20), GUIContent ("", "ExplosionSmoke_Side01")))
	{	Instantiate(effect13, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (190,70,20,20), GUIContent ("", "Explosion_Smoke01")))
	{	Instantiate(effect14, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	
	if(GUI.Button (Rect (10,100,20,20), GUIContent ("", "Explosion_Smoke02")))
	{	Instantiate(effect15, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (40,100,20,20), GUIContent ("", "Explosion_Smoke03")))
	{	Instantiate(effect16, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (70,100,20,20), GUIContent ("", "Explosion_Smoke04")))
	{	Instantiate(effect17, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (100,100,20,20), GUIContent ("", "Explosion_Smoke05")))
	{	Instantiate(effect18, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (130,100,20,20), GUIContent ("", "Explosion_Smoke06")))
	{	Instantiate(effect19, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
	if(GUI.Button (Rect (160,100,20,20), GUIContent ("", "Explosion_Smoke07")))
	{	Instantiate(effect20, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}
/**	if(GUI.Button (Rect (190,100,20,20), GUIContent ("", "EffectName")))
	{	Instantiate(effect21, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));	}**/





	
	GUI.Label (Rect (10,Screen.height-30,300,25), GUI.tooltip);
}