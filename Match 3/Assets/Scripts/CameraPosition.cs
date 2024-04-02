using UnityEngine;

public class CameraPosition : MonoBehaviour
{
	[SerializeField]private Board _board;
	
	//UI'a yer kalması için bu otomatik kodu iptal etmek durumunda kaldım, bu gem gridlerine göre kamerayı posizyonlamamıza yarıyordu,10,10 geme sahip bir board oluşturduğumuzda eksra
	//ayara gerek yoktu, ancak el ile yapacaksak CameraX fonksiyon içinde doğru pozisyonlandırma formülü mevcut
	private  void Start()
	{
		transform.position=new Vector3(_board.CameraX(),_board.CameraY(),transform.position.z);
	}
}
