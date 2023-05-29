using UnityEngine;

public class FingerBone_L : MonoBehaviour
{

	[SerializeField] private Transform[] bones;
	private Quaternion[] originRots;

	[Header("Thumb")]

	public float conthumb1;
	public float conthumb11;
	public float conthumb2;
	public float conthumb22;
	public float conthumb3;
	public float conthumb4;

	[Header("Index")]

	public float conindex1;
	public float conindex11;
	public float conindex2;
	public float conindex3;

	[Header("Middle")]

	public float conmiddle1;
	public float conmiddle11;
	public float conmiddle2;
	public float conmiddle3;

	[Header("Ring")]

	public float conring1;
	public float conring11;
	public float conring2;
	public float conring3;


	[Header("Little")]

	public float conlittle1;
	public float conlittle11;
	public float conlittle2;
	public float conlittle3;

	[Header("Wrist")]
	public float Wrist1;
	public float Wrist2;
	public float Wrist3;
	public float Wrist4;
	public float Wrist5;
	public float Wrist6;
	/*
	[Header("Elbow")]
	public float Elbow;
	public float Elbowx;

	[Header("Shoulder")]
	public float Shoulder1;
	public float Shoulder2;
	*/
	private void Awake()
	{

		originRots = new Quaternion[bones.Length];
		for (var i = 0; i < bones.Length; i++)
		{
			originRots[i] = bones[i].localRotation;

		}
	}

	private void ResetRot(int index)
	{
		bones[index].localRotation = originRots[index];
	}

	#region Thumb
	public void ConThumb1(float value)
	{
		conthumb1 = value;
		UpdateConThumb();
	}
	public void ConThumb11(float value)
	{
		conthumb11 = value;
		UpdateConThumb();
	}
	public void ConThumb2(float value)
	{
		conthumb2 = value;
		UpdateConThumb();
	}
	public void ConThumb22(float value)
	{
		conthumb22 = value;
		UpdateConThumb();
	}
	public void ConThumb3(float value)
	{
		conthumb3 = value;
		UpdateConThumb();
	}
	public void ConThumb4(float value)
	{
		conthumb4 = value;
		UpdateConThumb();
	}
	public void UpdateConThumb()
	{
		ResetRot(0);
		ResetRot(1);
		ResetRot(2);

		bones[0].Rotate(new Vector3(conthumb1, 0, conthumb11), Space.Self);
		bones[1].Rotate(new Vector3(conthumb2 , -conthumb22, conthumb11), Space.Self);
		bones[2].Rotate(new Vector3(conthumb3, 0, 0), Space.Self);
	}

	#endregion

	#region Index
	public void ConIndex1(float value)
	{
		conindex1 = value;
		UpdateConIndex();
	}
	public void ConIndex11(float value)
	{
		conindex11 = value;
		UpdateConIndex();
	}
	public void ConIndex2(float value)
	{
		conindex2 = value;
		UpdateConIndex();
	}
	public void ConIndex3(float value)
	{
		conindex3 = value;
		UpdateConIndex();
	}
	public void UpdateConIndex()
	{
		ResetRot(3);
		ResetRot(4);
		ResetRot(5);

		bones[3].Rotate(new Vector3(conindex1, 0, -conindex11), Space.Self);
		bones[4].Rotate(new Vector3(conindex2, 0, 0), Space.Self);
		bones[5].Rotate(new Vector3(conindex3, 0, 0), Space.Self);
	}

	#endregion

	#region Middle
	public void ConMiddle1(float value)
	{
		conmiddle1 = value;
		UpdateConMiddle();
	}
	public void ConMiddle11(float value)
	{
		conmiddle11 = value;
		UpdateConMiddle();
	}
	public void ConMiddle2(float value)
	{
		conmiddle2 = value;
		UpdateConMiddle();
	}
	public void ConMiddle3(float value)
	{
		conmiddle3 = value;
		UpdateConMiddle();
	}
	public void UpdateConMiddle()
	{
		ResetRot(6);
		ResetRot(7);
		ResetRot(8);
		bones[6].Rotate(new Vector3(conmiddle1, 0, conmiddle11), Space.Self);
		bones[7].Rotate(new Vector3(conmiddle2, 0, 0), Space.Self);
		bones[8].Rotate(new Vector3(conmiddle3, 0, 0), Space.Self);
	}
	#endregion

	#region Ring
	public void ConRing1(float value)
	{
		conring1 = value;
		UpdateConRing();
	}
	public void ConRing11(float value)
	{
		conring11 = value;
		UpdateConRing();
	}
	public void ConRing2(float value)
	{
		conring2 = value;
		UpdateConRing();
	}
	public void ConRing3(float value)
	{
		conring3 = value;
		UpdateConRing();
	}
	public void UpdateConRing()
	{
		ResetRot(9);
		ResetRot(10);
		ResetRot(11);

		bones[9].Rotate(new Vector3(conring1, 0, conring11), Space.Self);
		bones[10].Rotate(new Vector3(conring2, 0, 0), Space.Self);
		bones[11].Rotate(new Vector3(conring3, 0, 0), Space.Self);
	}

	#endregion

	#region Little
	public void ConLittle1(float value)
	{
		conlittle1 = value;
		UpdateConLittle();
	}
	public void ConLittle11(float value)
	{
		conlittle11 = value;
		UpdateConLittle();
	}
	public void ConLittle2(float value)
	{
		conlittle2 = value;
		UpdateConLittle();
	}
	public void ConLittle3(float value)
	{
		conlittle3 = value;
		UpdateConLittle();
	}
	public void UpdateConLittle()
	{
		ResetRot(12);
		ResetRot(13);
		ResetRot(14);

		bones[12].Rotate(new Vector3(conlittle1, 0, conlittle11), Space.Self);
		bones[13].Rotate(new Vector3(conlittle2, 0, 0), Space.Self);
		bones[14].Rotate(new Vector3(conlittle3, 0, 0), Space.Self);
	}

	#endregion
	#region Wrist
	public void Wristx(float value)
	{
		Wrist1 = value;
		UpdateWrist();
	}

	public void Wristy(float value)
	{
		Wrist2 = value;
		UpdateWrist();
	}

	public void Wristz(float value)
	{
		Wrist3 = value;
		UpdateWrist();
	}

	public void WristAccelerationx(float value)
	{
		Wrist4 = value;
		UpdateWrist();
	}

	public void WristAccelerationy(float value)
	{
		Wrist5 = value;
		UpdateWrist();
	}

	public void WristAccelerationz(float value)
	{
		Wrist6 = value;
		UpdateWrist();
	}

	public void UpdateWrist()
	{
		ResetRot(15);
		ResetRot(16);
		bones[15].Rotate(new Vector3(-Wrist1, 0, 0), Space.Self);
		bones[16].Rotate(new Vector3(0, -Wrist2, 0), Space.Self);
		bones[15].Rotate(new Vector3(0, 0, -Wrist3), Space.Self);
		//bones[16].Rotate(new Vector3(0, -Wrist3, 0), Space.Self);
		//bones[15].Translate(new Vector3(0, Wrist4), Space.Self);
		bones[15].Translate(new Vector3(Wrist5, 0), Space.Self);
		//bones[15].Translate(new Vector3(Wrist6,0), Space.Self);

	}
	#endregion
	/*
	#region Elbow
	public void ElbowR(float value)
	{
		Elbow = value;
		UpdateElbow();
	}

	public void ElbowRx(float value)
	{
		Elbowx = value;
		UpdateElbow();
	}
	public void UpdateElbow()
	{
		ResetRot(16);
		bones[16].Rotate(new Vector3(Elbowx, 0, 0), Space.Self);
		bones[16].Rotate(new Vector3(0, 0, Elbow), Space.Self);

	}
	#endregion

	#region Shoulder1

	public void SetShoulder1(float value)
	{
		Shoulder1 = value ;
		UpdateShoulder();


	}

	#endregion

	#region Shoulder2
	
	public void SetShoulder2(float value)
	{
		Shoulder2 = value ;
		UpdateShoulder();
	}
	public void UpdateShoulder()
	{
		ResetRot(17);
	

		bones[17].Rotate(new Vector3(0, 0, -Shoulder1), Space.Self);
		bones[17].Rotate(new Vector3(0, Shoulder2, 0), Space.Self);

	}
	#endregion
	*/

}