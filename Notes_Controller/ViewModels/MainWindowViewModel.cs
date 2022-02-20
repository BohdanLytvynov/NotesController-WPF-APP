using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibrary.VMBase;
using ViewModelBaseLibrary.CommandsBase;
using System.Collections.ObjectModel;
using Models;
using DataApiProvider;
using LogModels;
using System.Windows.Input;
using ConfigController;
using Notes_Controller.WPFFunctions;
using System.Security;
using System.Windows;
using System.Windows.Media;
using MethodCallLibrary;
using NoteController.Controller;
using System.Diagnostics;
using Models.ServerModel;
using Notes_Controller.Views;

namespace Notes_Controller.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private DotRotateLoader m_loader;

        List<Notes> m_addNotesList;

        List<Notes> m_EditNotesList;

        List<Guid> m_RemoveNotesList;

        private int m_SelectedNoteIndex = -1;

        private Window m_DispatcherWindow;

        #region NoteController
        private NoteController.Controller.Notes_Controller m_NoteController;
        #endregion

        private static string m_messageInit;

        #region Visisbility of buttons
        private Visibility m_loginV;
        private Visibility m_LogOutV;
        private Visibility m_UpdateV;
        private Visibility m_RemoveButtonV;
        #endregion

        #region Visibility of Grids
        private Visibility m_loginGridVisibility;

        private Visibility m_ControllerGrid;
        #endregion
        private bool isAdmin = false;
        private string m_Message;
        private string m_ErrorMessage;
        private ObservableCollection<Note> m_ModelCollection;
        private DataProvider m_dataProvider;
        private ObservableCollection<LogBase> m_LogCollection;
        private string m_urlHost;

        #region UesrLogin fields
        private string m_login;

        private int[] m_pass;

        #endregion

        #endregion

        #region UserLogin Properties
        public string Login
        {
            get => m_login;

            set
            {
                SetProperty<string>(ref m_login, value, nameof(Login));
                OnLoginButtonPressed.CanExecute(null);
            }
        }


        #endregion

        #region Properties

        #region Visibility of Buttons

        public Visibility RemoveButtonV
        {
            get => m_RemoveButtonV;
            set => SetProperty<Visibility>(ref m_RemoveButtonV, value,
                nameof(RemoveButtonV));
        }

        public Visibility LoginButtonV
        {
            get => m_loginV;
            set => SetProperty<Visibility>(ref m_loginV, value,
                nameof(LoginButtonV));
        }

        public Visibility LogOutButtonV
        {
            get => m_LogOutV;
            set => SetProperty<Visibility>(ref m_LogOutV, value,
                nameof(LogOutButtonV));
        }

        public Visibility UpdateButtonV
        {
            get => m_UpdateV;
            set => SetProperty<Visibility>(ref m_UpdateV, value,
                nameof(UpdateButtonV));
        }

        #endregion

        #region Grid Visibility Properties

        public Visibility LogingridV
        {
            get => m_loginGridVisibility;
            set => SetProperty<Visibility>(ref m_loginGridVisibility, value,
                nameof(LogingridV));
        }



        public Visibility ControllerGridV
        {
            get => m_ControllerGrid;
            set => SetProperty<Visibility>(ref m_ControllerGrid, value,
                nameof(ControllerGridV));
        }

        #endregion

        public int SelectedNoteIndex
        {
            get => m_SelectedNoteIndex;
            set => SetProperty<int>(ref m_SelectedNoteIndex, value, nameof(SelectedNoteIndex));
        }

        public string Message
        {
            get => m_Message;

            set => SetProperty<string>(ref m_Message, value, nameof(Message));
        }

        public string ErrorMessage
        {
            get => m_ErrorMessage;

            set => SetProperty<string>(ref m_ErrorMessage, value, nameof(ErrorMessage));
        }

        public ObservableCollection<Note> ModelCollection
        {
            get => m_ModelCollection;

            set => SetProperty<ObservableCollection<Note>>(ref m_ModelCollection, value,
                nameof(ModelCollection));
        }
        public ObservableCollection<LogBase> LogCollection
        {
            get => m_LogCollection;
            set => m_LogCollection = value;
        }

        public string UrlHost
        {
            get => m_urlHost;

            set
            {
                SetProperty<string>(ref m_urlHost, value, nameof(UrlHost));

                OnSaveSetButtonPressed.CanExecute(null);

                OnReconfigureHttpClientButtonPressed.CanExecute(null);

                OnGetNotesButtonPressed.CanExecute(null);
            }
        }
        #endregion

        #region Command Properties

        public ICommand OnSaveSetButtonPressed { get; }

        public ICommand OnReconfigureHttpClientButtonPressed { get; }

        public ICommand OnGetNotesButtonPressed { get; }

        public ICommand OnLoginButtonPressed { get; }

        public ICommand OnLogOutButtonPressed { get; }

        public ICommand OnAddUserButtonPressed { get; }

        public ICommand OnRemoveUserButtonPressed { get; }

        public ICommand OnUpdateDbButtonPressed { get; }

        #endregion

        #region Static ctor
        static MainWindowViewModel()
        {
            m_messageInit = "You are not signed in. Please sign in as admin or our member.";
        }
        #endregion

        #region Ctor
        public MainWindowViewModel(Window w)
        {
            m_loader = new DotRotateLoader();

            m_addNotesList = new List<Notes>();

            m_EditNotesList = new List<Notes>();

            m_RemoveNotesList = new List<Guid>();

            m_DispatcherWindow = w;

            m_loginV = Visibility.Visible;

            m_LogOutV = Visibility.Hidden;

            m_UpdateV = Visibility.Hidden;

            m_RemoveButtonV = Visibility.Hidden;

            m_Message = m_messageInit;

            m_loginGridVisibility = Visibility.Visible;

            m_ControllerGrid = Visibility.Hidden;

            Congiguration.Read().TryGetValue("host", out m_urlHost);

            OnpropertyChanged(nameof(UrlHost));

            OnSaveSetButtonPressed = new CommandTemplate(
                OnSaveSetButtonPressedExecute,
                CanSaveSetButtonPressedExecute
                );

            OnReconfigureHttpClientButtonPressed = new CommandTemplate(
                OnConfigureHttpClientPressed,
                CanreconfigureHttpClientButtonPressed
                );

            OnGetNotesButtonPressed = new CommandTemplate(
                OnGetNotesButtonPressedExecute,
                CanOnGetButtonPressed
                );

            OnLoginButtonPressed = new CommandTemplate(
                OnLoginButtonPressedExecute,
                CanOnLoginButtonPressed
                );

            OnLogOutButtonPressed = new CommandTemplate(
                OnLogOutButtonPressedExecute,
                CanLogOutButtonPrssed
                );

            OnAddUserButtonPressed = new CommandTemplate(
                OnAddButtonPressedExecute,
                CanOnAddUserButtonPressed
                );

            OnRemoveUserButtonPressed = new CommandTemplate(
                OnRemoveUserButtonPressedExecute,
                CanOnRemoveButtonPressed
                );

            OnUpdateDbButtonPressed = new CommandTemplate(
                OnUpdateDBButtonPressed,
                CanOnUpdateDbButtonPressed
                );

            ModelCollection = new ObservableCollection<Note>();

            m_dataProvider = new DataProvider(UrlHost);

            m_LogCollection = new ObservableCollection<LogBase>();

            m_NoteController = new NoteController.Controller.Notes_Controller();

        }
        #endregion

        #region Methods
       /// <summary>
       ///Gets the password that was written to password box
       /// </summary>
       /// <param name="Password"></param>
        public void SetPassword(string Password)
        {
            m_pass = new int[Password.Length];

            for (int i = 0; i < Password.Length; i++)
            {
                m_pass[i] = (int)Password[i];
            }

            Password = null;
        }

        /// <summary>
        /// Writes collection to Observable collection
        /// </summary>
        /// <typeparam name="Tlog"></typeparam>
        /// <param name="ObservLog"></param>
        /// <param name="list"></param>
        /// <param name="dispatcher"></param>
        private void WriteListToObservLog<Tlog>(ObservableCollection<Tlog> ObservLog, IList<Tlog> list, Window dispatcher)
        {
            if (list.Count != 0)
            {
                dispatcher.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ObservLog.Add(list[i]);
                    }
                });

                list.Clear();
            }
        }

        #region Command Methods
        #region On Save Setting Button Pressed

        private bool CanSaveSetButtonPressedExecute(object p)
        {
            if (String.IsNullOrWhiteSpace(m_urlHost))
                return false;

            return true;
        }

        private void OnSaveSetButtonPressedExecute(object p)
        {
            Dictionary<string, string> setdic = new Dictionary<string, string>();

            setdic.Add("host", UrlHost);

            Congiguration.WriteSettingsToFileXML(setdic);
        }

        #endregion

        #region On ReconfigureHttpclientPressed

        private bool CanreconfigureHttpClientButtonPressed(object p)
        {
            if (String.IsNullOrWhiteSpace(UrlHost))
            {
                return false;
            }
            return true;
        }

        private void OnConfigureHttpClientPressed(object p)
        {
            m_dataProvider.URLHost = UrlHost;
        }

        #endregion

        #region OnGet Notes button pressed

        private bool CanOnGetButtonPressed(object p)
        {
            if (String.IsNullOrWhiteSpace(UrlHost))
            {
                return false;
            }
            return true;
        }

        private async void OnGetNotesButtonPressedExecute(object p)
        {
            await Task.Run(() => 
            {
                ShowLoader("Performing Request. Please Wait...", 24);
                
                GetNotes();

                HideLoader();
            
            });
        }

        private void ShowLoader(string message, double fontsize)
        {
            m_DispatcherWindow.Dispatcher.Invoke(() =>
            {
                m_loader.WindowText = message;

                m_loader.FontSize = fontsize;

                m_loader.Topmost = true;
               
                m_loader.Show();
            });
        }
       
        private void HideLoader()
        {
            m_DispatcherWindow.Dispatcher.Invoke(() =>
            {
                m_loader.Close();

                m_loader = new DotRotateLoader();
            });            
        }

        private void GetNotes()
        {                   
            var tempLogList = new List<LogBase>();

            List<Note> temp = null;

            temp = (List<Note>)m_dataProvider.GetNotesFromServer(tempLogList);
            
            if (temp == null)
            {
                HideLoader();

                CommonFunctions.ShowMessageBox("Error Occured! See Log",
                   System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);                
            }
            else
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (!ModelCollection.Contains(temp[i]))
                    {
                        m_DispatcherWindow.Dispatcher.Invoke(() =>
                        {
                            if (isAdmin)
                            {
                                temp[i].IsEditable = true;
                                temp[i].ShouldValidatorBeEnabled = true;
                            }

                            temp[i].ShouldBeMarkedAsEdit = true;
                            ModelCollection.Add(temp[i]);
                        });
                    }
                }
            }

            WriteListToObservLog<LogBase>(LogCollection, tempLogList, m_DispatcherWindow);
        }

        #endregion

        #region On Login Button Pressed

        private bool CanOnLoginButtonPressed(object p)
        {
            if (String.IsNullOrWhiteSpace(Login))
            {
                return false;
            }
            return true;
        }

        private async void OnLoginButtonPressedExecute(object p)
        {
            var tempLogList = new List<LogBase>();

            await Task.Run(() =>
            {
                ShowLoader("Performing Request... Please Wait...", 24);

                List<bool> resp = null;

                resp = m_dataProvider.PostUserAndGetAuthStatusEnvelope(
                    new User()
                    {
                        Login = m_login,
                        Pass =
                    m_pass
                    }, tempLogList
                   );

                HideLoader();

                if (resp == null)
                {                   
                    CommonFunctions.ShowMessageBox("Error Occured! See Log",
                       System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    if (!resp[0])//User Not found
                    {
                        ErrorMessage = "User not found! Check Input!";
                    }
                    else
                    {
                        if (resp[1])//User Authorized
                        {
                            ErrorMessage = String.Empty;

                            LogingridV = Visibility.Hidden;

                            LoginButtonV = Visibility.Hidden;

                            UpdateButtonV = Visibility.Visible;

                            LogOutButtonV = Visibility.Visible;

                            ControllerGridV = Visibility.Visible;

                            if (resp[2])//User Admin
                            {
                                Message = $"You are signed as {Login} with Admin Rules";

                                isAdmin = true;

                                RemoveButtonV = Visibility.Visible;

                                for (int i = 0; i < ModelCollection.Count; i++)
                                {
                                    ModelCollection[i].IsEditable = true;

                                    ModelCollection[i].ShouldValidatorBeEnabled = true;
                                }

                            }
                            else // Ordinary user
                            {
                                Message = $"You are signed as {Login} with User Rules";
                            }
                        }
                        else
                        {
                            ErrorMessage = "Incorrect Password!";
                        }
                    }
                }

                WriteListToObservLog<LogBase>(LogCollection, tempLogList, m_DispatcherWindow);

            });

        }

        #endregion

        #region OnLog out button pressed

        private bool CanLogOutButtonPrssed(object p)
            => true;

        private async void OnLogOutButtonPressedExecute(object p)
        {

            if (ControllerGridV.Equals(Visibility.Visible))
            {
                ControllerGridV = Visibility.Hidden;

                if (isAdmin)
                {
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < ModelCollection.Count; i++)
                        {
                            ModelCollection[i].IsEditable = false;
                        }
                    });
                }

            }

            LogingridV = Visibility.Visible;

            LogOutButtonV = Visibility.Hidden;

            LoginButtonV = Visibility.Visible;

            RemoveButtonV = Visibility.Hidden;

            Message = m_messageInit;

            isAdmin = false;
        }

        #endregion


        #region OnAdd User Button Pressed

        private bool CanOnAddUserButtonPressed(object p)
        => true;

        private void OnAddButtonPressedExecute(object p)
        {
            string m = "Enter Value";

            m_NoteController.AddEntity(new Note(
                Guid.NewGuid(), m, m, m, m, m, m, true
                )
            { IsEditable = true }, ModelCollection);

            Debug.WriteLine("In Add");
        }

        #endregion

        #region OnRemove Button pressed

        private bool CanOnRemoveButtonPressed(object p)
        {
            if (SelectedNoteIndex >= 0)
                return true;

            return false;
        }

        private void OnRemoveUserButtonPressedExecute(object p)
        {
            m_NoteController.RemoveEntity(SelectedNoteIndex, ModelCollection);
        }

        #endregion

        #region On Update Db Button Pressed

        private bool CanOnUpdateDbButtonPressed(object p)
        {
            return true;
        }

        private async void OnUpdateDBButtonPressed(object p)
        {
            var temp = new List<Note>();

            var tempLogList = new List<LogBase>();

            bool[] result = new bool[4];

            await Task.Run(() =>
            {
                ShowLoader("Updating Request... Pkease Wait..", 24);

                for (int i = 0; i < ModelCollection.Count; i++)
                {
                    if (ModelCollection[i].OperationProp == 3)
                    {
                        HideLoader();

                        CommonFunctions.ShowMessageBox("Incorrect Note Field!"
                            , MessageBoxButton.OK, MessageBoxImage.Error);

                        m_addNotesList.Clear();
                        m_EditNotesList.Clear();
                        m_RemoveNotesList.Clear();
                        result[0] = false;
                        break;
                    }

                    if (ModelCollection[i].IsNew && !ModelCollection[i].Remove)
                    {
                        m_addNotesList.Add(
                            new Notes(ModelCollection[i].Id,
                            ModelCollection[i].Surename,
                            ModelCollection[i].Name,
                            ModelCollection[i].Lastname,
                            ModelCollection[i].Phone,
                            ModelCollection[i].Adress,
                            ModelCollection[i].Description
                            ));
                    }

                    if (isAdmin)
                    {
                        if (ModelCollection[i].OperationProp == 2 &&
                        !ModelCollection[i].Remove && !ModelCollection[i].IsNew)
                        {
                            m_EditNotesList.Add
                                (
                                    new Notes(                                        
                                            ModelCollection[i].Id,
                                            ModelCollection[i].Surename,
                                ModelCollection[i].Name,
                                ModelCollection[i].Lastname,
                                ModelCollection[i].Phone,
                                ModelCollection[i].Adress,
                                ModelCollection[i].Description   
                                
                                ));
                        }

                        if (ModelCollection[i].Remove && !ModelCollection[i].IsNew)
                        {
                            m_RemoveNotesList.Add(ModelCollection[i].Id);
                        }
                    }

                    result[0] = true;
                }

                if (!result[0])
                {
                    return;
                }

                result[1] = m_dataProvider.AddNotesRequest(tempLogList, m_addNotesList);

                result[2] = m_dataProvider.EditNotesRequest(tempLogList, m_EditNotesList);

                result[3] = m_dataProvider.RemoveRequest(tempLogList, m_RemoveNotesList);

                m_addNotesList.Clear();
                m_EditNotesList.Clear();
                m_RemoveNotesList.Clear();

                m_DispatcherWindow.Dispatcher.Invoke(() =>
                { ModelCollection.Clear(); });

                GetNotes();

                HideLoader();

                CommonFunctions.ShowMessageBox(CreateMessage(result),
                    MessageBoxButton.OK, MessageBoxImage.Information);               
            });


        }

        private string CreateMessage(bool[] array)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            {
                switch (i)
                {
                    case 1:
                        if (array[i])
                        {
                            sb.Append("\nAdd Request Performed!");
                        }
                        else
                        {
                            sb.Append("\nAdd Request Failure!");
                        }
                        break;
                    case 2:
                        if (array[i])
                        {
                            sb.Append("\nEdit Request Performed!");
                        }
                        else
                        {
                            sb.Append("\nEdit Request Failure!");
                        }
                        break;
                    case 3:
                        if (array[i])
                        {
                            sb.Append("\nRemove Request Performed!");
                        }
                        else
                        {
                            sb.Append("\nRemove Request Failure!");
                        }
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #endregion


    }
}



