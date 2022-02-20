using System;
using System.Collections.Generic;
using System.Text;
using ViewModelBaseLibrary.VMBase;
using Enums;
using System.Diagnostics.CodeAnalysis;
using ErrorChecker;

namespace Models
{
    public partial class Note : ViewModelBase, IEquatable<Note>
    {
        #region Fields
        
        #region Error Checker

        private FieldChecker m_errorChecker;//Error checker instance

        #endregion

        #region EditIndicators
        //Indictaes what field was edited
        private byte m_SNameEdited = 0;
        private byte m_NameEdited = 0;
        private byte m_LNameEdited = 0;
        private byte m_PhoneEdited = 0;
        private byte m_AdressEdited = 0;
        private byte m_DescEdited = 0;

        #endregion

        #region Error Indicators

        private byte m_SNameError = 0;
        private byte m_NameError = 0;
        private byte m_LNameError = 0;
        private byte m_PhoneError = 0;
        private byte m_AdressError = 0;
        private byte m_DescError = 0;

        #endregion


        #region Command fields

        private bool m_remove; // remove operation should be performed

        private byte m_operation;// 1 - add, 2 - edit, 3 - error in the field

        private bool m_isNew; //Determine if this note is new(used) 

        private bool m_IsEditable;//Determine if user has edit rules        

        #endregion

        #region Note fields

        private Guid m_id;
        private string m_surename;
        private string m_name;
        private string m_lastname;
        private string m_phone;
        private string m_adress;
        private string m_description;

        #endregion

        #endregion

        #region Properties
        
        #region Properties that determines what field was edited

        public byte SNameEdited
        {
            get => m_SNameEdited;
            set => SetProperty<byte>(ref m_SNameEdited, value, nameof(SNameEdited));
        } 

        public byte NameEdited
        {
            get => m_NameEdited;
            set => SetProperty<byte>(ref m_NameEdited, value, nameof(NameEdited));
        }

        public byte LNameEdited
        {
            get => m_LNameEdited;
            set => SetProperty<byte>(ref m_LNameEdited, value, nameof(LNameEdited));
        }

        public byte AdressEdited
        {
            get => m_AdressEdited;
            set => SetProperty<byte>(ref m_AdressEdited, value, nameof(AdressEdited));
        }

        public byte PhoneEdited
        {
            get => m_PhoneEdited;
            set => SetProperty<byte>(ref m_PhoneEdited, value, nameof(PhoneEdited));
        }

        public byte DescEdited
        {
            get => m_DescEdited;
            set => SetProperty<byte>(ref m_DescEdited, value, nameof(DescEdited));
        }
        #endregion

        #region Properties that determines where error occured

        public byte SNameError
        {
            get => m_SNameError;
            set => SetProperty<byte>(ref m_SNameError, value, nameof(SNameError));
        }

        public byte NameError
        {
            get => m_NameError;
            set => SetProperty<byte>(ref m_NameError, value, nameof(NameError));
        }

        public byte LNameError
        {
            get => m_LNameError;
            set => SetProperty<byte>(ref m_LNameError, value, nameof(LNameError));
        }

        public byte AdressError
        {
            get => m_AdressError;
            set => SetProperty<byte>(ref m_AdressError, value, nameof(AdressError));
        }

        public byte PhoneError
        {
            get => m_PhoneError;
            set => SetProperty<byte>(ref m_PhoneError, value, nameof(PhoneError));
        }

        public byte DescError
        {
            get => m_DescError;
            set => SetProperty<byte>(ref m_DescError, value, nameof(DescError));
        }

        #endregion

        #region Sould Be Marked as Must Be Edited

        //Controls the edit controller system.

        public bool ShouldValidatorBeEnabled { get; set; }//Controlls the validator system

        public bool ShouldBeMarkedAsEdit { get; set; }//Controlls wether editing is enabled

        #endregion

        #region Operation Type

        public bool IsNew
        {
            get => m_isNew;
            set => SetProperty<bool>(ref m_isNew, value, nameof(IsNew));
        }

        public bool Remove 
        { 
            get => m_remove; 
            set => SetProperty<bool>(ref m_remove, value, nameof(Remove)); 
        }

        public byte OperationProp // 1 - Add 2 - edit 3 - error occured
        { 
            get=> m_operation; 
            set => SetProperty<byte>(ref m_operation, value, 
                nameof(OperationProp)); 
        }

        public bool IsEditable { get=> m_IsEditable; 
            
            set=> SetProperty<bool>(ref m_IsEditable, value, nameof(IsEditable)); }


        #endregion

        #region Note Main Properties

        public Guid Id 
        { 
            get=> m_id;
            
            set=> SetProperty<Guid>(ref m_id, value, nameof(Id)); 
        }//Id of a Note

        
        public string Surename 
        { 
            get => m_surename;
            set { SetProperty<string>(ref m_surename, value, nameof(Surename));
                TextFieldValidator(m_surename, ShouldValidatorBeEnabled, 20, 0);
                ShouldBeEdit(0);                
            }
        }//Surename

        
        public string Name
        {
            get => m_name;
            set { SetProperty<string>(ref m_name, value, nameof(Name));
                TextFieldValidator(m_name, ShouldValidatorBeEnabled, 20, 1);
                ShouldBeEdit(1);                
            }
         
        }//Name

        
        public string Lastname 
        { 
            get=> m_lastname;
            set { SetProperty<string>(ref m_lastname, value, nameof(Lastname)); 
                
                TextFieldValidator(m_lastname, ShouldValidatorBeEnabled, 20,2);
                ShouldBeEdit(2);
            } 
        
        }//Lastname

        
        public string Phone 
        { 
            get => m_phone;
            set { SetProperty<string>(ref m_phone, value, nameof(Phone)); 
                
                NumberValidator(m_phone, ShouldValidatorBeEnabled, 20,3);
                ShouldBeEdit(3);
            }
        }//Phone

       
        public string Adress 
        { 
            get=> m_adress;
            set { SetProperty<string>(ref m_adress, value, nameof(Adress)); 
                
                TextFieldValidator(m_adress, ShouldValidatorBeEnabled, 200,4);
                ShouldBeEdit(4);
            }
        }//Adress

        
        public string Description 
        { 
            get=> m_description;
            set { SetProperty<string>(ref m_description, value, nameof(Description)); 
                
                TextFieldValidator(m_description, ShouldValidatorBeEnabled, 3000,5);
                ShouldBeEdit(5);
            }
        }//Description

        #endregion

        #endregion
        //Empty ctor used for deserialization
        public Note()
        {
            if(m_errorChecker == null)
            m_errorChecker = new FieldChecker();

            ShouldBeMarkedAsEdit = false;
        }

        //Ctor with arguments
        public Note(Guid id, string surename, string name, string lastname,
            string phone, string adress, string description)
        {
            if (m_errorChecker == null)
                m_errorChecker = new FieldChecker();

            IsNew = false;

            this.ShouldBeMarkedAsEdit = false;

            this.ShouldValidatorBeEnabled = false;

            this.Id = id;
            this.Surename = surename;
            this.Name = name;
            this.Lastname = lastname;
            this.Phone = phone;
            this.Adress = adress;
            this.Description = description;

            this.ShouldBeMarkedAsEdit = true;
            
            this.IsEditable = false;

            this.ShouldValidatorBeEnabled = true;
        }
        /// <summary>
        /// Overloaded ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surename"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <param name="phone"></param>
        /// <param name="adress"></param>
        /// <param name="description"></param>
        /// <param name="isNew"></param>
        public Note(Guid id, string surename, string name, string lastname,
            string phone, string adress, string description, bool isNew)
            :this
            (id, surename, name, lastname,
                phone, adress, description)
        {
            if (m_errorChecker == null)
                m_errorChecker = new FieldChecker();

            if (isNew)
            {                
                OperationProp = 1;
                IsNew = isNew;
            }
            else
            {
                IsNew = !isNew;
            }
        }

        /// <summary>
        /// Sets new value to a note in db
        /// </summary>
        /// <param name="note"></param>
        public void SetNewValues(Note note)
        {
            this.Surename = note.Surename;
            this.Name = note.Name;
            this.Lastname = note.Lastname;
            this.Phone = note.Phone;
            this.Adress = note.Adress;
            this.Description = note.Description;
        }

        /// <summary>
        /// Implementation of the IEquatable<Note>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Note other)
        {
            if (other == null)
                return false;
            if (this.Id.Equals(other.Id)&& 
                this.Surename.Equals(other.Surename)
                && this.Name.Equals(other.Name)
                && this.Lastname.Equals(other.Lastname)
                && this.Phone.Equals(other.Phone)
                && this.Adress.Equals(other.Adress)
                && this.Description.Equals(other.Description))
            {
                return true;
            }

            return false;
            
        }
    }

    public partial class Note
    {
        /// <summary>
        /// Sets the propriate value to the bool field that indicates
        /// what field was edited
        /// </summary>
        /// <param name="index"></param> prop index
        private void ShouldBeEdit(int index)
        {
            if (ShouldBeMarkedAsEdit)
            {
                if(this.OperationProp != 2)
                    this.OperationProp = 2;

                switch (index)
                {
                    case 0:
                        if(SNameError==0)
                        SNameEdited = 1;
                        break;
                        
                    case 1:
                        if (NameError == 0)
                            NameEdited = 1;
                        break;

                    case 2:
                        if (LNameError == 0)
                            LNameEdited = 1;
                        break;

                    case 3:
                        if (PhoneError == 0)
                            PhoneEdited = 1;
                        break;

                    case 4:
                        if (AdressError == 0)
                            AdressEdited = 1;
                        break;

                    case 5:
                        if (DescError == 0)
                            DescEdited = 1;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Validate Text fields (There must not be digits)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="IsValidated"></param>
        /// <param name="maxCount"></param>
        /// <param name="fieldIndex"></param>
        private void TextFieldValidator(string field, bool IsValidated, int maxCount, int fieldIndex)
        {
            if (IsValidated)
            {
                if (!m_errorChecker.IsTextFieldCorrect(field, maxCount))
                { OperationProp = 3; }
              
                DefineField(fieldIndex);
            }
        }
        /// <summary>
        ///  Text fields (There must not be letters)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="IsValidated"></param>
        /// <param name="maxCount"></param>
        /// <param name="fieldIndex"></param>
        private void NumberValidator(string field, bool IsValidated, int maxCount, int fieldIndex)
        {
            if (IsValidated)
            {
                if (!m_errorChecker.IsPhoneCorrect(field, maxCount))
                { OperationProp = 3; }

                DefineField(fieldIndex);                
            }
        }
        /// <summary>
        /// Define field that will be edited according to its index`
        /// </summary>
        /// <param name="index"></param>
        private void DefineField(int index)
        {
            switch (index)
            {
                case 0:
                    if (OperationProp.Equals(3))
                    {
                        SNameError = 1;
                    }
                    else
                    {
                        SNameError = 0;
                    }                    
                    break;
                case 1:
                    if (OperationProp.Equals(3))
                        NameError = 1;
                    else
                        NameError = 0;
                    break;
                case 2:
                    if (OperationProp.Equals(3))
                    {
                        LNameError = 1;
                    }
                    else
                        LNameError = 0;    
                    break;
                case 3:
                    if (OperationProp.Equals(3))
                        PhoneError = 1;
                    else
                        PhoneError = 0;
                    break;               
                default:
                    break;  
            }
        }

    }

}
