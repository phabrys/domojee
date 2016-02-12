﻿using Jeedom.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Jeedom.Model
{
    [DataContract]
    public class EqLogic : INotifyPropertyChanged
    {
        #region Public Fields

        public int ColSpan = 1;

        [DataMember]
        public string isEnable;

        [DataMember]
        public string isVisible;

        [DataMember]
        public string logicalId;

        [DataMember]
        public string object_id;

        public JdObject Parent;

        public int RowSpan = 1;

        #endregion Public Fields

        #region Private Fields

        private ObservableCollection<Command> _cmds;

        private string _consommation;

        private EqLogicDisplay _display;

        private string _eqtype_name;

        private RelayCommand<object> _execCommandByLogicalID;
        private RelayCommand<object> _execCommandByName;

        private RelayCommand<object> _execCommandByType;
        private string _name;

        private bool _onVisibility = false;

        private string _puissance;

        private string _state;

        private bool _updating;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        [DataMember]
        public ObservableCollection<Command> cmds
        {
            get
            {
                return _cmds;
            }

            set
            {
                _cmds = value;
                NotifyPropertyChanged();
            }
        }

        public string Consommation
        {
            get
            {
                return _consommation;
            }
            set
            {
                _consommation = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public EqLogicDisplay display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                if (_display != null)
                {
                    if (_display.customParameters != null)
                    {
                        if (_display.customParameters.DomojeeColSpan != 0)
                            ColSpan = _display.customParameters.DomojeeColSpan;
                        if (_display.customParameters.DomojeeRowSpan != 0)
                            RowSpan = _display.customParameters.DomojeeRowSpan;
                    }
                }
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public String eqType_name
        {
            get
            {
                return _eqtype_name;
            }
            set
            {
                _eqtype_name = value;
                switch (value)
                {
                    case "openzwave":
                        ColSpan = 2;
                        break;

                    case "energy":
                        ColSpan = 1;
                        break;

                    default:
                        break;
                }
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "logicalId"
        /// </summary>
        public RelayCommand<object> ExecCommandByLogicalID
        {
            get
            {
                this._execCommandByLogicalID = this._execCommandByLogicalID ?? new RelayCommand<object>(async parameters =>
                 {
                     try
                     {
                         var cmd = cmds.Where(c => c.logicalId.ToLower() == parameters.ToString().ToLower()).First();
                         await ExecCommand(cmd);
                     }
                     catch (Exception) { }
                 });
                return this._execCommandByLogicalID;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "name"
        /// </summary>
        public RelayCommand<object> ExecCommandByName
        {
            get
            {
                this._execCommandByName = this._execCommandByName ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = cmds.Where(c => c.name.ToLower() == parameters.ToString().ToLower()).First();
                        await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._execCommandByName;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "generic_type"
        /// </summary>
        public RelayCommand<object> ExecCommandByType
        {
            get
            {
                this._execCommandByType = this._execCommandByType ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = cmds.Where(c => c.display.generic_type == parameters.ToString()).First();
                        await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._execCommandByType;
            }
        }

        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string name
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

        public bool OnVisibility
        {
            get
            {
                return _onVisibility;
            }
            set
            {
                _onVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public string Puissance
        {
            get
            {
                return _puissance;
            }
            set
            {
                _puissance = value;
                NotifyPropertyChanged();
            }
        }

        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
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

        #endregion Public Properties

        #region Public Methods

        public ObservableCollection<Command> GetActionsCmds()
        {
            IEnumerable<Command> results = cmds.Where(c => c.type == "action");
            return new ObservableCollection<Command>(results);
        }

        public ObservableCollection<Command> GetInformationsCmds()
        {
            if (cmds != null)
            {
                IEnumerable<Command> results = cmds.Where(c => c.type == "info");
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task ExecCommand(Command cmd)
        {
            if (cmd != null)
            {
                this.Updating = true;
                await RequestViewModel.GetInstance().ExecuteCommand(cmd);
                //await Task.Delay(TimeSpan.FromSeconds(3));
                await RequestViewModel.GetInstance().UpdateEqLogic(this);
                NotifyPropertyChanged("cmds");
                this.Updating = false;
            }
        }

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