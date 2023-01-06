using PQM_V2.Commands;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels
{
    public class GraphGenerationViewModel : BaseViewModel
    {
        private GraphStore _graphStore;
        private GraphCustomizeStore _graphCustomizeStore;

        private double _xmin;
        private double _xmax;
        private double _dpi;

        private bool _showDomainError;
        private string _domainError;
        private string _exportError;

        public double xmin { get => _xmin; 
            set { 
                _xmin = value;
                onPropertyChanged(nameof(xmin));
            } }
        public double xmax { get => _xmax; 
            set { _xmax = value;
                onPropertyChanged(nameof(xmax));
            } } 

        public double dpi { get => _dpi; set { _dpi = value; exportError = null;  onPropertyChanged(nameof(dpi)); } }
        public bool showDomainError { get => _showDomainError;
            set { _showDomainError = value;
                onPropertyChanged(nameof(showDomainError));
            }}

        public string domainError { get => _domainError;
            set { _domainError = value;
                onPropertyChanged(nameof(domainError));
            }}

        public string exportError { get => _exportError; set { _exportError = value; onPropertyChanged(nameof(exportError)); } }

        public string backgroundColorString { get; set; }

        public RelayCommand updateDomainCommand { get; private set; }
        public RelayCommand updateNumPointsCommand { get; private set; }
        public RelayCommand updateExportCommand { get; private set; }

        public GraphGenerationViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;

            xmin = _graphCustomizeStore.xmin;
            xmax = _graphCustomizeStore.xmax;
            dpi = _graphCustomizeStore.dpi;

            _showDomainError = false;
            _domainError = string.Format("Value cannot be greater than max value {0}", _graphStore.graph.xmax);

            updateDomainCommand = new RelayCommand(updateDomain);
            updateNumPointsCommand = new RelayCommand(updateNumPoints);
            updateExportCommand = new RelayCommand(updateExport);
        }
        private void updateDomain(object _)
        {
            showDomainError = true;

            if(_xmin < 0)
            {
                domainError = "Value cannot be less than 0";

            }
            else if (_graphStore.graph.xmax <= _xmax)
            {
                domainError = String.Format("Value cannot be greater than {0}", _graphStore.graph.xmax);
            }
            else if(_xmin >= _xmax)
            {
                domainError = "Xmin cannot be greater than Xmax";
            }
            else
            {
                showDomainError = false;
                _graphCustomizeStore.xmin = _xmin;
                _graphCustomizeStore.xmax = _xmax;
                _graphCustomizeStore.onGraphCustomizeChanged();
            }
        }
        private void updateNumPoints(object _)
        {
        }

        private void updateExport(object _)
        {
            if(dpi <= 0) { exportError = "DPI must be greater than 0"; return; }
            _graphCustomizeStore.dpi = dpi;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
    }
}
