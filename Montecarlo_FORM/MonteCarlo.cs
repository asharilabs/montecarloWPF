using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Montecarlo_FORM
{
    class MonteCarloForecast
    {
        private string mingguKe;
        private string random;
        private string taksiran;

        public string MingguKe { get { return this.mingguKe; } }
        public string Random { get { return this.random; } }
        public string Taksiran { get { return this.taksiran; } }

        public MonteCarloForecast(string _mingguke, string _random, string _taksiran)
        {
            this.mingguKe = _mingguke;
            this.random = _random;
            this.taksiran = _taksiran;
        }
    }
    class MonteCarloTable
    {
        private string kategori;
        private string frekuensi;
        private string prob;
        private string kumProb;
        private string interval;

        #region Properties
        public string Kategori
        {
            get { return this.kategori; }
            set { this.kategori = value; }
        }
        public string Frekuensi
        {
            get { return this.frekuensi; }
            set { this.frekuensi = value; }
        }
        public string Prob
        {
            get { return this.prob; }
            set { this.prob = value; }
        }
        public string KumProb
        {
            get { return this.kumProb; }
            set { this.kumProb = value; }
        }
        public string Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }
        #endregion

        #region _Constructor
        public MonteCarloTable(string _kat, string _frek, string _prob,
            string _kumProb, string _interval)
        {
            this.kategori = _kat;
            this.frekuensi = _frek;
            this.prob = _prob;
            this.kumProb = _kumProb;
            this.interval = _interval;
        }
        #endregion
    }
    class MonteCarlo
    {
        class IntervalMonteCarlo
        {
            private string batasBawah;
            private string batasAtas;

            public string BatasBawah 
            { get { return this.batasBawah; } }
            public string BatasAtas
            { get { return this.batasAtas; } }
            public IntervalMonteCarlo(string _bb, string _ba)
            {
                this.batasBawah = _bb;
                this.batasAtas = _ba;
            }
        }

        #region Atribut
        /// <summary>
        /// Himpun semua data mentah dari database
        /// </summary>
        List<int> dataMentah = new List<int>();        
        /// <summary>
        /// berisikan data-data yang telah dihimpun sebelumnya
        /// dari data mentah yang telah diurutkan
        /// outputnya: (Mis.) 0,1,2,3,4
        /// </summary>
        List<int> variablePenting = new List<int>();
        /// <summary>
        /// Himpun semua data hasil random ke variable
        /// collectionOfRandom, number of generate -nya
        /// tergantung berapa banyak forecast
        /// </summary>
        List<double> collectionOfRandom = new List<double>();
        /// <summary>
        /// himpun semua data interval batas bawah
        /// dan batas atas untuk setiap variable penting
        /// </summary>
        List<IntervalMonteCarlo> intervalMonteCarloList = new List<IntervalMonteCarlo>();
        /// <summary>
        /// Generate Number of Forecasting (How much forecast we want)
        /// </summary>
        private int nof;
        /// <summary>
        /// Get jumlah data dari sekumpulan data tunggal        
        /// </summary>
        private int jumlahData;
        #endregion

        #region Properties        
        public int BanyakDataMentah
        {
            get
            {
                return dataMentah.Count;
            }
        }
        public int JumlahData
        {
            get { return this.jumlahData; }
            private set { this.jumlahData = value; }
        }
        public List<int> VariablePenting
        {
            get { return this.variablePenting; }            
        }
        public int NOF { set { this.nof = value > 0 ? value : 0 ; } }

        #endregion

        #region _CONSTRUCTOR
        public MonteCarlo(List<int> data)
        {
            this.dataMentah.Clear();
            this.dataMentah = data;

            // klasifikasi variable
            this.KlasifikasiVariablePenting(this.dataMentah);

            // jumlah data
            this.jumlahData = GetJumlahData(this.dataMentah);
        }
        #endregion

        #region Fungsi
        private int CekBanyakDataPerVariable(List<int> _dataMentah, int _dataYangDicek)
        {
            int _banyak = 0;
            
            for (int i = 0; i < _dataMentah.Count; i++)
            {
                if (_dataYangDicek == _dataMentah[i]) _banyak++;
            }

            return _banyak;
        }
        public List<MonteCarloTable> GenerateMonteCarloTable()
        {
            List<MonteCarloTable> _dataMonteCarlo = new List<MonteCarloTable>();

            int banyakKategori = this.variablePenting.Count;
            List<double> probKumList = new List<double>();

            for (int i = 0; i < banyakKategori; i++)
            {
                int banyakDataPerVar = this.CekBanyakDataPerVariable(this.dataMentah, this.variablePenting[i]);
                double probabilitas = (double)banyakDataPerVar / (double) this.BanyakDataMentah;
                probKumList.Add(i == 0 ? probabilitas : (probKumList[i - 1] + probabilitas));
                
                IntervalMonteCarlo imc = new IntervalMonteCarlo(
                    i == 0 ? 0.ToString() : (probKumList[i - 1] + 0.001).ToString(),
                    probKumList[i].ToString()
                    );

                // himpun interval monte carlo ke list
                this.intervalMonteCarloList.Add(imc);

                //System.Windows.MessageBox.Show(
                //    "permintaan: " + this.variablePenting[i] +
                //    "\nfrek: " + banyakDataPerVar +
                //    "\nprob: " + probabilitas +
                //    "\nprobkum: " + probKumList[i]);

                MonteCarloTable mcd = new MonteCarloTable(
                    this.variablePenting[i].ToString(),
                    banyakDataPerVar.ToString(),
                    (Math.Round(probabilitas, 3)).ToString(),
                    (Math.Round(probKumList[i], 3)).ToString(),
                    Math.Round(double.Parse(imc.BatasBawah), 3) + " - " +
                    Math.Round(double.Parse(imc.BatasAtas), 3));

                _dataMonteCarlo.Add(mcd);
            }
            
            return _dataMonteCarlo;
        }
        int GetJumlahData(List<int> _datamentah)
        {
            int _jumlah = 0;

            foreach (int item in _datamentah)
            {
                _jumlah += item;
            }

            return _jumlah;
        }
        void KlasifikasiVariablePenting(List<int> data)
        {
            int _data = -9999;

            foreach (int item in data)
            {
                if(item != _data)
                {
                    this.variablePenting.Add(item);
                    _data = item;                    
                }
            }
        }
        private int GetDataHasilTaksiran(double _hasilRandom)
        {
            int nilaiTaksiran = -1;

            int _index = 0;
            int banyakKategori = this.variablePenting.Count;

            for (int i = 0; i < banyakKategori; i++)
            {
                if( _hasilRandom < 
                    double.Parse(this.intervalMonteCarloList[i].BatasAtas))
                {
                    break;
                }
                else
                {
                    _index++;
                }
            }

            nilaiTaksiran = this.variablePenting[_index];

            return nilaiTaksiran;
        }
        public List<MonteCarloForecast> Penaksiran()
        {
            List<MonteCarloForecast> mcf = new List<MonteCarloForecast>();
            Random random = new Random();

            int initForecast = this.BanyakDataMentah + 1;
            int lastForecast = initForecast + this.nof;

            //System.Windows.MessageBox.Show("init: " + initForecast +
            //    ", last: " + lastForecast);

            for (int i = initForecast; i < lastForecast; i++)
            {
                int getRandNumber = (lastForecast - initForecast) - this.nof;
                double _rand = random.NextDouble();

                MonteCarloForecast _mcf = new MonteCarloForecast(
                    i.ToString(),
                    Math.Round(_rand, 3).ToString(),
                    GetDataHasilTaksiran(_rand).ToString()
                    );

                this.collectionOfRandom.Add(_rand);

                mcf.Add(_mcf);
            }

            return mcf;
        }
        #endregion
    }
}
