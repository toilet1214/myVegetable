﻿using System.ComponentModel;

namespace prjVegetable.Models
{
    public class CGoodsInAndOutWrap
    {

        private TGoodsInAndOut _GoodsInAndOut = null;
        public TGoodsInAndOut GoodsInAndOut
        {
            get { return _GoodsInAndOut; }
            set { _GoodsInAndOut = value; }
        }

        public CGoodsInAndOutWrap()
        {
            _GoodsInAndOut = new TGoodsInAndOut();
        }

        [DisplayName("單號")]
        public int FId 
        {
            get { return _GoodsInAndOut.FId; }
            set { _GoodsInAndOut.FId = value; }
        }

        [DisplayName("進貨/出貨")]
        public int FInOut
        {
            get { return _GoodsInAndOut.FInOut; }
            set { _GoodsInAndOut.FInOut = value; }
        }

        [DisplayName("日期")]
        public DateOnly FDate
        {
            get { return _GoodsInAndOut.FDate; }
            set { _GoodsInAndOut.FDate = value; }
        }


        [DisplayName("發票號碼")]
        public int FInvoiceId
        {
            get { return _GoodsInAndOut.FInvoiceId; }
            set { _GoodsInAndOut.FInvoiceId = value; }
        }


        [DisplayName("供應商")]
        public int FProviderId
        {
            get { return _GoodsInAndOut.FProviderId; }
            set { _GoodsInAndOut.FProviderId = value; }
        }


        [DisplayName("購買者")]
        public int FPersonId
        {
            get { return _GoodsInAndOut.FPersonId; }
            set { _GoodsInAndOut.FPersonId = value; }
        }

        [DisplayName("商品總價")]
        public int FTotal
        {
            get { return _GoodsInAndOut.FTotal; }
            set { _GoodsInAndOut.FTotal = value; }
        }

        [DisplayName("編輯者")]
        public int FEditor
        {
            get { return _GoodsInAndOut.FEditor; }
            set { _GoodsInAndOut.FEditor = value; }
        }

        [DisplayName("備註")]
        public string? FNote
        {
            get { return _GoodsInAndOut.FNote; }
            set { _GoodsInAndOut.FNote = value; }
        }


        // 轉換 FInOut 為對應的文字
        public string FInOutText { get; set; }

        // 轉換 FPersonId 為對應的顧客名稱
        public string FPersonName { get; set; }

        // 轉換 FProviderId 為對應的供應商名稱
        public string FProviderName { get; set; }
    }

    
}
