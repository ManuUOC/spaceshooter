//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Firebase.Extensions;
//using Firebase.Firestore;
//using UnityEngine.Events;
//using System;
//using System.Collections.ObjectModel;
//using Firebase.Auth;

//public class FirebaseController : MonoBehaviour
//{
//    FirebaseFirestore db;
//    public Firebase.Auth.FirebaseAuth _auth;
//    Firebase.FirebaseApp app;
//    private Firebase.Auth.FirebaseUser User;


//    private bool tenemosUsuario = false;
//    public bool signedIn;
//    public bool TenemosUsuario { get => tenemosUsuario; }

//    private bool inicializado;

//    [Header("Eventos")]
//    public GestorInicializado RetornoGestor;
//    public DetectadoUsuario OnDetectadoUsuario;
//    public RecibidoDatosUsuario recibidoDatosUsuario;
//    public OnRegistrarUsuario onRegistrarUsuario;
//    public OnLoginUsuario onLoginUsuario;

//    public static FirebaseController _firebase;

//    private void Awake()
//    {
//        if (_firebase != null && _firebase != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            _firebase = this;
//            DontDestroyOnLoad(_firebase);
//        }

//        if (this.RetornoGestor == null)
//        {
//            this.RetornoGestor = new GestorInicializado();
//        }
//        if (this.OnDetectadoUsuario == null)
//        {
//            this.OnDetectadoUsuario = new DetectadoUsuario();
//        }
//        if (this.recibidoDatosUsuario == null)
//        {
//            this.recibidoDatosUsuario = new RecibidoDatosUsuario();
//        }
//        if (this.onRegistrarUsuario == null)
//        {
//            this.onRegistrarUsuario = new OnRegistrarUsuario();
//        }
//        if (this.onLoginUsuario == null)
//        {
//            this.onLoginUsuario = new OnLoginUsuario();
//        }

//        Init();
//    }
//    private void Start()
//    {


//    }

//    /// <summary>
//    /// Actualiza los datos del usuario usando la clase DatosUsuarioFirebase
//    /// </summary>
//    /// <param name="idUsuario"></param>
//    /// <param name="usuario"></param>
//    /// <returns></returns>
//    private IEnumerator ActualizarDatosUsuario(PlayerData usuario)
//    {
//        DocumentReference docRef = db.Collection("Usuarios").Document(_auth.CurrentUser.UserId);
//        var updateTask = docRef.SetAsync(usuario);

//        yield return new WaitUntil(() => updateTask.IsCompleted);

//        if (updateTask.Exception != null)
//        {
//            Debug.LogWarning(GetErrorMessage(updateTask.Exception.Flatten().InnerExceptions));
//        }
//        else
//        {
//            //Debug.Log($"El usuario con id usuario {_auth.CurrentUser.UserId} se ha actualizado con éxito.");
//        }
//    }

//    /// <summary>
//    /// En esta funci�n lo que haremos ser� inicializar nuestra instancia de Firebase. 
//    /// </summary>
//    private void Init()
//    {
//        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//        {
//            var dependencyStatus = task.Result;
//            if (dependencyStatus == Firebase.DependencyStatus.Available)
//            {
//                // Create and hold a reference to your FirebaseApp,
//                // where app is a Firebase.FirebaseApp property of your application class.
//                app = Firebase.FirebaseApp.DefaultInstance;
//                db = FirebaseFirestore.DefaultInstance;
//                _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
//                _auth.StateChanged += AuthStateChanged;
//                inicializado = true;
//                AuthStateChanged(this, null);
//                //StartCoroutine(GetDatosRanking());

//                // Set a flag here to indicate whether Firebase is ready to use by your app.
//                RetornoGestor.Invoke(0, "");
//            }
//            else
//            {
//                UnityEngine.Debug.LogError(System.String.Format(
//                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
//                // Firebase Unity SDK is not safe to use here.
//                RetornoGestor.Invoke(-1, "El dependency status no está available");
//            }
//        });
//    }

//    /// <summary>
//    /// Esta función la usaremos para saber si un usuario está logeado en su dispositivo.
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void AuthStateChanged(object sender, EventArgs e)
//    {
//        tenemosUsuario = _auth.CurrentUser != null;
//        if (tenemosUsuario)
//        {
//            if (OnDetectadoUsuario != null)
//            {
//                OnDetectadoUsuario.Invoke(0, "");

//            }
//        }
//        if (_auth.CurrentUser != User)
//        {
//            signedIn = User != _auth.CurrentUser && _auth.CurrentUser != null;
//            if (!signedIn && User != null)
//            {
//                Debug.Log("Signed out " + User.UserId);
//            }
//            User = _auth.CurrentUser;
//            if (signedIn)
//            {
//                StartCoroutine(LeerDatosUsuario(User.UserId));
//            }
//        }
//    }

//    /// <summary>
//    /// Actualiza un parámetro de tipo string
//    /// </summary>
//    /// <param name="idUsuario"></param>
//    /// <param name="nombreCampo"></param>
//    /// <param name="contenidoCampo"></param>
//    /// <returns></returns>
//    private IEnumerator ActualizarDatosUsuario(string nombreCampo, string contenidoCampo)
//    {
//        DocumentReference docRef = db.Collection("Usuarios").Document(_auth.CurrentUser.UserId);
//        var updateTask = docRef.UpdateAsync(nombreCampo, contenidoCampo);

//        yield return new WaitUntil(() => updateTask.IsCompleted);

//        if (updateTask.Exception != null)
//        {
//            Debug.LogWarning((updateTask.Exception.Flatten().InnerExceptions));
//        }
//        else
//        {
//            //Debug.Log($"El campo {nombreCampo} del usuario con id usuario {_auth.CurrentUser.UserId} se ha actualizado con éxito a {contenidoCampo}.");
//        }
//    }

//    private IEnumerator BorrarUsuario()
//    {
//        var user = _auth.CurrentUser;

//        var deleteTask = user.DeleteAsync();

//        yield return new WaitUntil(() => deleteTask.IsCompleted);

//        if (deleteTask.Exception != null)
//        {
//            string error = GetErrorMessage(deleteTask.Exception.Flatten().InnerExceptions);
//            Debug.LogWarning($"Error al borrar el usuario: {error}");
//            //recibidoDatosUsuario.Invoke(-1, error, null);
//        }
//        else
//        {
//            Debug.Log("Borrado realizada con éxito");
//            //recibidoDatosUsuario.Invoke(0, "", datosUsuario);
//        }
//    }

//    private IEnumerator CrearNuevoUsuario(string idUsuario, string nickname, string email)
//    {
//        DocumentReference docRef = db.Collection("Usuarios").Document(idUsuario);

//        PlayerData usuario = new PlayerData();
//        usuario.email = email;
//        usuario.nombreUsuario = nickname;

//        var nuevoUsuarioTask = docRef.SetAsync(usuario);

//        yield return new WaitUntil(() => nuevoUsuarioTask.IsCompleted);

//        if (nuevoUsuarioTask.Exception != null)
//        {
//            Debug.LogWarning($"No se ha podido crear el usuario nuevo en firestore debido a: {nuevoUsuarioTask.Exception.Message}");
//        }
//        else
//        {
//            Debug.Log($"El usuario con id usuario {idUsuario} se ha registrado con éxito.");
//        }

//    }

//    public bool EsInvitado()
//    {
//        var user = _auth.CurrentUser;

//        if (user.IsAnonymous)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    /// Lee los datos del usuario
//    /// </summary>
//    /// <param name="idUsuario"></param>
//    /// <returns></returns>
//    public IEnumerator LeerDatosUsuario(string idUsuario)
//    {
//        DocumentReference docRef = db.Collection("Usuarios").Document(idUsuario);
//        var lecturaTask = docRef.GetSnapshotAsync();

//        yield return new WaitUntil(() => lecturaTask.IsCompleted);

//        if (lecturaTask.Exception != null)
//        {
//            string error = GetErrorMessage(lecturaTask.Exception.Flatten().InnerExceptions);
//            Debug.LogWarning($"Error al hacer la consulta: {error}");
//            recibidoDatosUsuario.Invoke(-1, error, null);
//        }
//        else
//        {
//            DocumentSnapshot consulta = lecturaTask.Result;
//            if (consulta.Exists)
//            {
//                //Return diccionario
//                PlayerData datosUsuario = consulta.ConvertTo<PlayerData>();
//                //Debug.Log(String.Format("Document data for {0} document: ", consulta.Id));
//                //Debug.Log(String.Format("Email: {0}", datosUsuario.email));
//                Debug.Log("[GestorFirebase](LeerDatosUsuario) Lectura realizada con éxito");
//                recibidoDatosUsuario.Invoke(0, "", datosUsuario);
//            }
//            else
//            {
//                Debug.LogWarning("[GestorFirebase](LeerDatosUsuario) La consulta está vacía");
//                recibidoDatosUsuario.Invoke(-1, "Couldn't read user data", null);
//            }
//        }
//    }

//    /// <summary>
//    /// Permite al usuario hacer login y autentica los datos
//    /// </summary>
//    /// <param name="mail"></param>
//    /// <param name="password"></param>
//    /// <returns></returns>
//    public IEnumerator LoginUsuario(string mail, string password)
//    {
//        var loginTask = _auth.SignInWithEmailAndPasswordAsync(mail, password);
//        yield return new WaitUntil(() => loginTask.IsCompleted);

//        if (loginTask.Exception != null)
//        {
//            string error = GetErrorMessage(loginTask.Exception.Flatten().InnerExceptions);
//            Debug.LogWarning($"Login failed with {error}");
//            onLoginUsuario.Invoke(-1, error);
//        }
//        else
//        {
//            Debug.Log($"Successfully logged in with {loginTask.Result.Email}");
//            User = loginTask.Result;
//            onLoginUsuario.Invoke(0, "");
//        }

//    }

//    public void LlamarBorrarUsuario()
//    {
//        StartCoroutine(BorrarUsuario());
//    }

//    public void LlamarLoginUsuario(string mail, string pass)
//    {
//        StartCoroutine(LoginUsuario(mail, pass));
//    }

//    public void LlamarLeerDatosUsuario()
//    {
//        if (_auth.CurrentUser != null)
//        {
//            StartCoroutine(LeerDatosUsuario(_auth.CurrentUser.UserId));
//        }
//        else
//        {
//            RetornoGestor.Invoke(0, "");
//        }
//    }

//    /// <summary>
//    /// Llama a la corrutina Registrar Usuario
//    /// </summary>
//    /// <param name="mail"></param>
//    /// <param name="pass"></param>
//    public void LlamarRegistrarUsuario(string mail, string nickname, string pass)
//    {
//        Debug.Log("");
//        if (string.IsNullOrEmpty(mail) && string.IsNullOrEmpty(pass))
//        {
//            StartCoroutine(CrearUsuarioVacio());
//        }
//        else
//        {
//            StartCoroutine(RegistrarUsuario(mail, nickname, pass));
//        }
//    }

//    public void LlamarActualizarDatosUsuario(PlayerData datosUsuario)
//    {
//        StartCoroutine(ActualizarDatosUsuario(datosUsuario));
//    }

//    // <summary>
//    /// Registra al usuario en Firebase
//    /// </summary>
//    /// <returns></returns>
//    private IEnumerator RegistrarUsuario(string mail, string nickname, string pass)
//    {
//        var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(mail, pass);

//        yield return new WaitUntil(() => registerTask.IsCompleted);

//        if (registerTask.Exception != null)
//        {
//            string error = GetErrorMessage(registerTask.Exception.Flatten().InnerExceptions);
//            Debug.LogWarning($"Failed to register task with{error}");
//            onRegistrarUsuario.Invoke(-1, error);
//        }
//        else
//        {
//            yield return StartCoroutine(CrearNuevoUsuario(_auth.CurrentUser.UserId, nickname, mail));
//            Debug.Log($"Successfully registered user {registerTask.Result.Email}");
//            onRegistrarUsuario.Invoke(0, "");
//        }
//    }

//    /// <summary>
//    /// Registra al usuario en Firebase
//    /// </summary>
//    /// <returns></returns>
//    private IEnumerator CrearUsuarioVacio()
//    {
//        var registerTask = _auth.SignInAnonymouslyAsync();

//        yield return new WaitUntil(() => registerTask.IsCompleted);

//        if (registerTask.Exception != null)
//        {
//            string error = GetErrorMessage(registerTask.Exception.Flatten().InnerExceptions);
//            Debug.LogWarning($"Failed to register task with{error}");
//            onRegistrarUsuario.Invoke(-1, error);
//        }
//        else
//        {
//            yield return StartCoroutine(CrearNuevoUsuario(_auth.CurrentUser.UserId, "", ""));
//            Debug.Log($"Successfully registered user {registerTask.Result.Email}");
//            onRegistrarUsuario.Invoke(0, "");
//        }
//    }

//    private IEnumerator RegistrarUsuarioAnonimo(string mail, string pass)
//    {
//        Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(mail, pass);

//        var linkTask = _auth.CurrentUser.LinkWithCredentialAsync(credential);
//        yield return new WaitUntil(() => linkTask.IsCompleted);

//        if (linkTask.Exception != null)
//        {
//            Debug.LogWarning($"Failed to register task with{linkTask.Exception.Message}");
//            onRegistrarUsuario.Invoke(-1, linkTask.Exception.Message);
//        }
//        else
//        {
//            //wait for this to get done
//            Debug.Log($"Successfully registered user {linkTask.Result.Email}");
//            yield return StartCoroutine(ActualizarDatosUsuario("email", mail));
//            onRegistrarUsuario.Invoke(0, "");
//        }
//    }

//    public void SignedOut()
//    {
//        if (_auth.CurrentUser == User)
//        {
//            _auth.SignOut();
//        }
//    }

//    public string GetErrorMessage(System.Collections.ObjectModel.ReadOnlyCollection<System.Exception> exceptions)
//    {
//        Firebase.FirebaseException firebaseEx = null;
//        bool firebaseExIsNull = true;
//        int i = 0;
//        while (firebaseExIsNull && i < exceptions.Count)
//        {
//            firebaseEx = exceptions[i] as Firebase.FirebaseException;
//            if (firebaseEx != null)
//            {
//                firebaseExIsNull = false;
//                var errorCode = (AuthError)firebaseEx.ErrorCode;
//                return GetErrorMessage(errorCode);
//            }
//            i++;
//        }

//        return "An error has occurred";
//    }

//    private string GetErrorMessage(AuthError errorCode)
//    {
//        var message = "";
//        switch (errorCode)
//        {
//            case AuthError.AccountExistsWithDifferentCredentials:
//                //message = "Ya existe la cuenta con credenciales diferentes";
//                message = "An account already exists with different credentials";
//                break;
//            case AuthError.MissingPassword:
//                //message = "Hace falta el Password";
//                message = "Missing password";
//                break;
//            case AuthError.WeakPassword:
//                //message = "El password es debil";
//                message = "Weak password";
//                break;
//            case AuthError.WrongPassword:
//                //message = "El password es Incorrecto";
//                message = "Wrong password";
//                break;
//            case AuthError.EmailAlreadyInUse:
//                //message = "Ya existe la cuenta con ese correo electrónico";
//                message = "Email already in use";
//                break;
//            case AuthError.InvalidEmail:
//                //message = "Correo electronico invalido";
//                message = "Invalid email";
//                break;
//            case AuthError.MissingEmail:
//                //message = "Hace falta el correo electrónico";
//                message = "Missing email";
//                break;
//            case AuthError.UserNotFound:
//                message = "User not found";
//                break;
//            default:
//                //message = "Ocurrió un error";
//                message = errorCode.ToString();
//                break;
//        }
//        return message;
//    }
//}
//[Serializable]
//public class GestorInicializado : UnityEvent<int, string> { }
//[Serializable]
//public class DetectadoUsuario : UnityEvent<int, string> { }
//[Serializable]
//public class RecibidoDatosUsuario : UnityEvent<int, string, PlayerData> { }
//[System.Serializable]
//public class OnRegistrarUsuario : UnityEvent<int, string> { }
//[System.Serializable]
//public class OnLoginUsuario : UnityEvent<int, string> { }