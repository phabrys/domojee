﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
namespace Jeedom.Model
{
    [DataContract]
    public class Command : INotifyPropertyChanged
    {
        #region Propriétés sans notification de changement

        private string _name;

        [DataMember]
        public string logicalId;

        [DataMember]
        public string eqLogic_id;

        [DataMember]
        public string id;

        [DataMember]
        public string type;

      /*  [DataMember(Name = "configuration")]
        private CommandConfiguration _configuration;

        public CommandConfiguration configuration

        {
            get
            {
                if (_configuration == null)
                    return new CommandConfiguration();
                else
                    return _configuration;
            }

            set
            {
                _configuration = value;
            }
        }*/
        [DataMember(Name = "display")]
        private CommandDisplay _display;

        public CommandDisplay display

        {
            get
            {
                if (_display == null)
                    return new CommandDisplay();
                else
                    return _display;
            }

            set
            {
                _display = value;
            }
        }

        [DataMember]
        public string subType;

        //private string _unite;

        [DataMember]
        private bool _isVisible = true;
        public bool isVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = Convert.ToBoolean(value);

            }
        }

        [DataMember(Name = "value")]
        private string _value;

        private bool _updating = false;

        public EqLogic Parent;

        #endregion Propriétés sans notification de changement

        #region Propriétés avec notification de changement

        public double datetime;

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                 _value = value;
                // ExecCommand();
                NotifyPropertyChanged();
                if (Parent != null)
                {
                    switch (name)
                    {
                        case "Consommation":
                            Parent.Consommation = value + " ";// + unite;
                            break;

                        case "Puissance":
                            Parent.Puissance = value + " ";// + unite;
                            break;

                        case "Etat":
                            Parent.State = value;
                            break;
                    }
                }
            }
        }

        [DataMember]
        public String name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public bool Updating
        {
            get
            {
                return _updating;
            }

            set
            {
                _updating = value;
                NotifyPropertyChanged();
            }
        }

      /*  [DataMember]
        public string unite
        {
            get
            {
                return _unite;
            }

            set
            {
                _unite = value;
                NotifyPropertyChanged();
            }
        }*/

        #endregion Propriétés avec notification de changement


        #region Private Methods
        private async Task ExecCommand()
        {
                this.Updating = true;
                await RequestViewModel.Instance.ExecuteCommand(this);
                this.Updating = false;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
   
        #endregion Private Methods
    }
}