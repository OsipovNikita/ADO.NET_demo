using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace LINQsql_1
{
     [Table(Name = "Region")]
    class Region
    {
        private int _RegionID;
        [Column(IsPrimaryKey = true, Storage = "_RegionID")]
        public int RegionID
        {
            get
            {
                return this._RegionID;
            }
            set
            {
                this._RegionID = value;
            }

        }

        private string _RegionDescription;
        [Column(Storage = "_RegionDescription")]
        public string RegionDescription
        {
            get
            {
                return this._RegionDescription;
            }
            set
            {
                this._RegionDescription = value;
            }
        }



        public override string ToString()
        {
            return _RegionID + "\t" + _RegionDescription + "\t";
        }

       
    }
    
}
