using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthentication : MonoBehaviour
{
	[SerializeField] UIAuth uiAuth;
	string userName;
	GoogleSignInConfiguration configuration;
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
					uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
					uiAuth.OpenPanel(uiAuth.GamePanel);
				}              
			});
		}
		else
		{
			uiAuth.OpenPanel(uiAuth.LoginPanel);
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
						uiAuth.NameInput.text = "";
						uiAuth.OpenPanel(uiAuth.NamePanel);
					}
				});
			}
		});
	}
	public void GoogleSingUp()
	{
		GoogleSignIn.Configuration = configuration;
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
					uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
					uiAuth.OpenPanel(uiAuth.GamePanel);
				}
				else
				{
					uiAuth.NameInput.text = "";
					uiAuth.OpenPanel(uiAuth.NamePanel);
				}
			});
		});
	}
	public void LoginEmail()
	{
		FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(uiAuth.EmailLoginInput.text, uiAuth.PasswordLoginInput.text).ContinueWithOnMainThread(task =>
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
						uiAuth.NameText.text = snapshot.Child(FirebaseManager.Instance.User.UserId).Child("username").Value.ToString();
						uiAuth.OpenPanel(uiAuth.GamePanel);
					}
					else
					{
						uiAuth.EmailLoginInput.text = "";
						uiAuth.PasswordLoginInput.text = "";
						uiAuth.NameInput.text = "";
						uiAuth.OpenPanel(uiAuth.NamePanel);
					}
				});
			}
		});
	}
	public void SignUpEmail()
	{
		FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(uiAuth.EmailSingUpInput.text, uiAuth.PasswordSingUpInput.text).ContinueWithOnMainThread(task =>
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
				uiAuth.EmailSingUpInput.text = "";
				uiAuth.PasswordSingUpInput.text = "";
				uiAuth.OpenPanel(uiAuth.LoginPanel);
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
				userName = "";
				uiAuth.NameWarningText.text = "Bu Kullanici Adi Var";
				uiAuth.NameWarningText.color = Color.red;
				confirmBTN.interactable = false;
			}
			else
			{
				userName = text;
				uiAuth.NameWarningText.text = "Bu Kullanici Musait";
				uiAuth.NameWarningText.color = Color.green;
				confirmBTN.interactable = true;
			}
		});
	}
	public void SetUserName()
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Userss").Child(FirebaseManager.Instance.User.UserId);
		reference.Child("username").SetValueAsync(userName);
		reference.Child("level").SetValueAsync(1);
		reference.Child("currentxp").SetValueAsync(0);
		//reference.Child("neededExp").SetValueAsync(0);

		reference.Child("lvl_1").SetValueAsync(0);
		reference.Child("lvl_2").SetValueAsync(0);
		reference.Child("lvl_3").SetValueAsync(0);
		reference.Child("lvl_4").SetValueAsync(0);
		reference.Child("point").SetValueAsync(0);

		uiAuth.NameText.text = userName;
		FirebaseManager.Instance.GetPlayerLevelData(uiAuth.GetLevelDataStart);
		uiAuth.OpenPanel(uiAuth.GamePanel);
		
	}
	public void SingOut()
	{
		if (FirebaseManager.Instance.User != null)
		{
			FirebaseManager.Instance.Auth.SignOut();
			FirebaseManager.Instance.User = null;
			uiAuth.OpenPanel(uiAuth.LoginPanel);
		}
	}
}
