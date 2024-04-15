using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthentication : MonoBehaviour
{
	[SerializeField] UIAuth _uiAuth;
	string _userName;
	GoogleSignInConfiguration _configuration;
	private string _api= "291580415155-2knvgd714vviff2p92nero31393hqq6l.apps.googleusercontent.com"; //değiştir
	private void Awake()
	{
		_configuration = new GoogleSignInConfiguration
  		{
      		WebClientId = _api,
      		RequestIdToken = true
  		};
	}
	private void Start()
	{
		CheckUser();
	}
	private void CheckUser()
	{
		if (FirebaseManager.Instance.User != null)
		{
			FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(FirebaseManager.Instance.User.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
			{
				if (task.IsFaulted || task.IsCanceled)
				{
					Debug.Log("username Hatasi");
					return;
				}
				DataSnapshot snapshot = task.Result;
				if (snapshot.HasChildren)
				{
					_uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
					_uiAuth.OpenPanel(_uiAuth.GamePanel);
				}              
			});
		}
		else
		{
			_uiAuth.OpenPanel(_uiAuth.LoginPanel);
		}
	}
	public void SingInAnonymous()
	{
		FirebaseManager.Instance.Auth.SignInAnonymouslyAsync().ContinueWith(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("Anonymous Kayit Hatasi");
			}
			else
			{
				AuthResult result = task.Result;
				FirebaseManager.Instance.User = result.User;

				FirebaseDatabase.DefaultInstance.GetReference("Userss").GetValueAsync().ContinueWithOnMainThread(vTask =>
				{
					DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Userss");
					if (vTask.IsFaulted || vTask.IsCanceled)
					{
						Debug.Log("Anonymous Kayit Hatasi");
					}
					else
					{
						_uiAuth.NameInput.text = "";
						_uiAuth.OpenPanel(_uiAuth.NamePanel);
					}
				});
			}
		});
	}
	public void GoogleSingUp()
	{
		GoogleSignIn.Configuration = _configuration;
		GoogleSignIn.Configuration.UseGameSignIn = false;
		GoogleSignIn.Configuration.RequestIdToken = true;
		GoogleSignIn.Configuration.RequestEmail = true;
		GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(FinishSingIn);
	}
	private void FinishSingIn(Task<GoogleSignInUser> task)
	{
		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.Log("Kayit Hatasi");

			return;
		}

		Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

		FirebaseManager.Instance.Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("Credential Hatasi");
				return;
			}
			FirebaseManager.Instance.User = task.Result;
			FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(FirebaseManager.Instance.User.UserId).GetValueAsync().ContinueWithOnMainThread(ctask =>
			{
				if (ctask.IsFaulted || ctask.IsCanceled)
				{
					Debug.Log("username Hatasi");
					return;
				}
				DataSnapshot snapshot = ctask.Result;
				if (snapshot.HasChildren)
				{
					_uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
					_uiAuth.OpenPanel(_uiAuth.GamePanel);
				}
				else
				{
					_uiAuth.NameInput.text = "";
					_uiAuth.OpenPanel(_uiAuth.NamePanel);
				}
			});
		});
	}
	public void LoginEmail()
	{
		FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(_uiAuth.EmailLoginInput.text, _uiAuth.PasswordLoginInput.text).ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled || task.IsFaulted)
			{
				Debug.Log("SingIn Canceled");
				return;
			}
			else
			{
				AuthResult result = task.Result;
				FirebaseManager.Instance.User = result.User;
				Debug.Log(FirebaseManager.Instance.User.UserId);

				FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(FirebaseManager.Instance.User.UserId).GetValueAsync().ContinueWithOnMainThread(ctask =>
				{
					if (ctask.IsFaulted || ctask.IsCanceled)
					{
						Debug.Log("username Hatasi");
						return;
					}
					DataSnapshot snapshot = ctask.Result;
					if (snapshot.HasChildren)
					{
						_uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
						_uiAuth.OpenPanel(_uiAuth.GamePanel);
					}
					else
					{
						_uiAuth.EmailLoginInput.text = "";
						_uiAuth.PasswordLoginInput.text = "";
						_uiAuth.NameInput.text = "";
						_uiAuth.OpenPanel(_uiAuth.NamePanel);
					}
				});
			}
		});
	}
	public void SignUpEmail()
	{
		FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(_uiAuth.EmailSingUpInput.text, _uiAuth.PasswordSingUpInput.text).ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled || task.IsFaulted)
			{
				Debug.Log("SingUp Canceled");
				return;
			}
			else
			{
				AuthResult result = task.Result;
				FirebaseManager.Instance.User = result.User;
				Debug.Log("Kayit Basarili");
				_uiAuth.EmailSingUpInput.text = "";
				_uiAuth.PasswordSingUpInput.text = "";
				_uiAuth.OpenPanel(_uiAuth.LoginPanel);
			}
		});
	}
	public void CheckUserName(string text, Button confirmBTN)
	{
		FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByChild("username").EqualTo(text).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("username Hatasi");
				return;
			}
			DataSnapshot snapshot = task.Result;
			if (snapshot.HasChildren)
			{
				_userName = "";
				_uiAuth.NameWarningText.text = "Bu Kullanici Adi Var";
				_uiAuth.NameWarningText.color = Color.red;
				confirmBTN.interactable = false;
			}
			else
			{
				_userName = text;
				_uiAuth.NameWarningText.text = "Bu Kullanici Musait";
				_uiAuth.NameWarningText.color = Color.green;
				confirmBTN.interactable = true;
			}
		});
	}
	public void SetUserName()
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Userss").Child(FirebaseManager.Instance.User.UserId);
		reference.Child("username").SetValueAsync(_userName);
		reference.Child("level").SetValueAsync(1);
		reference.Child("currentxp").SetValueAsync(0);
		//reference.Child("neededExp").SetValueAsync(0);

		reference.Child("lvl_1").SetValueAsync(0);
		reference.Child("lvl_2").SetValueAsync(0);
		reference.Child("lvl_3").SetValueAsync(0);
		reference.Child("lvl_4").SetValueAsync(0);
		reference.Child("point").SetValueAsync(0);

		_uiAuth.NameText.text = _userName;
		FirebaseManager.Instance.GetPlayerLevelData(_uiAuth.GetLevelDataStart);
		_uiAuth.OpenPanel(_uiAuth.GamePanel);
		
	}
	public void SingOut()
	{
		if (FirebaseManager.Instance.User != null)
		{
			FirebaseManager.Instance.Auth.SignOut();
			FirebaseManager.Instance.User = null;
			_uiAuth.OpenPanel(_uiAuth.LoginPanel);
		}
	}
}
