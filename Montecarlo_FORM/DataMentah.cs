using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Montecarlo_FORM
{
    class DataMentah
    {
        #region Atribut dan Properties
        private string mingguke;
        private string frekuensiTerjual;
        public string MingguKe
        {
            get { return this.mingguke; }
            set { this.mingguke = value; }
        }
        public string FrekuensiTerjual
        {
            get { return this.frekuensiTerjual; }
            set { this.frekuensiTerjual = value; }
        }
        #endregion

        #region Constructor
        public DataMentah(string _mingguke, string _terjual)
        {
            this.mingguke = _mingguke;
            this.frekuensiTerjual = _terjual;
        }
        #endregion
    }
}
