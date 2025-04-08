using System.ComponentModel;

namespace AlkoLib
{
    public class Beverage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _name;
        private double _volume, _abv, _price, _spejson, _ethanol, tmpAbv;
        public Beverage()
        {
            _name = string.Empty;
            _volume = 0.0;
            _abv = 0.0;
            _price = 0.0;
            _spejson = 0.0;
            _ethanol = 0.0;
            tmpAbv = 0.0;
        }
        public Beverage(string name, double volume, double abv, double price, double spejson, double ethanol)
        {
            _name = name;
            _volume = volume;
            _abv = abv;
            _price = price;
            _spejson = spejson;
            _ethanol = ethanol;
        }

        public void calculateCostEfficiency()
        {
            tmpAbv = _abv;
            if (_volume != 500.0)
            {
                double podzial = 500.0 / _volume;
                tmpAbv = tmpAbv / podzial;
            }
            double wspolczynnik = tmpAbv / _price;
            _spejson = double.Round(wspolczynnik, 3);
        }

        public void calculateEthanol()
        {
            double realprocent = tmpAbv / 100;
            double ilealko = realprocent * _volume;
            if (_volume != 500.0)
            {                   
                double podzial = 500.0 / _volume;
                ilealko = ilealko * podzial;
            }
           _ethanol = double.Round(ilealko,3);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public double Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }
        public double ABV
        {
            get { return _abv; }
            set { _abv = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public double Spejson
        {
            get { return _spejson; }
            set { _spejson = value; }
        }
        public double Ethanol
        {
            get { return _ethanol; }
            set { _ethanol = value; }
        }

        public void setName(string nazwa)
        {
            _name = nazwa;
        }

        public void setVolume(double ml) {  _volume = ml; }
        public void setABV(double abv) {  _abv = abv; }
        public void setPrice(double price) {  _price = price; }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
