using System;
using CretaBase;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CC3_GUI
{
    public class MachineState : ViewModelBase
    {
        #region members
        private bool _bOneLine;
       
        private bool _bFluent;
        private CretaTypes.EMachineStateMode _machineStateMode;
        private bool _isRightHand;
        #endregion

        public MachineState()
        {
            IsRightHand = true;
            IsOneLine = true;
            IsFluent = false;
            NumOfBars = 8;
            Bar1Exists = NumOfBars >= 1;
            Bar2Exists = NumOfBars >= 2;
            Bar3Exists = NumOfBars >= 3;
            Bar4Exists = NumOfBars >= 4;
            Bar5Exists = NumOfBars >= 5;
            Bar6Exists = NumOfBars >= 6;
            Bar7Exists = NumOfBars >= 7;
            Bar8Exists = NumOfBars >= 8;
            MachineStateMode = CretaTypes.EMachineStateMode.MACHINE_STATE_INVALID;
            BarColorsList = new List<Color>();
            BarWizardsState = new List<FluentWizardsState>();
            for (int i = 0; i < CretaTypes.PRINT_MAX_BARS; ++i )
            {
                BarColorsList.Add(Color.FromArgb(255,255,0,255));
                BarWizardsState.Add(new FluentWizardsState());
            }
        }

        public void FillInitialData(int iNumBars, bool bIsOneLine, bool bIsRightHand)
        {
            FillInitialData(iNumBars, bIsOneLine, bIsRightHand, GlobalState.MachineState.ElectronicsType);
        }
        public void FillInitialData(int iNumBars, bool bIsOneLine, bool bIsRightHand, CretaTypes.enumTipoElectronica electronicsType)
        {
#if PLOTTER
            IsPlotter = true;
#else
            IsPlotterBeltMode = true;//Por si acaso que funcione en modo banda si es MÁQUINA
            IsPlotter = false;
#endif
            ElectronicsType = electronicsType;
            IsRightHand = bIsRightHand;
            IsOneLine = bIsOneLine;
            NumOfBars = iNumBars;
            Bar1Exists = NumOfBars >= 1;
            Bar2Exists = NumOfBars >= 2;
            Bar3Exists = NumOfBars >= 3;
            Bar4Exists = NumOfBars >= 4;
            Bar5Exists = NumOfBars >= 5;
            Bar6Exists = NumOfBars >= 6;
            Bar7Exists = NumOfBars >= 7;
            Bar8Exists = NumOfBars >= 8;
        }

        #region properties
        public bool IsOneLine
        {
            get { return _bOneLine; }
            set
            {
                if (value != _bOneLine)
                {
                    _bOneLine = value;
                    NotifyPropertyChanged("IsOneLine");
                }
            }
        }
        public List<FluentWizardsState> BarWizardsState { get; set; }
        public bool IsFluent
        {
            get { return _bFluent; }
            set
            {
                if (value != _bFluent)
                {
                    _bFluent = value;
                    NotifyPropertyChanged("IsFluent");
                }
            }
        }
        public CretaTypes.EMachineStateMode MachineStateMode
        {
            get { return _machineStateMode; }
            set
            {
                if (value != _machineStateMode)
                {
                    _machineStateMode = value;
                    NotifyPropertyChanged("MachineStateMode");
                }
            }
        }
        public bool IsRightHand
        {
            get { return _isRightHand; }
            set
            {
                if (value != _isRightHand)
                {
                    _isRightHand = value;
                    NotifyPropertyChanged("IsRightHand");
                }
            }
        }
        public int NumOfBars { get; set; }
        public bool Bar1Exists { get; set; }
        public bool Bar2Exists { get; set; }
        public bool Bar3Exists { get; set; }
        public bool Bar4Exists { get; set; }
        public bool Bar5Exists { get; set; }
        public bool Bar6Exists { get; set; }
        public bool Bar7Exists { get; set; }
        public bool Bar8Exists { get; set; }
        public List<Color> BarColorsList { get; set; }
        public int NumOfSlaves { get; set; }
        public bool IsPlotter { get; set; }
        // amc: Propiedad para indicar la visibilidad de los textbox en modo scanning
        public bool IsPlotterScanMode { get; set; }
        public bool IsPlotterBeltMode { get; set; }
        public bool IsHighLevelUser { get; set; }
        public bool IsMedLevelUser { get; set; }
        public bool IsAdminLevelUser { get; set; }
        public CretaTypes.enumTipoElectronica ElectronicsType { get; set; }
        #endregion
    }

    public class FluentWizardsState : ViewModelBase
    {

        private string _imgLoadInkWizard;
        public string ImgLoadInkWizard
        {
            get
            {
                return this._imgLoadInkWizard;
            }
            set
            {
                if (value != this._imgLoadInkWizard)
                {
                    this._imgLoadInkWizard = value;
                    NotifyPropertyChanged("ImgLoadInkWizard");
                    NotifyPropertyChanged("ImgIsWizardRunnig");
                }
            }
        }

        private string _imgEmptyManifoldWizard;
        public string ImgEmptyManifoldWizard
        {
            get
            {
                return this._imgEmptyManifoldWizard;
            }
            set
            {
                if (value != this._imgEmptyManifoldWizard)
                {
                    this._imgEmptyManifoldWizard = value;
                    NotifyPropertyChanged("ImgEmptyManifoldWizard");
                    NotifyPropertyChanged("ImgIsWizardRunnig");
                }
            }
        }

        private string _imgEmptyDepositWizard;
        public string ImgEmptyDepositWizard
        {
            get
            {
                return this._imgEmptyDepositWizard;
            }
            set
            {
                if (value != this._imgEmptyDepositWizard)
                {
                    this._imgEmptyDepositWizard = value;
                    NotifyPropertyChanged("ImgEmptyDepositWizard");
                    NotifyPropertyChanged("ImgIsWizardRunnig");
                }
            }
        }

        private string _imgCleanManifoldWizard;
        public string ImgCleanManifoldWizard
        {
            get
            {
                return this._imgCleanManifoldWizard;
            }
            set
            {
                if (value != this._imgCleanManifoldWizard)
                {
                    this._imgCleanManifoldWizard = value;
                    NotifyPropertyChanged("ImgCleanManifoldWizard");
                    NotifyPropertyChanged("ImgIsWizardRunnig");
                }
            }
        }

        public int EmptyManifoldWizardStep { get; set; }
        public int EmptyDepositWizardStep { get; set; }
        public int LoadInkWizardStep { get; set; }
        public int CleanManifoldWizardStep { get; set; }

        public string ImgIsWizardRunnig
        {
            get
            {
                if ((this.ImgLoadInkWizard == CretaTypes.IMG_BUTTON_PLAY) ||
                        (this.ImgEmptyDepositWizard == CretaTypes.IMG_BUTTON_PLAY) ||
                        (this.ImgEmptyManifoldWizard == CretaTypes.IMG_BUTTON_PLAY) ||
                        (this.ImgCleanManifoldWizard == CretaTypes.IMG_BUTTON_PLAY))
                {
                    return CretaTypes.IMG_BUTTON_PLAY;
                }
                else
                {
                    return null;
                }
            }
        }

        public FluentWizardsState()
        {

            EmptyManifoldWizardStep = 0;
            ImgEmptyManifoldWizard = CretaTypes.IMG_BUTTON_STOP;

            EmptyDepositWizardStep = 0;
            ImgEmptyDepositWizard = CretaTypes.IMG_BUTTON_STOP;

            LoadInkWizardStep = 0;
            ImgLoadInkWizard = CretaTypes.IMG_BUTTON_STOP;

            CleanManifoldWizardStep = 0;
            ImgCleanManifoldWizard = CretaTypes.IMG_BUTTON_STOP;

        }

    }

}
