using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfor
    {
        private int iD;
        private int iDBill;
        private int iDFood;
        private int count;

        public int ID { get => iD; set => iD = value; }
        public int IDBill { get => iDBill; set => iDBill = value; }
        public int IDFood { get => iDFood; set => iDFood = value; }
        public int Count { get => count; set => count = value; }

        public BillInfor(int id, int idBill, int iDFood, int count)
        {
            this.ID = id;
            this.IDBill = idBill;
            this.IDFood = iDFood;
            this.Count = count;
        }

        public BillInfor(DataRow row)
        {
            this.ID = (int)row["Id"];
            this.IDBill = (int)row["IdBill"];
            this.IDFood = (int)row["IDFood"];
            this.Count = (int)row["Count"];
        }
    }
}