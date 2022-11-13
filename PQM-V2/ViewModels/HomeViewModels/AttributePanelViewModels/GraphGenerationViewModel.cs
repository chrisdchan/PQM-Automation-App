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
        private int _pointsPerPlot;

        private bool _showDomainError;
        private string _domainError;

        public double xmin { get => _xmin; 
            set { 
                _xmin = value;
                onPropertyChanged(nameof(xmin));
            } }
        public double xmax { get => _xmax; 
            set { _xmax = value;
                onPropertyChanged(nameof(xmax));
            } } 
        public int pointsPerPlot { get => _pointsPerPlot; 
            set { _pointsPerPlot = value;
                onPropertyChanged(nameof(pointsPerPlot));
            } }

        public bool showDomainError { get => _showDomainError;
            set { _showDomainError = value;
                onPropertyChanged(nameof(showDomainError));
            }}

        public string domainError { get => _domainError;
            set { _domainError = value;
                onPropertyChanged(nameof(domainError));
            }}

        public string backgroundColorString { get; set; }

        public RelayCommand updateDomainCommand { get; private set; }
        public RelayCommand updateNumPointsCommand { get; private set; }

        public GraphGenerationViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;

            xmin = _graphCustomizeStore.xmin;
            xmax = _graphCustomizeStore.xmax;
            pointsPerPlot = _graphCustomizeStore.numPoints;

            _showDomainError = false;
            _domainError = string.Format("Value cannot be greater than max value {0}", _graphStore.graph.xmax);

            updateDomainCommand = new RelayCommand(updateDomain);
            updateNumPointsCommand = new RelayCommand(updateNumPoints);
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
    }
}
